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

        if (!Input.GetKey(KeyCode.LeftShift) || ctx.moveInput.magnitude < 0.1f)
            ctx.SwitchState(factory.Walk());

        if (Input.GetKeyDown(KeyCode.LeftControl))
            ctx.SwitchState(factory.Slide());

        if (Input.GetButtonDown("Jump") && !ctx.inParkourAction)
        {
            if(DetectObstacleAndAdjust()) return;
            ctx.SwitchState(factory.JumpUp());
        }
    
        if (Input.GetKeyDown(KeyCode.C))
        {
            ctx.SwitchState(factory.Slide());
            return;
        }
        if(!ctx.inParkourAction) DetectObstacleAndAdjust(true);
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
    bool DetectObstacleAndAdjust(bool auto = false)
    {
        float extraOffset = auto ? 0 : ctx.sprintSpeed * 0.1f;
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

        ctx.animator.SetFloat("Speed", moveDir.magnitude * 2f, 0.1f, Time.deltaTime);
    }


    public override void ExitState() {}
}
