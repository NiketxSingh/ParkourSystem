public class PlayerFallState : PlayerBaseState
{
    public PlayerFallState(PlayerStateMachine ctx, PlayerStateFactory factory)
        : base(ctx, factory) {}

    public override void EnterState() {}

    public override void UpdateState()
    {
        if (ctx.isGrounded)
            ctx.SwitchState(factory.Land());
    }

    public override void ExitState() {}
}
