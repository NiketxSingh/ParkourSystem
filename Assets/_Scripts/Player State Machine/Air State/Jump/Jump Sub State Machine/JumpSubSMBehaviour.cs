using UnityEngine;

public class JumpSubSMBehaviour : StateMachineBehaviour
{
    override public void OnStateMachineEnter(
        Animator animator,
        int stateMachinePathHash)
    {
        // Optional: called when Jump SM starts
        // Debug.Log("Entered Jump Sub State Machine");
    }

    override public void OnStateMachineExit(
        Animator animator,
        int stateMachinePathHash)
    {
        // 🔥 This is what you want
        PlayerStateMachine psm =
            animator.GetComponent<PlayerStateMachine>();

        if (psm != null)
        {
            psm.SwitchState(psm.states.Idle());
        }
    }
}
