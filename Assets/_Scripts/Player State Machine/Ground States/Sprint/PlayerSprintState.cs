using UnityEngine;
public class PlayerSprintState : PlayerBaseState
{
    public PlayerSprintState(PlayerStateMachine ctx, PlayerStateFactory factory) : base(ctx, factory) {}

    public override void EnterState()
    {
        ctx.animator.SetBool("Sprinting", true);
    }

    public override void UpdateState()
    {

        if (!ctx.sprintToggled || ctx.moveInput.magnitude < 0.1f)
            {ctx.SwitchState(factory.Walk()); return;}

        if (Input.GetKeyDown(KeyCode.LeftControl))
            ctx.SwitchState(factory.Slide());

        if (Input.GetButtonDown("Jump") && !ctx.inParkourAction)
        {
            if (ctx.TryStartParkour(false, ctx.sprintSpeed, out _))
                return;
            ctx.SwitchState(factory.JumpUp());
        }
        ctx.TryStartParkour(true, ctx.sprintSpeed, out _);
        if (Input.GetKeyDown(KeyCode.C))
        {
            ctx.SwitchState(factory.Slide());
            return;
        }
        
        // move with sprint speed only when animation is playing
        if (ctx.animator.GetCurrentAnimatorStateInfo(0).IsName("Run"))
        {
            Move(ctx.sprintSpeed);
        }
        else
        {
            Move(ctx.walkSpeed);
        }
    }
    void Move(float speed)
    {
        Vector3 camForward = ctx.cameraTransform.forward;
        Vector3 camRight = ctx.cameraTransform.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDir =
            camForward * ctx.moveInput.y +
            camRight * ctx.moveInput.x;

        ctx.velocity.x = moveDir.x * speed;
        ctx.velocity.z = moveDir.z * speed;

        // GTA-style rotation
        if (moveDir.sqrMagnitude > 0.01f)
        {
            Quaternion targetRot = Quaternion.LookRotation(moveDir);
            ctx.transform.rotation = Quaternion.Slerp(
                ctx.transform.rotation,
                targetRot,
                12f * Time.deltaTime
            );
        }

        ctx.animator.SetFloat("Speed", moveDir.magnitude * 2f, 0.1f, Time.deltaTime);
    }


    public override void ExitState() {}
}
