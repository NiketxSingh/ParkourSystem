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

        if (Input.GetKey(KeyCode.LeftShift))
            ctx.SwitchState(factory.Sprint());

        if (ctx.moveInput.magnitude < 0.1f)
            ctx.SwitchState(factory.Idle());

        if (Input.GetButtonDown("Jump") && !ctx.inParkourAction)
        {
            if(DetectObstacleAndAdjust()) return;
            ctx.SwitchState(factory.JumpUp());
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            ctx.SwitchState(factory.Crouch());
            return;
        }
        if(!ctx.inParkourAction) DetectObstacleAndAdjust(true);

    }
    bool DetectObstacleAndAdjust(bool auto = false)
    {
        float extraOffset = auto ? 0 : ctx.walkSpeed * 0.1f;
        var hit = ctx.environmentScanner.ObstacleCheck(auto ? 0f : extraOffset);

        if (hit.forwardHitFound)
        {
            foreach(var action in ctx.parkourActions)
            {
                if(action.CheckIfPossible(hit, ctx.transform) && (auto ? action.AnimName == "StepUp" : true))
                {
                    ctx.currentParkourAction = action;
                    Debug.Log("Executing Parkour Action: " + action.AnimName);
                    ctx.SwitchState(factory.Parkour());
                    return true;
                }
            }
        }
        return false;
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
