using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    PlayerControls controls;
    public GameObject pauseMenuUI;
    public static bool paused;

    private void Awake() {
        controls = new PlayerControls();

        controls.UI.Pause.performed += ctx => PauseCheck();
    }
    private void OnEnable() {
        controls.Enable();
    }
    private void OnDisable() {
        controls.Disable();
    }
    public void PauseCheck() {
        if (paused) {
            Time.timeScale = 1;
            paused = false;
            pauseMenuUI.SetActive(false);
        } else {
            Time.timeScale = 0f;
            paused = true;
            pauseMenuUI.SetActive(true);
        }
    }
    public void EndGame() {
        SceneController.CharacterScene();
        Time.timeScale = 1;
        paused = false;
        pauseMenuUI.SetActive(false);
    }
}
