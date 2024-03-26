using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Ui_Menu : UiBase
{
    public override void HideUi()
    {
        transform.localScale = Vector3.zero;

    }
    
    public override void ShowUi()
    {
        transform.localScale = Vector3.one;
    }
    
    
}
