using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI; 

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

    [Header("Dash Settings")]
    [SerializeField] float dashSpeed = 20.0f;
    [SerializeField] float dashDuration = 0.2f;
    [SerializeField] float dashCooldown = 2.0f;
    private bool isDashing = false;
    private bool canDash = true;

    public delegate void FiringGun();
    public static event FiringGun firingGun;
    Shooting shooting;

    [SerializeField] private AudioClip dashSound;

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
        if (isDashing) return; 

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
    }

    void CheckIfMoving()
    {
        float moveX = InGameUI.instance.moveJoystick != null ? InGameUI.instance.moveJoystick.Horizontal : 0f;
        float moveZ = InGameUI.instance.moveJoystick != null ? InGameUI.instance.moveJoystick.Vertical : 0f;

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

        float aimX = InGameUI.instance.aimJoystick != null ? InGameUI.instance.aimJoystick.Horizontal : 0f;
        float aimZ = InGameUI.instance.aimJoystick != null ? InGameUI.instance.aimJoystick.Vertical : 0f;
        Vector3 aimInput = new Vector3(aimX, 0, aimZ);
        
        bool isAimingWithJoystick = aimInput.magnitude > 0.1f;

        if (isAimingWithJoystick)
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
            if (move != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(move, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            }
        }
        else if (isPointing)
        {
            if (InGameUI.instance.aimJoystick != null && new Vector2(InGameUI.instance.aimJoystick.Horizontal, InGameUI.instance.aimJoystick.Vertical).magnitude > 0.1f)
            {
                Vector3 aimDirection = new Vector3(InGameUI.instance.aimJoystick.Horizontal, 0, InGameUI.instance.aimJoystick.Vertical);
                Quaternion toRotation = Quaternion.LookRotation(aimDirection, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * 2 * Time.deltaTime);
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
        bool tryingToShoot = isPointing && InGameUI.instance.aimJoystick != null && new Vector2(InGameUI.instance.aimJoystick.Horizontal, InGameUI.instance.aimJoystick.Vertical).magnitude > 0.1f;

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

    public void StartDash()
    {
        if (canDash && !isDashing)
        {
            StartCoroutine(DashRoutine());
        }
    }

    private IEnumerator DashRoutine()
    {
        AudioSource.PlayClipAtPoint(dashSound, transform.position);

        isDashing = true;
        canDash = false;

        if (InGameUI.instance.dashButtonImage != null)
        {
            Color c = InGameUI.instance.dashButtonImage.color;
            c.a = 10f / 255f;
            InGameUI.instance.dashButtonImage.color = c;
        }

        animator.speed = 0f; 
        float startTime = Time.time;
        Vector3 dashDirection = transform.forward; 

        while (Time.time < startTime + dashDuration)
        {
            controller.Move(dashDirection * dashSpeed * Time.deltaTime);
            yield return null; 
        }

        animator.speed = 1f; 
        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);

        if (InGameUI.instance.dashButtonImage != null)
        {
            Color c = InGameUI.instance.dashButtonImage.color;
            c.a = 1.0f;
            InGameUI.instance.dashButtonImage.color = c;
        }

        canDash = true;
    }
}