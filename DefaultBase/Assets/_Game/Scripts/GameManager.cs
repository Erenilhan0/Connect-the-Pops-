using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum  GamePhase{Menu,Game,End}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance{ get; private set; }
    public event Action<GamePhase> OnGamePhaseChange;
    public GamePhase currentGamePhase;
    
    [SerializeField] private TextMeshProUGUI scoreText;

    [SerializeField] private TextMeshProUGUI currentLevelText;
    [SerializeField] private TextMeshProUGUI nextLevelText;


    [SerializeField] private Slider levelProgressSlider;

    public float nextLevelScore;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SetLevelData();
    }


    public void SetLevelData()
    {
        currentLevelText.text = SaveLoad.I.playerProgress.currentLevel.ToString();
        nextLevelText.text = (SaveLoad.I.playerProgress.currentLevel+1).ToString();
        
        scoreText.text = SaveLoad.I.playerProgress.score.ToString();

    }


    public void UpdateScoreUI(int score)
    {
        if (score >= 1000)
        {
            scoreText.text = (score / 1000).ToString("F1") + "K";
        }
        else
        {
            scoreText.text = score.ToString();
        }

        levelProgressSlider.value += 2;
        if (levelProgressSlider.value >= levelProgressSlider.maxValue)
        {
            ChangeGameState(GamePhase.End);
        }
        
    }

    public void ChangeGameState(GamePhase to)
    {
        currentGamePhase = to;
        OnGamePhaseChange?.Invoke(to);
    }


    public void OnClickNextLevel()
    {
        SaveLoad.I.playerProgress.currentLevel++;
        SaveLoad.I.SaveToJson();

        StartLevel();
    }

    public void StartLevel()
    {
        ChangeGameState(GamePhase.Game);
    }

    public void OnClickRestart()
    {
        SaveLoad.I.SaveToJson();
        StartLevel();
    }

}
