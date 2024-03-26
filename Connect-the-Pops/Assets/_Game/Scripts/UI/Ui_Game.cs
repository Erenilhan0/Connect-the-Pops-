using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Ui_Game : UiBase
{
    [SerializeField] private TextMeshProUGUI scoreText;

    [SerializeField] private Image currentLevelImage;
    [SerializeField] private TextMeshProUGUI currentLevelText;

    [SerializeField] private Image nextLevelImage;
    [SerializeField] private TextMeshProUGUI nextLevelText;

    [SerializeField] private Slider levelProgressSlider;
    [SerializeField] private Image sliderImage;

    [SerializeField] private Color[] levelColors;

    private void Start()
    {
        GameManager.Instance.OnGamePhaseChange += OnGamePhaseChange;
        UiManager.Instance.OnScoreUpdate += OnScoreUpdate;
        SetLevelData();
    }

    private void OnScoreUpdate(int scoreUpdateAmount, int connectedPop)
    {
        UpdateScoreUI(scoreUpdateAmount, connectedPop);
    }

    private void OnGamePhaseChange(GamePhase obj)
    {
        if (obj == GamePhase.Game)
        {
            SetLevelData();
        }
    }

    private void SetLevelData()
    {
        levelProgressSlider.value = 0;

        var currentLevel = SaveLoad.I.PlayerProgress.CurrentLevel;

        currentLevelText.text = currentLevel.ToString();

        nextLevelText.text = (currentLevel + 1).ToString();

        var colorIndex = (currentLevel - 1) % levelColors.Length;

        var nextColorIndex = (currentLevel) % levelColors.Length;

        currentLevelImage.color = levelColors[colorIndex];

        sliderImage.color = levelColors[colorIndex];

        nextLevelImage.color = levelColors[nextColorIndex];

        var score = SaveLoad.I.PlayerProgress.Score;

        if (score >= 1000)
        {
            scoreText.text = (score / 1000).ToString("F1") + "K";
        }
        else
        {
            scoreText.text = score.ToString();
        }
    }


    public override void UpdateScoreUI(int updateAmount, int connectedCount)
    {
        var score = ScoreController.UpdateScore(updateAmount);

        if (score >= 1000)
        {
            scoreText.text = (score / 1000).ToString("F1") + "K";
        }
        else
        {
            scoreText.text = score.ToString();
        }

        if (connectedCount <= 2)
        {
            levelProgressSlider.value += 1;
        }
        else
        {
            levelProgressSlider.value += 2;
        }

        if (levelProgressSlider.value >= levelProgressSlider.maxValue)
        {
            GameManager.Instance.ChangeGameState(GamePhase.End);
        }
    }

    public override void HideUi()
    {
        transform.localScale = Vector3.zero;
    }


    public override void ShowUi()
    {
        transform.localScale = Vector3.one;
    }
}