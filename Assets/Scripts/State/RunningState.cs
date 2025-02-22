using UnityEngine;
public class RunningState : BaseState
{
    public RunningState(PlayerStateMachine player) : base(player) { }

    public override void EnterState()
    {
        Debug.Log("Entered Running State");
        if (player.controller.isAiming)
        {
            player.animator.SetBool("isAiming", true);
        }
        else
        {
            player.animator.SetBool("isWalking",true);
        }

    }

    public override void ExitState()
    {
        Debug.Log("Exited Running State");
        player.animator.SetBool("isWalking",false);
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
        else if (player.controller.isAttacking)
        {
            player.TransitionToState(new AttackState(player));
        }
        else if (player.controller.isGuarding)
        {
            player.TransitionToState(new GuardState(player));
        }
    }

    public override BaseState GetNextState()
    {
        return null;
    }
}
