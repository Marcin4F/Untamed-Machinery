using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    CharacterController controller;
    Animator animator;
    private Camera mainCamera;

    Vector3 playerVelocity;
    bool isPointing = false, isMoving = false, groundedPlayer = false;
    Vector3 move;
    Vector3 moveAmount;

    [SerializeField] float runningSpeed = 3.0f;
    [SerializeField] float walkingSpeed = 1.0f;
    [SerializeField] float rotationSpeed = 720.0f;
    [SerializeField] float gravityValue = -9.81f;


    [SerializeField] private LayerMask groundMask;

    public delegate void FiringGun();
    public static event FiringGun firingGun;
    Shooting shooting;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        shooting = GetComponentInChildren<Shooting>();

        mainCamera = Camera.main;
    }

    void Update()
    {
        CheckIfPointing();
        CheckIfMoving(); 
        ProcessRotation();
        ProcessMovement();
        ProcessShot();
    }

    void CheckIfMoving()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
            playerVelocity.y = -2.0f;

        move = Vector3.forward * Input.GetAxis("Vertical");
        move += Vector3.right * Input.GetAxis("Horizontal");
        move.y = 0;
        // Debug.Log("X: " + move.x + ", Z: " + move.z);

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

        move.Normalize();
    }

    void CheckIfPointing()
    {
        if (Input.GetMouseButtonDown(1) && !isPointing)
        {
            isPointing = true;
            animator.SetBool("isPointing", true);
        }
        else if (Input.GetMouseButtonUp(1) && isPointing)
        {
            isPointing = false;
            animator.SetBool("isPointing", false);
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
            // to znaczy ze biega
            if (move != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(move, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            }
        }
        else if (isPointing)
        {
            // to znaczy ze jest wcelowany (albo chodzi albo stoi)
            var (success, position) = GetMousePosition();
            if (success)
            {
                // transform.forward = position - transform.position;
                // position.y = 0.0f;
                position.y = transform.position.y;
                transform.LookAt(position, Vector3.up);
            }
        }
    }

    void ProcessMovement()
    {
        // obsluga ruchu poziomego
        if (isMoving)
        {
            float currentSpeed = isPointing ? walkingSpeed : runningSpeed;
            controller.Move(move * currentSpeed * Time.deltaTime);
        }

        // grawitacja (schodzenie ze wzniesieñ)
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    void ProcessShot()
    {
        if (Input.GetMouseButton(0) && isPointing && shooting.shotReady && !shooting.isReloading)
        {
            animator.SetTrigger("shot");
            firingGun?.Invoke();
            shooting.shotReady = false;
            Player.instance.currentAmmo -= 1;
            InGameUI.instance.SetAmmo();
            if (Player.instance.currentAmmo == 0)
                StartCoroutine(shooting.Reloading());
            else
                StartCoroutine(shooting.ShootingCooldown());
        }
    }

    // https://www.youtube.com/watch?v=AOVCKEJE6A8
    private (bool success, Vector3 position) GetMousePosition()
    {
        var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        // Debug.Log("Ray Origin: " + ray.origin);
        // Debug.Log("Ray Direction: " + ray.direction);
        // Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 2.0f);

        if (Physics.Raycast(ray, out var hitInfo, 50.0f, groundMask))
        {
            // The Raycast hit something, return with the position.
            return (success: true, position: hitInfo.point);
        }
        else
        {
            // The Raycast did not hit anything.
            return (success: false, position: Vector3.zero);
        }
    }
}
