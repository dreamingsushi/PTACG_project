using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{


    [Header("Movement Settings")]
    public float moveSpeed;
    public float gravity = -9.81f;
    
    [Header("Jump Settings")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundDistance = 0.2f;
    public LayerMask groundLayer;


    public Transform orientation;

    private CharacterController controller;
    private Vector2 m_Direction;
    private Vector3 moveDirection;
    private Vector3 velocity;

    bool isGrounded;
    bool readyToJump;

    public void OnMove(InputAction.CallbackContext context)
    {
        m_Direction = context.ReadValue<Vector2>();
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {

        }
    }

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        GroundCheck();
        MovePlayer();
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

        forward.y = 0f; // Prevents looking down from affecting movement
        right.y = 0f;

        moveDirection = forward.normalized * m_Direction.y + right.normalized * m_Direction.x;
        float speed = isGrounded ? moveSpeed : moveSpeed * airMultiplier;

        controller.Move(moveDirection * speed * Time.deltaTime);
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
    
}
