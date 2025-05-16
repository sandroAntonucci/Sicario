using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UpdateFinalScore : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreResultText;
    [SerializeField] private TMP_Text scoreResultText2;
    [SerializeField] private TMP_Text gradeResultText;
    [SerializeField] private TMP_Text gradeResultText2;

    void Start()
    {
        scoreResultText.text = $"{GameManager.Instance.Score}";
        scoreResultText2.text = $"{GameManager.Instance.Score}";
        gradeResultText.text = $"{GameManager.Instance.LetterGrade}";
        gradeResultText2.text = $"{GameManager.Instance.LetterGrade}";

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(1);
    }

}
