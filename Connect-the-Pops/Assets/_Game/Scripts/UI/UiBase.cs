using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class UiBase : MonoBehaviour
{
    public virtual void ShowUi()
    { }

    public virtual void HideUi()
    { }

    public virtual void UpdateScoreUI(int score, int connectedCount)
    { }


}
