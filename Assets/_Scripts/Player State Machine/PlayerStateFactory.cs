public class PlayerStateFactory
{
    PlayerStateMachine ctx;

    public PlayerStateFactory(PlayerStateMachine context)
    {
        ctx = context;
    }

    public PlayerBaseState Idle() => new PlayerIdleState(ctx, this);
    public PlayerBaseState Walk() => new PlayerWalkState(ctx, this);
    public PlayerBaseState Sprint() => new PlayerSprintState(ctx, this);
    public PlayerBaseState Crouch() => new PlayerCrouchState(ctx, this);
    public PlayerBaseState JumpUp() => new PlayerJumpUpState(ctx, this);
    public PlayerBaseState Fall() => new PlayerFallState(ctx, this);
    public PlayerBaseState Land() => new PlayerLandState(ctx, this);

    public PlayerBaseState Slide() => new PlayerSlideState(ctx, this);
    public PlayerBaseState Parkour() => new PlayerParkourState(ctx, this);

    public PlayerCutsceneState Cutscene() => new PlayerCutsceneState(ctx, this);
}
