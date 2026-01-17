using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(CharacterController))]
public class PlayerStateMachine : MonoBehaviour
{
    private float turnVelocity;
    [Header("Movement")]
    public float walkSpeed = 4f;
    public float sprintSpeed = 7f;
    public bool sprintToggled = false;
    public bool isGrounded;
    public float jumpForce = 7f;
    public float gravity = -20f;
    public float TurnVelocity = 0.1f;
    public float groundDistance = 0.1f;
    public Transform groundCheck;
    public LayerMask gLayer;

    [Header("Crouch")]
    public float standHeight = 2f;
    public float crouchHeight = 1.2f;
    public Vector3 standCenter = new Vector3(0, 1f, 0);
    public Vector3 crouchCenter = new Vector3(0, 0.6f, 0);
    public LayerMask obstacleLayer;
    public bool IsCrouching;
    [Header("Parkour")]
    public EnvironmentScanner environmentScanner;
    public List<ParkourActions> parkourActions;
    public bool inParkourAction = false;

    [Header("Parkour Runtime")]
    public ParkourActions currentParkourAction;
    [Header("IK")]
    public bool useIK;
    public Vector3 leftHandIK;
    public Vector3 rightHandIK;
    [Header("IK Runtime")]
    public float ikWeight;
    public float ikBlendSpeed = 8f;

    [Header("Foot IK")]
    public LayerMask groundLayer;
    [Range(0f, 1f)]
    public float footRayDistance;
    public float footOffset = 0.08f;



    [Header("References")]
    public Transform cameraTransform;

    // Components
    public CharacterController controller;
    public Animator animator;

    // Runtime
    public Vector3 velocity;
    public float verticalVelocity;
    public Vector2 moveInput;

    // State Machine
    PlayerBaseState currentState;
    bool hasControl = true;
    public PlayerStateFactory states;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        environmentScanner = GetComponent<EnvironmentScanner>();
        states = new PlayerStateFactory(this);
    }

    void Start()
    {
        currentState = states.Idle();
        currentState.EnterState();
    }

    void Update()
    {        
        UpdateGroundStatus();
        currentState.UpdateState();
        if(!hasControl) return;
        ReadInput();
        ApplyGravity();
        controller.Move(velocity * Time.deltaTime);
        
    }

    void ReadInput()
    {

        moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            sprintToggled = !sprintToggled;
        }
        // Debug.Log("Move Input: " + moveInput);
    }
    public void SwitchState(PlayerBaseState newState)
    {
        currentState.ExitState();
        currentState = newState;
        currentState.EnterState();
    }
    void UpdateGroundStatus()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, gLayer);
    }
    void ApplyGravity()
    {
        
        if (isGrounded && verticalVelocity < 0)
            verticalVelocity = -2f;
        else
            verticalVelocity += gravity * Time.deltaTime;
        // Debug.LogWarning("isGrounded: " + isGrounded);
        animator.SetBool("isGrounded", isGrounded);
        animator.SetFloat("VerticalVelocity", verticalVelocity);
        velocity.y = verticalVelocity;
    }
    // Apply Jump Force
    public void ApplyJump()
    {
        verticalVelocity = jumpForce;
    }

    // Crouch Methods

    public void EnterCrouch()
    {
        controller.height = crouchHeight;
        controller.center = crouchCenter;
        IsCrouching = true;
        sprintToggled = false;
        animator.SetBool("Crouching", true);
    }

    public bool CanStandUp()
    {
        float checkHeight = standHeight - crouchHeight;
        Vector3 origin = transform.position + Vector3.up * crouchHeight;
        bool hitObstacle = Physics.Raycast(
            origin,
            Vector3.up,
            checkHeight,
            obstacleLayer
        );
        return !hitObstacle;
    }

    public void ExitCrouch()
    {
        if (!CanStandUp()) return;
        
        controller.height = standHeight;
        controller.center = standCenter;
        IsCrouching = false;
        animator.SetBool("Crouching", false);
    }
    

    public void SetControl(bool control)
    {
        this.hasControl = control;
        controller.enabled = control;

        if(!control)
        {
            animator.SetFloat("Speed", 0);
            
            velocity = Vector3.zero;
        }
    }
    public void MatchTarget(ParkourActions action)
    {
        if(animator.isMatchingTarget) return;
        Debug.Log(action.MatchPos);
        animator.MatchTarget(
            action.MatchPos,
            transform.rotation,
            action.MatchBodyPart,
            new MatchTargetWeightMask(
                action.MatchPosWeight,
                0
            ),
            action.MatchStartTime,
            action.MatchTargetTime
        );
    }
    // void OnAnimatorIK(int layerIndex)
    // {
    //     Debug.Log("OnAnimatorIK called");
    //     // -------- FOOT IK (Locomotion) --------
    //     if (!inParkourAction && isGrounded)
    //     {
    //         animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1f);
    //         animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1f);
    //         animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1f);
    //         animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1f);
    //         // Left Foot
    //         RaycastHit hit;
            
    //         if(Physics.Raycast(animator.GetIKPosition(AvatarIKGoal.LeftFoot) + Vector3.up, Vector3.down, out hit, footRayDistance + 5f, groundLayer))
    //         {
    //             Vector3 footPos = hit.point;
    //             footPos.y += footRayDistance;
    //             animator.SetIKPosition(AvatarIKGoal.LeftFoot, footPos);
    //             animator.SetIKRotation(AvatarIKGoal.LeftFoot, Quaternion.LookRotation(transform.forward, hit.normal));
    //         }
    //         // Right Foot
    //         if(Physics.Raycast(animator.GetIKPosition(AvatarIKGoal.RightFoot) + Vector3.up, Vector3.down, out hit, footRayDistance + 5f, groundLayer))
    //         {
    //             Vector3 footPos = hit.point;
    //             footPos.y += footRayDistance;
    //             animator.SetIKPosition(AvatarIKGoal.RightFoot, footPos);
    //             animator.SetIKRotation(AvatarIKGoal.RightFoot, Quaternion.LookRotation(transform.forward, hit.normal));
    //         }
    //     }
    // }
    public bool TryStartParkour(
    bool autoTrigger,
    float speed,
    out ParkourActions selectedAction)
    {
        selectedAction = null;

        // Must be moving forward
        if (moveInput.y < 0.3f)
            return false;

        // Must have forward velocity
        if (new Vector3(velocity.x, 0, velocity.z).magnitude < speed * 0.5f)
            return false;

        var hit = environmentScanner.ObstacleCheck(
            autoTrigger ? 0f : speed * 0.4f
        );

        if (!hit.forwardHitFound)
            return false;

        float bestPriority = float.MinValue;

        foreach (var action in parkourActions)
        {
            if (!action.CheckIfPossible(hit, transform))
                continue;

            // Auto-trigger rules (StepUp only)
            if (autoTrigger && action.AnimName != "StepUp")
                continue;

            if (action.Priority > bestPriority)
            {
                bestPriority = action.Priority;
                selectedAction = action;
            }
        }

        if (selectedAction == null)
            return false;

        currentParkourAction = selectedAction;
        SwitchState(states.Parkour());
        return true;
    }

}
