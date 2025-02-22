using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Collections;

public class PlayerController : MonoBehaviour
{


    [Header("Movement Settings")]
    public float moveSpeed;
    public float rotationSpeed = 5;
    public float gravity = -9.81f;
    
    [Header("Jump Settings")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundDistance = 0.2f;
    public LayerMask groundLayer;

    [Header("Attack Settings")]
    public float attackDuration = 0.2f;


    public Transform orientation;

    private CharacterController controller;
    public Vector2 m_Direction;
    private Vector3 moveDirection;
    private Vector3 velocity;

    bool isGrounded;
    bool readyToJump;
    public bool canAttack;
    public bool isAttacking;
    public bool isJumping;
    public bool isGuarding;
    public bool isAiming;

    public event Action OnJumpEvent;
    public event Action OnAttackEvent;
    private PlayerInputs inputActions;

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
            isGuarding = true;
            canAttack = false;
            isAiming = true;
        }
        else if (context.canceled)
        {
            isGuarding = false;
            canAttack = true;
            isAiming = false;
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
        inputActions = new PlayerInputs();
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        GroundCheck();
        MovePlayer();
        if (!isAiming)
        {
            RotatePlayer();
        }

        ApplyGravity();
    }

    private void GroundCheck()
    {
        isGrounded = Physics.Raycast(groundCheck.position, Vector3.down, groundDistance, groundLayer);
    }

    private void MovePlayer()
    {
        // Get movement direction, but ignore Y to prevent unwanted vertical movement
        Vector3 forward = orientation.forward;
        Vector3 right = orientation.right;

        forward.y = 0f;
        right.y = 0f;

        moveDirection = forward.normalized * m_Direction.y + right.normalized * m_Direction.x;
        float speed = isGrounded ? moveSpeed : moveSpeed * airMultiplier;

        controller.Move(moveDirection * speed * Time.deltaTime);
    }

    private void RotatePlayer()
    {
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

        controller.Move(velocity * Time.deltaTime);
    }


    private void Jump()
    {
        velocity.y = jumpForce;
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
        yield return new WaitForSeconds(attackDuration);
        canAttack = true;
    }

    private void LookInfront()
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
    }

    private void Guard()
    {
        
    }
    

    public void ResetJump2() => isJumping = false; // Reset after jumping
    public void ResetAttack() => isAttacking = false; // Reset after attacking
    
}
