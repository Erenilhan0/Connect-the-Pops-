using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    
    public static UiManager I;


    [SerializeField] private UiBase[] Uis;

    [SerializeField] private Image bg;
    
    
    
    private void Awake()
    {
        I = this;
    }

    private void OnOnGamePhaseChange(GamePhase obj)
    {
        for (int i = 0; i < Uis.Length; i++)
        {
            if ((int) obj == i)
            {
                Uis[i].ShowUi();
            }
            else
            {
                Uis[i].HideUi();
            }
        }
        
        switch (obj)
        {
            case GamePhase.Menu:
                break;
            case GamePhase.Game:
                break;
            case GamePhase.End:
                break;
        }
    }


    
    

    private void Start()
    {
        GameManager.Instance.OnGamePhaseChange += OnOnGamePhaseChange;

    }



    public void OnClickNextLevel()
    {
        GameManager.Instance.OnClickNextLevel();
    }

    public void OnClickRestart()
    {
        GameManager.Instance.OnClickRestart();
    }
    

    public void OnClickPlayButton()
    {
        GameManager.Instance.StartLevel();

    }


    public void OpenCloseBgFade(bool open)
    {
        if (open)
        {
            bg.DOFade(.8f, .5f);
        }
        else
        {
            bg.DOFade(0, .5f);
        }
    }
 
    
    

}
