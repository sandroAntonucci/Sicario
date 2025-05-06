using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateFinalScore : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreResultText;
    [SerializeField] private TMP_Text gradeResultText;

    void Update()
    {
        scoreResultText.text = $"{ScoreSystem.Instance.score}/{ScoreSystem.Instance.minimumScoreForA}";
        gradeResultText.text = $"{ScoreSystem.Instance.GetScoreLetter()}";
    }
}
