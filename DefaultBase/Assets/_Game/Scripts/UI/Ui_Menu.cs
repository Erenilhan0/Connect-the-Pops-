using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Ui_Menu : UiBase
{
    public override void HideUi()
    {
        transform.DOScale(0, 0);
    }


    public override void ShowUi()
    {
        transform.DOScale(1, .4f);
    }
    
    
    
    
}
