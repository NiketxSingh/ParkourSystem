public class PlayerJumpUpState : PlayerBaseState
{
    public PlayerJumpUpState(PlayerStateMachine ctx, PlayerStateFactory factory)
        : base(ctx, factory) {}

    public override void EnterState()
    {
        ctx.verticalVelocity = ctx.jumpForce;
        ctx.animator.SetTrigger("Jump");
    }

    public override void UpdateState()
    {
        if (ctx.verticalVelocity < 0)
            ctx.SwitchState(factory.Fall());
    }

    public override void ExitState() {}
}
