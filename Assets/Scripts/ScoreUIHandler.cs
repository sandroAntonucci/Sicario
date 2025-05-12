using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreUIHandler : MonoBehaviour
{

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI scoreText2;
    public IEnumerator UpdateScore()
    {

        uint targetScore = ScoreSystem.Instance.score;
        uint score = uint.Parse(scoreText.text);

        float duration = 0.8f;
        float elapsedTime = 0f;

        StartCoroutine(ScoreEffect());

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            scoreText.text = Mathf.RoundToInt(Mathf.Lerp(score, targetScore, t)).ToString();
            scoreText2.text = Mathf.RoundToInt(Mathf.Lerp(score, targetScore, t)).ToString();
            yield return null;
        }

    }

    public IEnumerator ScoreEffect()
    {

        float duration = 0.2f;
        float elapsedTime = 0f;

        Vector3 scale = transform.localScale;
        Vector3 targetScale = scale + new Vector3(0.2f, 0.2f, 0.2f);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            transform.localScale = Vector3.Lerp(scale, targetScale, t);
            yield return null;
        }

        yield return new WaitForSeconds(0.4f);

        elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            transform.localScale = Vector3.Lerp(targetScale, scale, t);
            yield return null;
        }

    }
}