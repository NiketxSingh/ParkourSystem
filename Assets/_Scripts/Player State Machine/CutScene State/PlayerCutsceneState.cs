using UnityEngine;
using System.Collections.Generic;

public class PlayerCutsceneState : PlayerBaseState {
    public PlayerCutsceneState(PlayerStateMachine ctx, PlayerStateFactory factory)
        : base(ctx, factory) {
    }

    public override void EnterState() {
        ctx.SetControl(false);

        ctx.velocity = Vector3.zero;
        ctx.animator.SetFloat("Speed", 0f);
    }

    public override void UpdateState() {
    }

    public override void ExitState() {
        ctx.SetControl(true);
    }
}
