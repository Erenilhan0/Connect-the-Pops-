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
        GameManager.I.OnGamePhaseChange += OnOnGamePhaseChange;

    }




    



    private void Update()
    {
 

        if (GameManager.I.currentGamePhase == GamePhase.Menu &&
            Input.GetMouseButtonDown(0))
        {
            OnClickPlayButton();
        }

    }






    






    public void OnClickNextLevel()
    {
        GameManager.I.OnClickNextLevel();
    }

    public void OnClickRestart()
    {
        GameManager.I.OnClickRestart();
    }
    

    public void OnClickPlayButton()
    {
        GameManager.I.StartLevel();

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
