using UnityEngine;

public class DeadState : BaseState
{
    public DeadState(PlayerStateMachine player) : base(player) { }

    public override void EnterState()
    {
        player.animator.SetTrigger("isDead");
    }

    public override void ExitState()
    {
        player.animator.SetTrigger("Respawn");
    }

    public override void UpdateState()
    {
        if (player.health.currentHealth > 0)
        {
            player.TransitionToState(new IdleState(player));
        }
    }

    public override BaseState GetNextState()
    {
        return null;
    }
}
