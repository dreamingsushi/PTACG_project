using UnityEngine;

public class GuardState : BaseState
{
    public GuardState(PlayerStateMachine player) : base(player) { }

    public override void EnterState()
    {
        player.animator.SetBool("isGuarding", true);
        player.animator.SetBool("isAiming", true);
    }

    public override void ExitState()
    {
        player.animator.SetBool("isGuarding", false);
        player.animator.SetBool("isAiming", false);
    }

    public override void UpdateState()
    {
        if (!player.controller.isGuarding)
        {
            if (player.animator.GetFloat("Horizontal") == 0f && player.animator.GetFloat("Vertical") == 0f)
            {
                player.TransitionToState(new IdleState(player));
            }
            else if (player.controller.isJumping)
            {
                player.TransitionToState(new JumpingState(player));
            }
            else if (player.controller.m_Direction.magnitude > 0.1f)
            {
                player.TransitionToState(new RunningState(player));
            }
        }

    }

    public override BaseState GetNextState()
    {
        return null;
    }
}
