﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Transactions;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;

public class ScoreSystem : MonoBehaviour
{
    // Singleton
    private static ScoreSystem _instance;
    private Canvas finalScoreCanvas;

    [SerializeField] private GameObject finalMessage;
    [SerializeField] private GameObject finalDetection;

    // Vars
    public float baseMultiplier = 1;
    public float multiplier;
    public float baseMultiplierTime = 8;
    public float multiplierTime;
    public float multiplierDecrease = .1f;
    public uint score; // unsigned int can hold a bigger integer number =)

    public GameObject[] enemiesInLevel;
    public GameObject[] gunsInLevel;
    public int totalEnemyHealth;
    public float totalGunMultiplier = 1.25f;
    public float minimumScoreForA;
    public float scorePerLetter;
    public List<string> comboList = new List<string>();
    private uint scoreToAdd;

    [SerializeField] private ScoreUIHandler scoreUIHandler;
    [SerializeField] private MultiplierEffect multiplierEffect;
    [SerializeField] private PostProcessingSwap postProcessingEffects;

    // Coroutines
    public Coroutine multiplierCoroutine;

    // Events
    public delegate void AwardPointsHandler(int amount, string weapon = "");

    public event AwardPointsHandler awardPointsEvent;

    // Singleton

    public static ScoreSystem Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<ScoreSystem>();
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject("ScoreSystem");
                    _instance = singletonObject.AddComponent<ScoreSystem>();
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        InitScoreSystem();
    }

    private void InitScoreSystem()
    {
        multiplier = baseMultiplier;
        enemiesInLevel = GameObject.FindGameObjectsWithTag("Enemy");
        gunsInLevel = GameObject.FindGameObjectsWithTag("Gun");
        foreach (GameObject enemy in enemiesInLevel)
        {
            if (enemy.GetComponent<AIHandler>() != null)
            {
                totalEnemyHealth += enemy.GetComponent<AIHandler>().health;
            }
            else
            {
                enemiesInLevel = enemiesInLevel.Where(e => e != enemy).ToArray();
            }
        }
        foreach (GameObject GUN in gunsInLevel)
        {
            totalGunMultiplier += .25f;
        }
        minimumScoreForA = totalEnemyHealth * totalGunMultiplier;
        scorePerLetter = minimumScoreForA / 6;
    }

    private void OnEnable()
    {
        awardPointsEvent += AwardPointsEventCallback;
    }

    private void OnDisable()
    {
        awardPointsEvent -= AwardPointsEventCallback;
    }

    public void TriggerAwardPointsEvent(int amount, string weapon = "")
    {
        awardPointsEvent?.Invoke(amount, weapon);
    }

    private void AwardPointsEventCallback(int amount, string weapon = "")
    {
        multiplier = multiplier + 0.5f;
        multiplierTime = baseMultiplierTime;
        scoreToAdd = (uint)(amount * multiplier);
        score += scoreToAdd;

        if (scoreUIHandler != null)
        {
            StartCoroutine(scoreUIHandler.UpdateScore());
        }

        if (weapon != "")
        {
            if (comboList.Count == 0)
                comboList.Add(weapon);
            else if (comboList.Count > 0 && comboList[0] != weapon)
                comboList.Add(weapon);
        }

        if (multiplierCoroutine != null)
        {
            StopCoroutine(multiplierCoroutine);
        }
        multiplierCoroutine = StartCoroutine(MultiplierCoroutine());

    }

    private IEnumerator MultiplierCoroutine()
    {

        multiplierEffect.ChangeMultiplier(multiplier);

        if (postProcessingEffects.postProcessingCoroutine != null)
        {
            StopCoroutine(postProcessingEffects.postProcessingCoroutine);
        }

        postProcessingEffects.postProcessingCoroutine = StartCoroutine(postProcessingEffects.ActivateRush());


        while (multiplierTime > 0)
        {
            yield return new WaitForSeconds(multiplierDecrease);
            multiplierTime -= multiplierDecrease;
        }
        yield return new WaitForEndOfFrame();
        multiplier = baseMultiplier;

        if (postProcessingEffects.postProcessingCoroutine != null)
        {
            StopCoroutine(postProcessingEffects.postProcessingCoroutine);
        }

        postProcessingEffects.postProcessingCoroutine = StartCoroutine(postProcessingEffects.DeactivateRush());

        multiplierTime = 0;
        comboList.Clear();
    }

    public void CheckDeadEnemies()
    {

        bool allEnemiesDead = true;

        foreach (GameObject gO in enemiesInLevel)
        {
            if (gO != null)
            {
                if (gO.GetComponent<AIHandler>() != null)
                {
                    if (!gO.GetComponent<AIHandler>().isDead) allEnemiesDead = false;
                }
            }
        }

        if (allEnemiesDead)
        {
            if (finalMessage != null)
            {
                finalMessage.SetActive(true);
                finalDetection.SetActive(true);

                GameManager.Instance.Score = (int)score;
                GameManager.Instance.LetterGrade = GetScoreLetter();
            }
        }
    }

    public string GetScoreLetter()
    {
        float amount = score / scorePerLetter;
        if (amount > 6)
            return "S";
        else if (amount > 5)
            return "A";
        else if (amount > 4)
            return "B";
        else if (amount > 3)
            return "C";
        else if (amount > 2)
            return "D";
        else if (amount > 1)
            return "E";
        else
            return "F";
    }
}
