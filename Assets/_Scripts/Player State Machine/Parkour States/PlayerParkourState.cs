using UnityEngine;

public class PlayerParkourState : PlayerBaseState
{
    EnvironmentScanner environmentScanner;
    Animator animator;
    bool wasSprinting;
    Vector3 startPos;
    Vector3 endPos;
    public PlayerParkourState(PlayerStateMachine ctx, PlayerStateFactory factory)
        : base(ctx, factory) { }

    public override void EnterState()
    {
        // Debug.Log("Enter Step Up State");
        environmentScanner = ctx.environmentScanner;
        animator = ctx.GetComponent<Animator>();
        
        ctx.SetControl(false);
        ctx.inParkourAction = true;
        wasSprinting = ctx.sprintToggled;
        ctx.sprintToggled = false;
        // animator.SetBool("inParkourAction", true);
        // animator.CrossFade(ctx.currentParkourAction.AnimName, 0.2f);

        if (ctx.currentParkourAction.ObstacleTag == "Vault")
        {
            if (ctx.environmentScanner.GetVaultHandTargets(
                out Vector3 left,
                out Vector3 right))
            {
                ctx.leftHandIK = left;
                ctx.rightHandIK = right;
                ctx.useIK = true;
            }
        }


        animator.Play(ctx.currentParkourAction.AnimName);
        if (ctx.currentParkourAction.RotateToObstacle)
        {
            Quaternion targetRot = ctx.currentParkourAction.targetRotation;
            ctx.transform.rotation = targetRot;
        }
    }

    public override void UpdateState()
    {
        // Debug.Log("Updating Step Up State");
        
        // if (ctx.currentParkourAction.TargetMatching)
        // {
        //     Debug.LogWarning("Matching Target");
        //     ctx.MatchTarget(ctx.currentParkourAction);
        // }
        
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);

        if (!info.IsName(ctx.currentParkourAction.AnimName))
            return;

        if (!ctx.currentParkourAction.TargetMatching)
            return;
        // float ikStart = ctx.currentParkourAction.MatchStartTime;
        // float ikEnd   = ctx.currentParkourAction.MatchTargetTime;

        
        if (info.normalizedTime >= ctx.currentParkourAction.MatchStartTime &&
            info.normalizedTime <= ctx.currentParkourAction.MatchTargetTime)
        {
            ctx.MatchTarget(ctx.currentParkourAction);
        }
        // if (info.normalizedTime < ikStart || info.normalizedTime > ikEnd)
        // {
        //     ctx.useIK = false;
        // }
        // else
        // {
        //     ctx.useIK = true;
        // }
        

    }

    

    public override void ExitState()
    {
        // Debug.LogWarning("Exit Step Up State");
        ctx.useIK = false;
        ctx.SetControl(true);
        ctx.inParkourAction = false;
        ctx.sprintToggled = wasSprinting;
    }
}
