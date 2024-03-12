using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public static class ScoreController 
{
    private static int Score
    {
        get => SaveLoad.I.playerProgress.score;
        set => SaveLoad.I.playerProgress.score = value;
    }
    
    
    public static void UpdateScore(int updateAmount)
    {
        Score += updateAmount;
        GameManager.Instance.UpdateScoreUI(Score);
        SaveLoad.I.SaveToJson();
    }
    
    
    
}
