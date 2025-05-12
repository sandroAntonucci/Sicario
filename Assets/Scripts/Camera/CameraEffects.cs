using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEffects : MonoBehaviour
{

    public static CameraEffects Instance { get; private set; }

    public Camera mainCamera;

    public ProceduralRecoil recoil;

    public Coroutine ChangeFOVCoroutine;

    public float defaultFOV = 90f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        mainCamera = Camera.main;
        defaultFOV = OptionsMenu.fov;
        mainCamera.fieldOfView = defaultFOV;
    }

    public IEnumerator ChangeFOV(float targetFOV, float duration)
    {
        float startFOV = mainCamera.fieldOfView;
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            mainCamera.fieldOfView = Mathf.Lerp(startFOV, targetFOV, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        mainCamera.fieldOfView = targetFOV;
    }



}
