using UnityEngine;
public class PlayerWalkState : PlayerBaseState
{
    public PlayerWalkState(PlayerStateMachine ctx, PlayerStateFactory factory) : base(ctx, factory) {}

    public override void EnterState()
    {
        ctx.animator.SetBool("Sprinting", false);
    }

    public override void UpdateState()
    {
        Move(ctx.walkSpeed);

        if (ctx.sprintToggled && ctx.moveInput.magnitude > 0.1f)
            {ctx.SwitchState(factory.Sprint()); return;}

        if (ctx.moveInput.magnitude < 0.1f)
            ctx.SwitchState(factory.Idle());

        if (Input.GetButtonDown("Jump") && !ctx.inParkourAction)
        {
            if (ctx.TryStartParkour(false, ctx.walkSpeed, out _))
                return;
            ctx.SwitchState(factory.JumpUp());
        }
        ctx.TryStartParkour(true, ctx.walkSpeed, out _);
        if (Input.GetKeyDown(KeyCode.C))
        {
            ctx.SwitchState(factory.Crouch());
            return;
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

        ctx.animator.SetFloat("Speed", moveDir.magnitude, 0.1f, Time.deltaTime);
    }


    public override void ExitState() {}
}
