using Unity.VisualScripting;
using UnityEngine;

public class IdleState : BaseState
{
    public IdleState(PlayerStateMachine player) : base(player) { }

    public override void EnterState()
    {
        Debug.Log("Entered Idle State");
    }

    public override void ExitState()
    {
        Debug.Log("Exited Idle State");
    }

    public override void UpdateState()
    {
        if (player.controller.m_Direction.magnitude > 0.1f)
        {
            player.TransitionToState(new RunningState(player));
        }
        else if (player.controller.isJumping)
        {
            player.TransitionToState(new JumpingState(player));
        }
        else if (player.controller.isAttacking)
        {
            player.TransitionToState(new AttackState(player));
        }
        else if (player.controller.isGuarding)
        {
            player.TransitionToState(new GuardState(player));
        }
        else if (player.health.currentHealth <= 0)
        {
            player.TransitionToState(new DeadState(player));
        }
    }

    public override BaseState GetNextState()
    {
        return null;
    }
}
