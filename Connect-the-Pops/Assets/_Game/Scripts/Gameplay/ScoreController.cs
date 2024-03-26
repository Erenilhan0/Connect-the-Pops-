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
    
    
    public static int UpdateScore(int updateAmount)
    {
        Score += updateAmount;
        SaveLoad.I.SaveToJson();
        return Score;
    }
    
    
    
}
