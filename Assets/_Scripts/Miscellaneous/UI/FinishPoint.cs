using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishPoint : MonoBehaviour {
    [Header("Settings")]
    public LayerMask playerLayer;
    public string winSceneName = "WinScene";

    private void OnTriggerEnter(Collider other) {
        Debug.Log("Triggered by: " + other.gameObject.name);
        if (other.gameObject.CompareTag("Player")) {
            LoadWinScene();
        }
    }

    void LoadWinScene() {
        SceneManager.LoadScene(winSceneName);
    }
}
