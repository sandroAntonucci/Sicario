using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyIndicatorUI : MonoBehaviour
{
    public GameObject enemy;
    public Transform playerTransform;

    private Image image;
    private Color targetColor = Color.red;
    private float fadeDuration = 1.5f;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        image = GetComponent<Image>();
        if (image != null)
        {
            // Start with transparent red
            Color startColor = targetColor;
            startColor.a = 0;
            image.color = startColor;

            StartCoroutine(FadeInIndicator());
        }
    }

    private IEnumerator FadeInIndicator()
    {
        float time = 0f;
        Color startColor = image.color;
        Color endColor = targetColor;
        endColor.a = 1f;

        while (time < fadeDuration)
        {
            image.color = Color.Lerp(startColor, endColor, time / fadeDuration);
            time += Time.deltaTime;
            yield return null;
        }

        image.color = endColor;
    }

    void Update()
    {
        if (enemy != null)
        {
            Vector3 worldDirection = enemy.transform.position - playerTransform.position;
            Vector3 localDirection = playerTransform.InverseTransformDirection(worldDirection);
            float angle = Mathf.Atan2(localDirection.x, localDirection.z) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(0, 0, -angle);
        }

        if (enemy.GetComponent<AIHandler>().isDead)
        { 
            Destroy(gameObject);
        }
        
    }
}
