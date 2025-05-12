using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Audio;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] private PauseMenu pauseMenu;

    [SerializeField] private TextMeshProUGUI musicVolumeText;
    [SerializeField] private TextMeshProUGUI sfxVolumeText;
    [SerializeField] private TextMeshProUGUI sensitivityText;
    [SerializeField] private TextMeshProUGUI fovText;

    [SerializeField] private AudioMixer audioMixer;

    [SerializeField] private PlayerRotate playerAim;
    [SerializeField] private PlayerRotateSmooth playerAimSmooth;

    private static float musicVolume = 100f;
    private static float sfxVolume = 100f;
    public static float sensitivity = 100f;
    public static float fov = 90f;


    public static OptionsMenu Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        musicVolumeText.text = musicVolume.ToString();
        sfxVolumeText.text = sfxVolume.ToString();
        sensitivityText.text = sensitivity.ToString();
        fovText.text = fov.ToString();
    }

    private void OnEnable()
    {
        pauseMenu.enabled = false;  
    }

    private void OnDisable()
    {
        pauseMenu.enabled = true;
    }

    public void ChangeMusicVolume(bool increase)
    {
        float delta = 10f;
        if (increase)
            musicVolume = Mathf.Min(musicVolume + delta, 100f);
        else
            musicVolume = Mathf.Max(musicVolume - delta, 0f);

        // Avoid log(0) by clamping to a small positive value
        float linearVolume = Mathf.Clamp(musicVolume / 100f, 0.0001f, 1f);

        // Convert to decibels
        float dB = Mathf.Log10(linearVolume) * 20f;
        audioMixer.SetFloat("Music", dB);

        musicVolumeText.text = musicVolume.ToString();
    }

    public void ChangeSFXVolume(bool increase)
    {
        float delta = 10f;
        if (increase)
            sfxVolume = Mathf.Min(sfxVolume + delta, 100f);
        else
            sfxVolume = Mathf.Max(sfxVolume - delta, 0f);

        // Avoid log(0) by clamping to a small positive value
        float linearVolume = Mathf.Clamp(sfxVolume / 100f, 0.0001f, 1f);

        // Convert to decibels
        float dB = Mathf.Log10(linearVolume) * 20f;
        audioMixer.SetFloat("SFX", dB);

        sfxVolumeText.text = sfxVolume.ToString();
    }

    public void ChangeFOV(bool increase)
    {
        if (increase)
            fov = Mathf.Min(fov + 10f, 120f);
        else
            fov = Mathf.Max(fov - 10f, 60f);

        CameraEffects.Instance.defaultFOV = fov;  
        Camera.main.fieldOfView = fov;
        fovText.text = fov.ToString();  
    }

    public void ChangeSensitivity(bool increase)
    {
        if (increase)
            sensitivity = Mathf.Min(sensitivity + 10f, 180f);
        else
            sensitivity = Mathf.Max(sensitivity - 10f, 40f);

        playerAim._speed = sensitivity;
        playerAimSmooth._speed = sensitivity;

        sensitivityText.text = sensitivity.ToString();
    }   

    public void HideOptions()
    {
        gameObject.SetActive(false);
    }


}
