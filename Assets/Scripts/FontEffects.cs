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

    public float textureScrollSpeed = 1.0f;
    public float orbitSpeed = 50f; // Speed of the orbit motion
    public float tiltAmount = 15f; // Max tilt amount (left to right)
    public float orbitRadius = 1f; // Distance from the center of rotation
    public float switchTime = 2.0f; // Time to change rotation direction
    public float tiltSpeed = 0.5f; // Speed of the tilting effect

    private Vector3 orbitCenter;
    private float angle = 0f;

    private void Start()
    {
        if (textMeshPro == null) textMeshPro = GetComponent<TMP_Text>();
        if (textMaterial == null) textMaterial = textMeshPro.fontSharedMaterial;

        orbitCenter = transform.position; // Assume the center is at the object's current position

        if (useBackground)
            StartCoroutine(ChangeBackgroundOverTime());

        if (useOrbit)
            StartCoroutine(OrbitAndTiltText());

        if (useSideToSide)
            StartCoroutine(SideToSide(0.5f, 20f, 0.01f)); // Adjust speed and distance as needed
    }

    private IEnumerator ChangeBackgroundOverTime()
    {
        Vector4 faceTex_ST = textMaterial.GetVector("_FaceTex_ST");

        float minOffset = -0.5f;
        float maxOffset = 0.5f;

        while (true)
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

        while (true)
        {
            // Orbit the text in a circular motion
            angle += orbitSpeed * Time.deltaTime; // Adjust the orbit speed

            // Calculate new position in circular motion
            float x = Mathf.Cos(angle) * orbitRadius;
            float y = Mathf.Sin(angle) * orbitRadius;

            // Apply the new position to the text
            transform.position = orbitCenter + new Vector3(x, y, 0);

            // Smoothly tilt the text from +tiltAmount to -tiltAmount and back
            float tiltAngle = Mathf.Sin(elapsedTime * tiltSpeed) * tiltAmount; // Smooth oscillating tilt
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
            // Move side-to-side using sine wave
            float xOffset = Mathf.Sin(Time.time * speed) * distance;
            transform.position = transform.position + new Vector3(xOffset, 0f, 0f);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Reset position after movement
        transform.position = startPosition;
    }
}
