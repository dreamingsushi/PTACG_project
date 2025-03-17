using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Collections;
using Cinemachine;
using Unity.VisualScripting;
using UnityEditor.Rendering.LookDev;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public PlayerClass currentClass;
    public enum PlayerClass
    {
        Knight, Mage, Support
    }
    [Header("Components to Assign")]
    public Transform orientation;
    public CinemachineVirtualCamera cam;
    
    [Header("Movement Settings")]
    public float moveSpeed = 10;
    public float rotationSpeed = 5;
    public float gravity = -9.81f;
    
    [Header("Jump Settings")]
    public float jumpForce = 4;
    public float jumpCooldown = 0.2f;
    public float airMultiplier = 1;

    [Header("Ground Check")]
    public LayerMask groundLayer;

    [Header("Attack Settings")]
    public float attackDuration = 0.2f;
    
    [Header("Knight Settings")]
    [SerializeField] private float sprintSpeedMultiplier = 2f;
    [SerializeField] private float sprintDuration = 3f;
    [SerializeField] private float sprintCooldown = 10f;
    private float normalSpeed;
    private bool isSprinting = false;
    private bool canSprint = true;
    [SerializeField] private Image knightShift_CD;
    [SerializeField] private Image knightM1_CD;
    [SerializeField] private GameObject speedEffect;
    [Header("Mage Settings")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] public Transform staffTip;
    private bool canTeleport = true;
    [SerializeField] private float teleportCooldown = 1.5f;
    [SerializeField] private float teleportDistance = 5f;
    [SerializeField] private Image mageShift_CD;
    [SerializeField] private Image mageM1_CD;

    [Header("Support Settings")]
    [SerializeField] private GameObject supportProjectilePrefab;
    [SerializeField] public Transform supportHand;
    [SerializeField] private float projectileSpeed = 20f;
    [SerializeField] private Image supportShift_CD;
    [SerializeField] private Image supportM1_CD;
    [SerializeField] private GameObject healingCircle;
    [SerializeField] private bool canPlaceHealingCircle = true;
    [SerializeField] public Transform supportLeg;

    [Header ("Zhe King Ice Map Exclusive Settings")]
    public bool isOnIce;
    public bool iceMap;
    private Vector3 iceVelocity = Vector3.zero;
    [SerializeField] private float iceFriction = 1f;
    [SerializeField] private float iceAcceleration = 0.05f;

    [Header("Player Information (u dont have to edit :3)")]
    public bool isGrounded;
    public bool readyToJump;
    public Vector2 m_Direction;
    public bool canAttack = true;
    public bool canRotate = true;
    public bool isAttacking;
    public bool isJumping;
    public bool isGuarding;
    public bool isAiming;
    public bool isFalling;
    public bool isDead;

    private CharacterController controller;
    private CameraManager cameraManager;
    private Vector3 moveDirection;
    private Vector3 velocity;
    private PlayerInputs inputActions;
    // public event Action OnJumpEvent;
    // public event Action OnAttackEvent;
    public static PlayerController Instance;
    
    public void Shoot()
    {
        if (currentClass == PlayerClass.Mage)
        {
            if (projectilePrefab == null || staffTip == null) return;
            Vector3 shootDirection = cameraManager.GetAimDirection();
            GameObject projectile = Instantiate(projectilePrefab, staffTip.position, Quaternion.LookRotation(shootDirection));
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = shootDirection * projectileSpeed;
            }
        }
        if (currentClass == PlayerClass.Support)
        {
            if (supportProjectilePrefab == null || supportHand == null) return;
            Vector3 shootDirection = cameraManager.GetAimDirection();
            GameObject projectile = Instantiate(supportProjectilePrefab, supportHand.position, Quaternion.LookRotation(shootDirection));
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = shootDirection * projectileSpeed;
            }
        }

    }

    public void OnMove(InputAction.CallbackContext context)
    {
        m_Direction = context.ReadValue<Vector2>();
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            isJumping = true;
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (canAttack)
            {
                isAttacking = true;
                Attack();
            }
        }
    }
    public void OnSpecial(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            LookInfront();
            isAiming = true;
            AimCameraZoom();

            if (currentClass == PlayerClass.Knight) // Change this to match your system
            {
                canAttack = false;
                isGuarding = true;
            }
        }
        else if (context.canceled)
        {
            isAiming = false;
            AimCameraRelease();

            if (currentClass == PlayerClass.Knight)
            {
                canAttack = true;
                isGuarding = false;
            }
        }
    }
    public void OnOption(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            
        }
    }

    public void OnDodge(InputAction.CallbackContext context)
    {
        if (context.performed && isAiming && currentClass == PlayerClass.Mage)
        {
            TeleportForward();
        }

        if (context.performed && currentClass == PlayerClass.Knight)
        {
            StartCoroutine(Sprint());
        }

        if (context.performed && currentClass == PlayerClass.Support && canPlaceHealingCircle)
        {
            StartCoroutine(PlaceHealingCircle());
        }
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;
        inputActions.Player.Jump.performed += OnJump;
        inputActions.Player.Attack.performed += OnAttack;
        inputActions.Player.Special.performed += OnSpecial;
    }

    private void OnDisable()
    {
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Move.canceled -= OnMove;
        inputActions.Player.Jump.performed -= OnJump;
        inputActions.Player.Attack.performed -= OnAttack;
        inputActions.Player.Special.performed -= OnSpecial;
        inputActions.Player.Disable();
    }

    private void Awake()
    {
        Instance = this;
        inputActions = new PlayerInputs();
        controller = GetComponent<CharacterController>();
        cameraManager = GetComponent<CameraManager>(); 
    }

    private void Update()
    {
        if (isDead)
        {
            OnDisable();
            return;
        }
        GroundCheck();
        MovePlayer();
        if (!isAiming)
        {
            RotatePlayer();
        }
        ApplyGravity();
        
        isFalling = !isGrounded && velocity.y < 0;
    }

    private void GroundCheck()
    {

        isGrounded = controller.isGrounded;

        if (isGrounded)
        {
            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 1.5f, groundLayer))
            {
                isOnIce = hit.collider.CompareTag("IcyGround");
            }
            else
            {
                isOnIce = false;
            }
        }
    }

    private void MovePlayer()
    {
        Vector3 forward = orientation.forward;
        Vector3 right = orientation.right;

        forward.y = 0f;
        right.y = 0f;

        moveDirection = forward.normalized * m_Direction.y + right.normalized * m_Direction.x;
        float speed = isGrounded ? moveSpeed : moveSpeed * airMultiplier;

        if (isOnIce)
        {
            HandleIceMovement(speed);
        }
        else
        {
            controller.Move(moveDirection * speed * Time.deltaTime);
        }
    }

    private void RotatePlayer()
    {
        if (!canRotate) return;

        if (moveDirection != Vector3.zero)
        {
            Vector3 lookDir = new Vector3(moveDirection.x, 0f, moveDirection.z); // Ignore Y
            transform.forward = Vector3.Slerp(transform.forward, lookDir, Time.deltaTime * rotationSpeed);
        }
    }

    private void ApplyGravity()
    {
        if (!isGrounded)
        {
            velocity.y += gravity * Time.deltaTime;
        }
        else if (velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (!isGrounded && iceMap)
        {
            controller.Move(iceVelocity * Time.deltaTime);
        }
        controller.Move(velocity * Time.deltaTime);
    }


    private void Jump()
    {
        velocity.y = jumpForce;
    }

    public void JumpWithForce(float jumpMultiplier)
    {
        velocity.y = jumpForce*jumpMultiplier;
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    private void Attack()
    {
        canAttack = false;
        LookInfront();
        StartCoroutine(ResetAttackCooldown());
    }
    private IEnumerator ResetAttackCooldown()
    {
        float elapsedTime = 0f;

        if (currentClass == PlayerClass.Knight)
        {
            knightM1_CD.fillAmount = 1f;
        }
        else if (currentClass == PlayerClass.Mage)
        {
            mageM1_CD.fillAmount = 1f;
        }
        else if (currentClass == PlayerClass.Support)
        {
            supportM1_CD.fillAmount = 1f;
        }

        while (elapsedTime < attackDuration)
        {
            elapsedTime += Time.deltaTime;
            float fillAmount = 1f - (elapsedTime / attackDuration);

            if (currentClass == PlayerClass.Knight)
            {
                knightM1_CD.fillAmount = fillAmount;
            }
            else if (currentClass == PlayerClass.Mage)
            {
                mageM1_CD.fillAmount = fillAmount;
            }
            else if (currentClass == PlayerClass.Support)
            {
                supportM1_CD.fillAmount = fillAmount;
            }

            yield return null;
        }

        if (currentClass == PlayerClass.Knight)
        {
            knightM1_CD.fillAmount = 0f;
        }
        else if (currentClass == PlayerClass.Mage)
        {
            mageM1_CD.fillAmount = 0f;
        }
        else if (currentClass == PlayerClass.Support)
        {
            supportM1_CD.fillAmount = 0f;
        }

        canAttack = true;
    }

    public void LookInfront()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        Vector3 targetPoint;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity)) 
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(100); 
        }
        Vector3 lookDirection = targetPoint - transform.position;
        lookDirection.y = 0f;

        if (lookDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(lookDirection);
        }
        StartCoroutine(DisableRotationTemporarily(2f));
    }

    
    public void AimCameraZoom()
    {
        StartCoroutine(SmoothZoom(30f));
    }

    public void AimCameraRelease()
    {
        StartCoroutine(SmoothZoom(60f));
    }

    private IEnumerator SmoothZoom(float targetFOV)
    {
        float startFOV = cam.m_Lens.FieldOfView;
        float duration = 0.3f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            cam.m_Lens.FieldOfView = Mathf.Lerp(startFOV, targetFOV, elapsed / duration);
            yield return null;
        }

        cam.m_Lens.FieldOfView = targetFOV;
    }

    private IEnumerator DisableRotationTemporarily(float duration)
    {
        canRotate = false;
        yield return new WaitForSeconds(duration);
        canRotate = true;
    }

    private void HandleIceMovement(float speed)
    {
        if (moveDirection != Vector3.zero)
        {
            iceVelocity = Vector3.Lerp(iceVelocity, moveDirection * speed, iceAcceleration);
        }
        iceVelocity *= iceFriction; 
        controller.Move(iceVelocity * Time.deltaTime);
    }

    public void Teleport(Vector3 targetPosition)
    {
        controller.enabled = false;
        transform.position = targetPosition;
        controller.enabled = true;
    }

    public void TeleportForward()
    {
        if (!canTeleport) return;

        Vector3 teleportPosition = transform.position + transform.forward * teleportDistance;

        if (!Physics.Raycast(transform.position, transform.forward, teleportDistance))
        {
            controller.enabled = false;
            transform.position = teleportPosition;
            controller.enabled = true;
        }
        canTeleport = false;
        StartCoroutine(ResetTeleportCooldown());
    }

    private IEnumerator ResetTeleportCooldown()
    {
        float elapsedTime = 0f;
        mageShift_CD.fillAmount = 1f;

        while (elapsedTime < teleportCooldown)
        {
            elapsedTime += Time.deltaTime;
            mageShift_CD.fillAmount = 1f - (elapsedTime / teleportCooldown);
            yield return null;
        }
        mageShift_CD.fillAmount = 0f;
        canTeleport = true;
    }

    private IEnumerator Sprint()
    {
        if (!canSprint || isSprinting) yield break;

        isSprinting = true;
        canSprint = false;
        speedEffect.SetActive(true);

        normalSpeed = moveSpeed;
        moveSpeed *= sprintSpeedMultiplier;
        knightShift_CD.fillAmount = 1f;

        yield return new WaitForSeconds(sprintDuration);
        speedEffect.SetActive(false);
        moveSpeed = normalSpeed;
        isSprinting = false;

        float elapsedTime = 0f;

        while (elapsedTime < sprintCooldown)
        {
            elapsedTime += Time.deltaTime;
            knightShift_CD.fillAmount = 1f - (elapsedTime / sprintCooldown);
            yield return null;
        }

        knightShift_CD.fillAmount = 0f;
        canSprint = true;
    }

    private IEnumerator PlaceHealingCircle()
    {
        canPlaceHealingCircle = false;

        GameObject healingCircleInstance = Instantiate(healingCircle, supportLeg.position, Quaternion.identity);

        yield return new WaitForSeconds(10f);

        Destroy(healingCircleInstance);

        float elapsedTime = 0f;
        supportShift_CD.fillAmount = 1f;

        while (elapsedTime < 30f)
        {
            elapsedTime += Time.deltaTime;
            supportShift_CD.fillAmount = 1f - (elapsedTime / 30f);
            yield return null;
        }

        supportShift_CD.fillAmount = 0f;
        canPlaceHealingCircle = true;
    }


    public void ResetJump2() => isJumping = false;
    public void ResetAttack() => isAttacking = false;
    
}
