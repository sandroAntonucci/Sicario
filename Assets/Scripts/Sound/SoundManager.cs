using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    [SerializeField] private AudioClip audioClip;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayRandomPitch()
    {
        audioSource = UpdateAudioSource();

        if (audioSource != null)
        {
            audioSource.clip = audioClip;
            float randomPitch = Random.Range(0.95f, 1.05f);
            audioSource.pitch = randomPitch;
            audioSource.Play();
        }

    }

    public void PlaySound()
    {
        audioSource = UpdateAudioSource();

        if (audioSource != null)
        {
            audioSource.clip = audioClip;
            audioSource.Play();
        }
    }

    private AudioSource UpdateAudioSource()
    {

        AudioSource[] audioSources = GetComponents<AudioSource>();

        foreach (AudioSource source in audioSources)
        {
            if (!source.isPlaying)
            {
                return source;
            }
        }

        AudioSource newAudioSource = gameObject.AddComponent<AudioSource>();
        newAudioSource.playOnAwake = false;
        newAudioSource.loop = false;
        newAudioSource.volume = audioSource.volume;
        newAudioSource.outputAudioMixerGroup = audioSource.outputAudioMixerGroup;

        return newAudioSource;

    }

}