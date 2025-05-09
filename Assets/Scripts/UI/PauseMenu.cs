using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    private Canvas pauseCanvas;

    private bool isPaused = false;

    [SerializeField] private InputActionAsset PlayerControls;

    private InputAction pauseAction;

    private void Awake()
    {
        pauseCanvas = GetComponent<Canvas>();
        pauseCanvas.enabled = false;

        pauseAction = PlayerControls.FindActionMap("Player").FindAction("Pause");

        pauseAction.performed += ctx => TogglePause();
    }

    private void OnEnable()
    {
        pauseAction.Enable();
    }
    private void OnDisable()
    {
        pauseAction.Disable();
    }

    private void TogglePause()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    private void PauseGame()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (pauseCanvas != null)
        {
            pauseCanvas.enabled = true;
        }

        Time.timeScale = 0f;
        isPaused = true;
        PlayerControls.FindActionMap("Player").FindAction("Shoot").Disable();
    }

    public void ResumeGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (pauseCanvas != null)
        {
            pauseCanvas.enabled = false;
        }

        Time.timeScale = 1f;
        isPaused = false;
        PlayerControls.FindActionMap("Player").FindAction("Shoot").Enable();
    }

    public void ExitToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
