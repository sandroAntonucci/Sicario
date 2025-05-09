using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorePointsEffect : MonoBehaviour
{

    void Start()
    {

        // Set initial rotation to look at the player
        Vector3 playerPosition = Camera.main.transform.position;
        Vector3 direction = (playerPosition - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(-direction);

        transform.rotation = lookRotation;



        StartCoroutine(ScoreEffect());
    }


    private IEnumerator ScoreEffect()
    {
        float duration = 0.5f;
        float elapsedTime = 0f;

        Vector3 startPosition = transform.position - new Vector3(0, -2, 0);
        Vector3 targetPosition = startPosition + new Vector3(0, 1, 0);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        Destroy(gameObject);
    }


}
