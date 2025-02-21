using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    public PlayerController controller;
    public Animator animator;
    private BaseState currentState;
    private float stateTimer;

    public float TimeInState => stateTimer; // Track time in current state

    void Start()
    {
        TransitionToState(new IdleState(this)); // Start in Idle state
    }

    void Update()
    {
        stateTimer += Time.deltaTime;
        currentState?.UpdateState(); // Call current state's Update

        float movementSpeed = controller.m_Direction.magnitude; // Get movement intensity
        animator.SetFloat("Speed", movementSpeed, 0.1f, Time.deltaTime); // Smooth transition

        animator.SetFloat("Horizontal", controller.m_Direction.x);
        animator.SetFloat("Vertical", controller.m_Direction.y);
    }

    public void TransitionToState(BaseState newState)
    {
        currentState?.ExitState(); // Exit current state
        currentState = newState;   // Assign new state
        stateTimer = 0f;           // Reset state timer
        currentState?.EnterState(); // Enter new state
    }


}
