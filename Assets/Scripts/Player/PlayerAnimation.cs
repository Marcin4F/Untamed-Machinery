using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerAnimation : MonoBehaviour
{
    CharacterController controller;
    Animator animator;
    private Camera mainCamera;

    Vector3 playerVelocity;
    bool isPointing = false, isMoving = false;
    Vector3 move;

    private int currentSceneIndex;

    [SerializeField] float runningSpeed = 3.0f;
    [SerializeField] float walkingSpeed = 1.0f;
    [SerializeField] float rotationSpeed = 720.0f;
    [SerializeField] float gravityValue = -9.81f;
    [SerializeField] private LayerMask groundMask;

    [Header("Mobile Controls")]
    public MobileJoystick moveJoystick;
    public MobileJoystick aimJoystick;

    public delegate void FiringGun();
    public static event FiringGun firingGun;
    Shooting shooting;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        shooting = GetComponentInChildren<Shooting>();
        
        mainCamera = Camera.main;
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    void Update()
    {
        CheckIfMoving(); 
        CheckIfPointing();
        ProcessRotation();
        ProcessMovement();
        ProcessShot();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += UpdateSceneIndex;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= UpdateSceneIndex;
    }

    private void UpdateSceneIndex(Scene scene, LoadSceneMode mode)
    {
        currentSceneIndex = scene.buildIndex;
        mainCamera = Camera.main;

        GameObject moveObj = GameObject.Find("MoveJoystick_BG");
        if (moveObj != null) moveJoystick = moveObj.GetComponent<MobileJoystick>();

        GameObject aimObj = GameObject.Find("AimJoystick_BG");
        if (aimObj != null) aimJoystick = aimObj.GetComponent<MobileJoystick>();
    }

    void CheckIfMoving()
    {
        // Replace Input.GetAxis with Mobile Joystick input
        float moveX = moveJoystick != null ? moveJoystick.Horizontal : Input.GetAxis("Horizontal");
        float moveZ = moveJoystick != null ? moveJoystick.Vertical : Input.GetAxis("Vertical");

        move = Vector3.forward * moveZ + Vector3.right * moveX;
        move.y = 0;

        if (move.magnitude == 0)
        {
            if (isMoving)
            {
                isMoving = false;
                animator.SetBool("isMoving", false);
            }
        }
        else if (!isMoving)
        {
            isMoving = true;
            animator.SetBool("isMoving", true);
        }

        // Avoid normalizing if magnitude is very small to allow for slow walking on slight joystick pushes
        if (move.magnitude > 1f)
            move.Normalize();
    }

    void CheckIfPointing()
    {
        if (currentSceneIndex == 1)
        {
            if (isPointing)
            {
                isPointing = false;
                animator.SetBool("isPointing", false);
            }
            return;
        }

        // Get aim input from right joystick (fallback to PC Right-Click for testing)
        bool isAimingWithMouse = Input.GetMouseButton(1);
        float aimX = aimJoystick != null ? aimJoystick.Horizontal : 0;
        float aimZ = aimJoystick != null ? aimJoystick.Vertical : 0;
        Vector3 aimInput = new Vector3(aimX, 0, aimZ);

        // A deadzone of 0.1f prevents jittering when the joystick resets
        if (aimInput.magnitude > 0.1f || isAimingWithMouse)
        {
            if (!isPointing)
            {
                isPointing = true;
                animator.SetBool("isPointing", true);
            }
        }
        else
        {
            if (isPointing)
            {
                isPointing = false;
                animator.SetBool("isPointing", false);
            }
        }

        if (isPointing)
        {
            Vector3 relativeMove = Quaternion.Inverse(transform.rotation) * move;
            animator.SetFloat("pMovX", relativeMove.x, 0.2f, Time.deltaTime);
            animator.SetFloat("pMovZ", relativeMove.z, 0.2f, Time.deltaTime);
        }
    }

    void ProcessRotation()
    {
        if (!isPointing && isMoving)
        {
            // Player is just running, face movement direction
            if (move != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(move, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            }
        }
        else if (isPointing)
        {
            // Player is aiming via Mobile Joystick
            if (aimJoystick != null && new Vector2(aimJoystick.Horizontal, aimJoystick.Vertical).magnitude > 0.1f)
            {
                Vector3 aimDirection = new Vector3(aimJoystick.Horizontal, 0, aimJoystick.Vertical);
                Quaternion toRotation = Quaternion.LookRotation(aimDirection, Vector3.up);
                // We use RotateTowards so they snap fast but smoothly, adjust rotationSpeed if it feels too sluggish
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * 2 * Time.deltaTime);
            }
            // Fallback for PC mouse testing
            else if (Input.GetMouseButton(1))
            {
                var (success, position) = GetMousePosition();
                if (success)
                {
                    position.y = transform.position.y;
                    transform.LookAt(position, Vector3.up);
                }
            }
        }
    }

    void ProcessMovement()
    {
        if (isMoving)
        {
            float currentSpeed = isPointing ? walkingSpeed : runningSpeed;
            controller.Move(move * currentSpeed * Time.deltaTime);
        }

        if (currentSceneIndex == 1)
        {
            playerVelocity.y += gravityValue * Time.deltaTime;
            controller.Move(playerVelocity * Time.deltaTime);
        }
    }

    void ProcessShot()
    {
        // Auto-shoot when pointing (Mobile), or Left Click when pointing (PC fallback)
        bool tryingToShoot = (isPointing && aimJoystick != null && new Vector2(aimJoystick.Horizontal, aimJoystick.Vertical).magnitude > 0.1f) 
                             || (isPointing && Input.GetMouseButton(0));

        if (tryingToShoot && shooting.shotReady && !shooting.isReloading)
        {
            animator.SetTrigger("shot");
            firingGun?.Invoke();
            shooting.shotReady = false;
            Player.instance.currentAmmo -= 1;
            InGameUI.instance.SetAmmo();
            
            if (Player.instance.currentAmmo <= 0)
                StartCoroutine(shooting.Reloading());
            else
                StartCoroutine(shooting.ShootingCooldown());
        }
    }

    private (bool success, Vector3 position) GetMousePosition()
    {
        var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hitInfo, 50.0f, groundMask))
        {
            return (success: true, position: hitInfo.point);
        }
        return (success: false, position: Vector3.zero);
    }
}