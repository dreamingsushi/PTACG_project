using Unity.Mathematics;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    public Animator animator;
    public float moveSpeed = 5f;   // Movement speed
    public float jumpForce = 5f;   // Jump force
    public float gravityStrength = 2f;
    public bool isGrounded;
    public bool canJump;
    public float turnMultiplier = 100f;

    private Rigidbody rb;          
    public Vector3 gravityForce;
    private float movementBlendSpeed = 0f;
    private BossController bossController;
    
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        bossController = GetComponent<BossController>();
        
    }

    void Update()
    {
        gravityForce = new Vector3(0, -9.81f*gravityStrength, 0);
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

        if(moveX != 0 || moveZ != 0 ){
            animator.SetBool("isWalking", true);
            movementBlendSpeed += 2f*Time.deltaTime;

            //transform.Rotate(-transform.up * turnMultiplier *bossController.turnDirection * Time.deltaTime);
            transform.Rotate(-transform.up * turnMultiplier *bossController.turnDirection);
            
            
            bossController.focusPoint.transform.parent.Rotate(transform.up * turnMultiplier*100 *bossController.turnDirection * Time.deltaTime);
            
            
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

        rb.AddForce(gravityForce, ForceMode.Acceleration);
    }

    private void OnCollisionStay(Collision other) {
        if(other.gameObject.layer == 6)
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

    public void JumpingWithForce(float jumpPower)
    {

        rb.AddForce(Vector3.up * jumpForce*jumpPower, ForceMode.Impulse);
    }
}
