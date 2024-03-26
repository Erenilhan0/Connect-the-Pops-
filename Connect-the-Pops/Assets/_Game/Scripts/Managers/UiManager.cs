using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public static UiManager Instance;
    [SerializeField] private UiBase[] uis;

    public event Action <int,int> OnScoreUpdate ;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GameManager.Instance.OnGamePhaseChange += OnOnGamePhaseChange;
    }
    
    private void OnOnGamePhaseChange(GamePhase obj)
    {
        for (int i = 0; i < uis.Length; i++)
        {
            if ((int) obj == i)
            {
                uis[i].ShowUi();
            }
            else
            {
                uis[i].HideUi();
            }
        }
  
    }
    public void OnClickNextLevel()
    {
        GameManager.Instance.OnClickNextLevel();
    }
    
    public void OnClickPlayButton()
    {
        GameManager.Instance.StartLevel();
    }

    public void UpdateScore(int scoreUpdateAmount, int connectedPop)
    {
        OnScoreUpdate?.Invoke(scoreUpdateAmount,connectedPop);
    }
    

}
