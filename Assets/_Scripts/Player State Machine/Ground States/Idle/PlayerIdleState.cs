using UnityEngine;
public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerStateMachine ctx, PlayerStateFactory factory) : base(ctx, factory) {}

    public override void EnterState()
    {
        ctx.animator.SetFloat("Speed", 0);
        ctx.velocity.x = 0;
        ctx.velocity.z = 0;
    }

    public override void UpdateState()
    {
        if (ctx.moveInput.magnitude > 0.1f)
            ctx.SwitchState(factory.Walk());

        if (Input.GetButtonDown("Jump") && !ctx.inParkourAction)
        {
            var hit = ctx.environmentScanner.ObstacleCheck();

            if (hit.forwardHitFound)
            {
                foreach(var action in ctx.parkourActions)
                {
                    if(action.CheckIfPossible(hit, ctx.transform))
                    {
                        ctx.currentParkourAction = action;
                        ctx.SwitchState(factory.Parkour());
                        return;
                    }
                }
            }

            ctx.SwitchState(factory.JumpUp());
        }
        
        if (Input.GetKeyDown(KeyCode.C))
        {
            ctx.SwitchState(factory.Crouch());
            return;
        }

    }

    public override void ExitState() {}
}
