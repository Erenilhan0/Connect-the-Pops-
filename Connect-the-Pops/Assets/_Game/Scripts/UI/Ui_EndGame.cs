using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Ui_EndGame : UiBase
{

    [SerializeField] private TextMeshProUGUI levelEndText;
    
    [SerializeField] private string[] levelEndStrings;
    
    
    public override void HideUi()
    {
        transform.localScale = Vector3.zero;
    }
    
    public override void ShowUi()
    {
        var randomText = levelEndStrings[Random.Range(0, levelEndStrings.Length)];
        
        levelEndText.text = randomText;
        
        transform.localScale = Vector3.one;
    }
    
    
    
    
    
}
