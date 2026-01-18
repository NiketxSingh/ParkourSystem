using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

    [SerializeField] private GameObject controlsPanel;
    [SerializeField] private GameObject buttonPanel;
    [SerializeField] private GameObject creditsPanel;

    private void Awake() {
        controlsPanel.SetActive(false);
        buttonPanel.SetActive(true);
        creditsPanel.SetActive(false);
    }
    public void StartGame() {
        SceneManager.LoadScene("GameScene");
        Time.timeScale = 1f;
    }
    public void GoToMainMenu() {
        if(SceneManager.GetActiveScene().Equals("MainMenu")) {
            buttonPanel.SetActive(true);
            controlsPanel.SetActive(false);
            creditsPanel.SetActive(false);
        }
        else {
            SceneManager.LoadScene("MainMenu");
        }
        Time.timeScale = 1f;
    }
    public void ShowControls() {
        buttonPanel.SetActive(false);
        controlsPanel.SetActive(true);
        creditsPanel.SetActive(false);
        Time.timeScale = 1f;
    }
    public void ShowCredits() {
        buttonPanel.SetActive(false);
        controlsPanel.SetActive(false);
        creditsPanel.SetActive(true);
        Time.timeScale = 1f;
    }
    public void QuitGame() {
        Application.Quit();
        Debug.Log("Game Quit");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Stops play mode in editor
#endif
    }
    public void WinGame() {
        SceneManager.LoadScene("WinScene");
        Time.timeScale = 1f;
    }
}