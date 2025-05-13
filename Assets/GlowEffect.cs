using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlowEffect : MonoBehaviour
{

    [SerializeField] private Image image;
    

    private IEnumerator GlowEffectCoroutine()
    {

        float duration = 1f;

        while (true)
        {
            float elapsedTime = 0f;

            Color originalColor = image.color;
            Color targetColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0.5f);

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.PingPong(elapsedTime / duration, 1);
                image.color = Color.Lerp(originalColor, targetColor, t);
                yield return null;
            }

            image.color = originalColor; // Reset to original color
        }

    }

}
