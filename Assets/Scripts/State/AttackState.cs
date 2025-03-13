using UnityEngine;

public class AttackState : BaseState
{
    public AttackState(PlayerStateMachine player) : base(player) { }

    public override void EnterState()
    {
        player.controller.ResetAttack();
        player.animator.SetTrigger("Attack");
    }

    public override void ExitState()
    {

    }

    public override void UpdateState()
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
        else if (player.controller.isGuarding)
        {
            player.TransitionToState(new GuardState(player));
        }
        else if (player.controller.isAttacking)
        {
            player.TransitionToState(new AttackState(player));
        }
    }

    public override BaseState GetNextState()
    {
        return null;
    }
}
