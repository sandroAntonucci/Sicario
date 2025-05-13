using UnityEngine;
using TMPro;
using System.Collections;

public class PsychedelicTextEffect : MonoBehaviour
{
    public TMP_Text textMeshPro; // Assign your TextMeshPro component in the Inspector
    public Material textMaterial; // Assign the TextMeshPro material in the Inspector

    public bool useBackground = true; // Use background or not
    public bool useSideToSide = false; // Use side to side motion or not
    public bool useOrbit = true; // Use orbit motion or not
    public bool useColorChange = false; // Use color change or not
    public bool useScale = false;
    public bool useScaleForthAndBack = false; // Use scale forth and back or not
    public bool useFadeOut = false; // Use fade out effect or not

    public Color backgroundColorChange;

    public float textureScrollSpeed = 1.0f;
    public float orbitSpeed = 50f; // Speed of the orbit motion
    public float tiltAmount = 15f; // Max tilt amount (left to right)
    public float orbitRadius = 1f; // Distance from the center of rotation
    public float switchTime = 2.0f; // Time to change rotation direction
    public float tiltSpeed = 0.5f; // Speed of the tilting effect
    public float finalScale = 1.5f; // Final scale of the text
    public float scaleDuration = 1.0f; // Duration of the scaling effect
    public float fadeDuration = 1.0f; // Duration of the fade out effect

    private Vector3 orbitCenter;
    private float angle = 0f;

    private void Start()
    {
        if (textMeshPro == null) textMeshPro = GetComponent<TMP_Text>();
        if (textMaterial == null) textMaterial = textMeshPro.fontSharedMaterial;

        orbitCenter = transform.position;

        if (useBackground)
            StartCoroutine(ChangeBackgroundOverTime());

        if (useOrbit)
            StartCoroutine(OrbitAndTiltText());

        if (useSideToSide)
            StartCoroutine(SideToSide(0.5f, 20f, 0.01f));

        if (useColorChange)
            StartCoroutine(ChangeBackgroundColor(backgroundColorChange));

        if (useScale)
            StartCoroutine(ScaleText());

        if (useScaleForthAndBack)
            StartCoroutine(ScaleForthAndBack());

        if (useFadeOut)
            StartCoroutine(FadeOut());
    }

    private IEnumerator ChangeBackgroundOverTime()
    {
        Vector4 faceTex_ST = textMaterial.GetVector("_FaceTex_ST");

        float minOffset = -0.5f;
        float maxOffset = 0.5f;

        while (useBackground)
        {
            float elapsedTime = 0f;

            while (elapsedTime < 1f)
            {
                faceTex_ST.z = Mathf.Lerp(maxOffset, minOffset, elapsedTime);
                textMaterial.SetVector("_FaceTex_ST", faceTex_ST);
                elapsedTime += Time.deltaTime * textureScrollSpeed;
                yield return null;
            }

            faceTex_ST.z = maxOffset;
            textMaterial.SetVector("_FaceTex_ST", faceTex_ST);
            yield return null;
        }
    }

    private IEnumerator OrbitAndTiltText()
    {
        float elapsedTime = 0f;

        while (useOrbit)
        {

            angle += orbitSpeed * Time.deltaTime;

            float x = Mathf.Cos(angle) * orbitRadius;
            float y = Mathf.Sin(angle) * orbitRadius;

            transform.position = orbitCenter + new Vector3(x, y, 0);

            float tiltAngle = Mathf.Sin(elapsedTime * tiltSpeed) * tiltAmount; 
            transform.rotation = Quaternion.Euler(0, 0, tiltAngle);

            elapsedTime += Time.deltaTime;

            yield return null;
        }
    }

    private IEnumerator SideToSide(float duration, float speed, float distance)
    {
        float elapsedTime = 0f;
        Vector3 startPosition = transform.position;

        while (elapsedTime < duration)
        {
            float xOffset = Mathf.Sin(Time.time * speed) * distance;
            transform.position = transform.position + new Vector3(xOffset, 0f, 0f);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = startPosition;
    }

    private IEnumerator ChangeBackgroundColor(Color color)
    {

        float duration = 1f;

        Color startColor = textMaterial.GetColor("_FaceColor");

        while (true)
        {

            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                textMaterial.SetColor("_FaceColor", Color.Lerp(startColor, color, elapsedTime / duration));
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            textMaterial.SetColor("_FaceColor", color);

            elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                textMaterial.SetColor("_FaceColor", Color.Lerp(color, startColor, elapsedTime / duration));
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            textMaterial.SetColor("_FaceColor", startColor);

        }

    }

    private IEnumerator ScaleText()
    {

        Vector3 originalScale = transform.localScale;
        Vector3 targetScale = originalScale * finalScale;

        float elapsedTime = 0f;

        while (elapsedTime < scaleDuration)
        {
            transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsedTime / scaleDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale;


    }

    private IEnumerator ScaleForthAndBack()
    {

        Vector3 originalScale = transform.localScale;
        Vector3 targetScale = originalScale * finalScale;


        while (useScaleForthAndBack)
        {
            float elapsedTime = 0f;

            while (elapsedTime < scaleDuration)
            {
                transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsedTime / scaleDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.localScale = targetScale;

            elapsedTime = 0f;

            while (elapsedTime < scaleDuration)
            {
                transform.localScale = Vector3.Lerp(targetScale, originalScale, elapsedTime / scaleDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.localScale = originalScale;

        }


    }

    private IEnumerator FadeOut()
    {

        Color originalColor = new Color(textMeshPro.color.r, textMeshPro.color.g, textMeshPro.color.b, 1);
        Color targetColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);

        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            textMeshPro.color = Color.Lerp(originalColor, targetColor, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        textMeshPro.color = targetColor;

    }

}
