using UnityEngine;

public class BossMovement : MonoBehaviour
{
    public Animator animator;
    public float moveSpeed = 5f;   // Movement speed
    public float jumpForce = 5f;   // Jump force
    public float gravityMultiplier = 2f; // Gravity strength
    public bool isGrounded;
    public bool canJump;

    private Rigidbody rb;           // Reference to Rigidbody
    private float movementBlendSpeed = 0f;
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Handle player movement
        float moveX = Input.GetAxis("Horizontal"); // A/D or Left/Right Arrow
        float moveZ = Input.GetAxis("Vertical");   // W/S or Up/Down Arrow

        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        //animation
        if(movementBlendSpeed <= 0f)
        {
            movementBlendSpeed = 0f;
        }
        else if(movementBlendSpeed >= 1f)
        {
            movementBlendSpeed = 1f;
        }

        if(moveX >= 0.001f || moveZ >= 0.001f ){
            animator.SetBool("isWalking", true);
            movementBlendSpeed += 2f*Time.deltaTime;
            
        }
        else if(moveX <= 0.001f || moveZ <= 0.001f && movementBlendSpeed >= 1f)
        {
            movementBlendSpeed -= 2f*Time.deltaTime;
            animator.SetBool("isWalking", false);
            
        }

        animator.SetFloat("movement", movementBlendSpeed);

        // Apply movement
        rb.MovePosition(rb.position + move * moveSpeed * Time.deltaTime);

        // Handle jumping
        if (Input.GetButtonDown("Jump") && canJump)
        {
            
            
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            
        }

        if(!isGrounded)
        {
            animator.SetBool("isJumping", true);
            canJump = false;
        }
        else
        {
            animator.SetBool("isJumping", false);
            canJump = true;
        }
    }

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.layer == 6 && !isGrounded)
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision other) {
        if(other.gameObject.layer == 6 && isGrounded)
        {
            isGrounded = false;
        }
    }
}
