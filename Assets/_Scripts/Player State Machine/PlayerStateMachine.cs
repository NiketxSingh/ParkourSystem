using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(CharacterController))]
public class PlayerStateMachine : MonoBehaviour
{
    private float turnVelocity;
    [Header("Movement")]
    public float walkSpeed = 4f;
    public float sprintSpeed = 7f;
    public float jumpForce = 7f;
    public float gravity = -20f;
    public float TurnVelocity = 0.1f;

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
        currentState.UpdateState();
        if(!hasControl) return;
        ReadInput();
        ApplyGravity();
        controller.Move(velocity * Time.deltaTime);
        
    }

    void ReadInput()
    {

        moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        // Debug.Log("Move Input: " + moveInput);
    }
    public void SwitchState(PlayerBaseState newState)
    {
        currentState.ExitState();
        currentState = newState;
        currentState.EnterState();
    }

    void ApplyGravity()
    {
        
        if (controller.isGrounded && verticalVelocity < 0)
            verticalVelocity = -2f;
        else
            verticalVelocity += gravity * Time.deltaTime;
        animator.SetBool("isGrounded", controller.isGrounded);
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
        animator.SetBool("Crouching", true);
    }

    public bool CanStandUp()
    {
        float checkHeight = standHeight - crouchHeight;
        Vector3 origin = transform.position + Vector3.up * crouchHeight;

        return !Physics.SphereCast(
            origin,
            controller.radius,
            Vector3.up,
            out _,
            checkHeight,
            obstacleLayer
        );
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
    void OnAnimatorIK(int layerIndex)
    {
        if (!useIK) return;
        Debug.LogWarning("Applying IK");
        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);
        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1f);

        animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandIK);
        animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandIK);

        animator.SetIKRotation(
            AvatarIKGoal.LeftHand,
            Quaternion.LookRotation(transform.forward)
        );
        animator.SetIKRotation(
            AvatarIKGoal.RightHand,
            Quaternion.LookRotation(transform.forward)
        );
    }

}
