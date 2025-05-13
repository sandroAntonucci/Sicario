using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PostProcessingSwap : MonoBehaviour
{

    [SerializeField] private Volume postProcessingOne;
    [SerializeField] private Volume postProcessingTwo;

    public Coroutine postProcessingCoroutine;

    private void Start()
    {
        postProcessingOne.weight = 1;
        postProcessingTwo.weight = 0;
    }

    public IEnumerator ActivateRush()
    {

        float duration = 0.3f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            postProcessingOne.weight = Mathf.Lerp(1, 0, t);
            postProcessingTwo.weight = Mathf.Lerp(0, 1, t);
            yield return null;
        }

        postProcessingCoroutine = null;
    }

    public IEnumerator DeactivateRush()
    {

        float duration = 0.3f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            postProcessingOne.weight = Mathf.Lerp(0, 1, t);
            postProcessingTwo.weight = Mathf.Lerp(1, 0, t);
            yield return null;
        }

        postProcessingCoroutine = null;
    }
}
