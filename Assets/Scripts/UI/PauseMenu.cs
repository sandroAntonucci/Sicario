using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

        pauseCanvas.enabled = true;
        Time.timeScale = 0f;
        isPaused = true;

        PlayerControls.FindActionMap("Player").FindAction("Shoot").Disable();
    }

    private void ResumeGame()
    {

        pauseCanvas.enabled = false;
        Time.timeScale = 1f;
        isPaused = false;

        PlayerControls.FindActionMap("Player").FindAction("Shoot").Enable();

    }

}
