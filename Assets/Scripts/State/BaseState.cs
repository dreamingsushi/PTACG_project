using UnityEngine;

public abstract class BaseState
{
    protected PlayerStateMachine player; // Reference to the state machine

    public BaseState(PlayerStateMachine player)
    {
        this.player = player;
    }

    public abstract void EnterState();
    public abstract void ExitState();
    public abstract void UpdateState();
    public abstract BaseState GetNextState();

}
