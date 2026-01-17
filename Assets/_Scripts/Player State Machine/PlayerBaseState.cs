public abstract class PlayerBaseState
{
    protected PlayerStateMachine ctx;
    protected PlayerStateFactory factory;

    public PlayerBaseState(PlayerStateMachine context, PlayerStateFactory factory)
    {
        ctx = context;
        this.factory = factory;
    }

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
}
