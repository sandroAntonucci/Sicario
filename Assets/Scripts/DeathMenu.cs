using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{

    public GameObject playerInterface;
    private GameObject player;

    public InputActionAsset PlayerControls;

    private InputAction restartAction;

    private void Awake()
    {
        restartAction = PlayerControls.FindAction("Restart");
        restartAction.performed += RestartGame;

        player = GameObject.FindGameObjectWithTag("Player");
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

        Time.timeScale = 1f;

        PickUpController.weaponEquipped = null;
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }


}
