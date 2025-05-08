using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreUIHandler : MonoBehaviour
{

    public TextMeshProUGUI scoreText;

    public IEnumerator UpdateScore()
    {

        uint targetScore = ScoreSystem.Instance.score;
        uint score = uint.Parse(scoreText.text);

        float duration = 0.5f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            scoreText.text = Mathf.RoundToInt(Mathf.Lerp(score, targetScore, t)).ToString();
            yield return null;
        }


    }

}
