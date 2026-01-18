using UnityEngine;

public class CutsceneHandler : MonoBehaviour {
    [Header("References")]
    public PlayerStateMachine playerStateMachine;

    public void StartCutscene() {
        if (playerStateMachine == null) {
            Debug.LogError("PlayerStateMachine not assigned!");
            return;
        }

        playerStateMachine.SwitchState(
            playerStateMachine.states.Cutscene()
        );
    }

    public void EndCutscene() {
        if (playerStateMachine == null) {
            Debug.LogError("PlayerStateMachine not assigned!");
            return;
        }

        playerStateMachine.SwitchState(
            playerStateMachine.states.Idle()
        );
    }
}

