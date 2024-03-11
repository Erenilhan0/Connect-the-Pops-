using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum  GamePhase{Menu,Game,End}

public class GameManager : MonoBehaviour
{
    public static GameManager I;
    public event Action<GamePhase> OnGamePhaseChange;
    public GamePhase currentGamePhase;
    
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private float score;


    [SerializeField] private TextMeshProUGUI currentLevelText;
    [SerializeField] private TextMeshProUGUI nextLevelText;


    public float nextLevelScore;
    private void Awake()
    {
        I = this;
    }

    private void Start()
    {
        SetLevelData();
    }


    public void SetLevelData()
    {
        currentLevelText.text = SaveLoad.I.playerProgress.currentLevel.ToString();
        nextLevelText.text = (SaveLoad.I.playerProgress.currentLevel+1).ToString();
    }


    public void UpdateScore(int updateAmount)
    {
        score += updateAmount;
        scoreText.text = score.ToString();
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
