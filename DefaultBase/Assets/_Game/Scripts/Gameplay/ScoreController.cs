using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public static class ScoreController 
{
    private static int Score
    {
        get => SaveLoad.I.PlayerProgress.Score;
        set => SaveLoad.I.PlayerProgress.Score = value;
    }
    
    
    public static void UpdateScore(int updateAmount,int connectedPopCount)
    {
        Score += updateAmount;
        UiManager.Instance.UpdateScore(Score,connectedPopCount);
        SaveLoad.I.SaveToJson();
    }
    
    
    
}
