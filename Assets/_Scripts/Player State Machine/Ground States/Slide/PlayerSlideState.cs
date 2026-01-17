using UnityEngine;
public class PlayerSlideState : PlayerBaseState
{
    // float timer = 0.8f;

    public PlayerSlideState(PlayerStateMachine ctx, PlayerStateFactory factory) : base(ctx, factory) {}

    public override void EnterState()
    {
        ctx.animator.SetTrigger("Slide");
        ctx.controller.height = ctx.crouchHeight;
        ctx.controller.center = ctx.crouchCenter;
        ctx.sprintToggled = false;
    }

    public override void UpdateState()
    {
        // timer -= UnityEngine.Time.deltaTime;
        ctx.velocity = ctx.transform.forward * ctx.sprintSpeed * 1.1f;
        // if(ctx.animator)
        // if (timer <= 0)
        //     ctx.SwitchState(factory.Idle());
    }

    public override void ExitState()
    {
        ctx.sprintToggled = true;
    }
}
