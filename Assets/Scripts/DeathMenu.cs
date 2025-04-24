using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{

    public InputActionAsset PlayerControls;

    private InputAction restartAction;

    private void Awake()
    {
        restartAction = PlayerControls.FindAction("Restart");
        restartAction.performed += RestartGame;
    }

    private void OnEnable()
    {
        restartAction.Enable();
    }

    private void OnDisable()
    {
        restartAction.Disable();
    }

    private void RestartGame(InputAction.CallbackContext context)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
