
using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Ui_EndGame : UiBase
{
    
    private bool isHidden;





    
    
    public override void HideUi()
    {
        if (isHidden) return;
        isHidden = true;
        UiManager.I.OpenCloseBgFade(false);
        transform.DOScale(0, 0);
    }


    public override void ShowUi()
    {
        isHidden = false;

        transform.DOScale(1, 0);



    }
    
    
    
    
    
}
