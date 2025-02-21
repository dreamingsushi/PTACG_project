using UnityEngine;
public class JumpingState : BaseState
{
    public JumpingState(PlayerStateMachine player) : base(player) { }

    public override void EnterState()
    {
        player.controller.ResetJump2();
        player.animator.SetTrigger("Jump");
    }

    public override void ExitState()
    {
        Debug.Log("Exited Jumping State");
    }

    public override void UpdateState()
    {
        if (player.animator.GetFloat("Horizontal") == 0f && player.animator.GetFloat("Vertical") == 0f)
        {
            player.TransitionToState(new IdleState(player));
        }
        else if (player.controller.isAttacking)
        {
            player.TransitionToState(new AttackState(player));
        }
        if (player.controller.m_Direction.magnitude > 0.1f)
        {
            player.TransitionToState(new RunningState(player));
        }
    }

    public override BaseState GetNextState()
    {
        return null;
    }
}
