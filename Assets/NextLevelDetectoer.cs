using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelDetector : MonoBehaviour
{
    public GameObject backgroundMusic;

    private void Start()
    {
        if (backgroundMusic == null)
        {
            backgroundMusic = GameObject.FindGameObjectWithTag("BackgroundMusic");
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(backgroundMusic);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

}
