using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessingEffects : MonoBehaviour
{
    public Volume volume;
    public Color color1 = Color.red;
    public Color color2 = Color.blue;
    private Vignette vignette;
    public float colorChangeSpeed = 1f; // Speed of color interpolation

    void Start()
    {
        volume = GetComponent<Volume>();

        if (volume.profile.TryGet(out vignette))
        {
            StartCoroutine(ChangeColorOverTime());
        }
    }

    private IEnumerator ChangeColorOverTime()
    {
        float t = 0f;

        while (true)
        {
            if (vignette != null)
            {
                // PingPong returns a value that goes back and forth between 0 and 1
                float lerpValue = Mathf.PingPong(t * colorChangeSpeed, 1f);
                vignette.color.value = Color.Lerp(color1, color2, lerpValue);
                t += Time.deltaTime;
            }
            yield return null;
        }
    }
}
