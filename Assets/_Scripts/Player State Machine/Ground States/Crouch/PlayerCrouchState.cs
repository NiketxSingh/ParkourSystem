using UnityEngine;

public class PlayerCrouchState : PlayerBaseState
{
    public PlayerCrouchState(PlayerStateMachine ctx, PlayerStateFactory factory)
        : base(ctx, factory) {}

    public override void EnterState()
    {
        ctx.EnterCrouch();
        ctx.velocity.x = 0;
        ctx.velocity.z = 0;
    }

    public override void UpdateState()
    {
        Move(ctx.walkSpeed * 0.4f); // crouch speed

        // Toggle off crouch
        if (Input.GetKeyDown(KeyCode.C))
        {
            ctx.ExitCrouch();
            if (!ctx.IsCrouching)
                ctx.SwitchState(factory.Idle());
        }

        // Optional: auto-uncrouch on jump
        if (Input.GetButtonDown("Jump"))
        {
            ctx.ExitCrouch();
            if (!ctx.IsCrouching)
                ctx.SwitchState(factory.JumpUp());
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

        if (moveDir.sqrMagnitude > 0.01f)
        {
            Quaternion targetRot = Quaternion.LookRotation(moveDir);
            ctx.transform.rotation = Quaternion.Slerp(
                ctx.transform.rotation,
                targetRot,
                10f * Time.deltaTime
            );
        }

        ctx.animator.SetFloat("Speed", moveDir.magnitude, 0.1f, Time.deltaTime);
    }

    public override void ExitState() {}
}
