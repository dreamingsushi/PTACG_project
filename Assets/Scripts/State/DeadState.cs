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

    }

    public override void UpdateState()
    {
       
    }

    public override BaseState GetNextState()
    {
        return null;
    }
}
