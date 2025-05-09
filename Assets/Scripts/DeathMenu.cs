using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class DeathMenu : MonoBehaviour
{
    public GameObject playerInterface;
    private GameObject player;

    public VideoPlayer TVEffect;
    public GameObject TVTexture;

    // Instead of assigning this in the Inspector, we clone it at runtime
    public InputActionAsset PlayerControls;
    private InputActionAsset playerControlsInstance;

    private InputAction restartAction;

    private void Awake()
    {
        // Clone the InputActionAsset so it doesn't persist across scenes
        playerControlsInstance = Instantiate(PlayerControls);
        restartAction = playerControlsInstance.FindAction("Restart");

        if (restartAction != null)
        {
            restartAction.performed += RestartGame;
        }

        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnEnable()
    {
        restartAction?.Enable();
    }

    private void OnDisable()
    {
        restartAction?.Disable();
    }

    private void OnDestroy()
    {
        // Clean up the event handler to prevent stacking callbacks
        if (restartAction != null)
        {
            restartAction.performed -= RestartGame;
        }

        // Optionally destroy the clone to be extra safe
        if (playerControlsInstance != null)
        {
            Destroy(playerControlsInstance);
        }
    }

    private void RestartGame(InputAction.CallbackContext context)
    {
        // Reset time and gameplay state
        Time.timeScale = 1f;
        PickUpController.weaponEquipped = null;

        // Remove all binding overrides
        foreach (var map in playerControlsInstance.actionMaps)
        {
            foreach (var action in map.actions)
            {
                action.RemoveAllBindingOverrides();
            }
        }

        StartCoroutine(RestartGameCoroutine());
    }

    private IEnumerator RestartGameCoroutine()
    {
        TVTexture.SetActive(true);
        TVEffect.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.05f);

        while (!TVEffect.isPaused)
        {
            yield return null;
        }

        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }
}
