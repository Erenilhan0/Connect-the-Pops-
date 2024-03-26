using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public enum GamePhase
{
    Menu,
    Game,
    End
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    public event Action<GamePhase> OnGamePhaseChange;
    
    public GamePhase CurrentGamePhase;
    
    private void Awake()
    {
        Instance = this;
    }
    
    public void ChangeGameState(GamePhase to)
    {
        CurrentGamePhase = to;
        
        OnGamePhaseChange?.Invoke(to);
    }
    
    public void OnClickNextLevel()
    {
        SaveLoad.I.PlayerProgress.CurrentLevel++;
        
        SaveLoad.I.SaveToJson();

        StartLevel();
    }

    public void StartLevel()
    {
        ChangeGameState(GamePhase.Game);
    }

    public void PauseTheGame()
    {
        ChangeGameState(GamePhase.Menu);
    }
    
}