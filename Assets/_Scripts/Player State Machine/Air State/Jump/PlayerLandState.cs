using UnityEngine;
public class PlayerLandState : PlayerBaseState
{

    public PlayerLandState(PlayerStateMachine ctx, PlayerStateFactory factory)
        : base(ctx, factory) {}

    public override void EnterState()
    {
        ctx.velocity.x = 0;
        ctx.velocity.z = 0;
    }

    public override void UpdateState()
    {
        ctx.velocity.x = 0f;
        ctx.velocity.z = 0f;
        // if(ctx.controller.isGrounded)
        // {
        //     landTime -= Time.deltaTime;
        //     if (landTime <= 0)
        //         ctx.SwitchState(factory.Idle());
        // }
        
    }

    public override void ExitState() {}
}
