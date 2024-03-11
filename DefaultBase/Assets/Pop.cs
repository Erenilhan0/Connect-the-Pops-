using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Pop : MonoBehaviour
{
    public Image popBg;
    public TextMeshProUGUI popValueText;
    
    public Color[] popColors;
    public int popLevel;
    
    public int column;
    public int row;
    
    public void Init(int _column, int _row)
    {
        popLevel = Random.Range(1, BoardManager.Instance.boardMaxLevel);
        column = _column;
        row = _row;
        
        transform.localScale = Vector3.zero;
        transform.localPosition = BoardManager.Instance.GetPositionFromCoordinate(column, row);
        
        // Delete
        name = "( " + column + ", " + row + " )";
        
        SetPopData();
        SpawnAnimation();
    }
    
    private void SetPopData()
    {
        popBg.color = popColors[popLevel];
        
        var popValue = MathF.Pow(2, popLevel);
        popValueText.text = popValue.ToString(CultureInfo.InvariantCulture);
    }
    
    private void SpawnAnimation()
    {
        transform.DOScale(Vector3.one, 0.2f);
    }
    
    public void UpgradeThePiece(int levelMultiplier)
    {
        popLevel += levelMultiplier;
        transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), .15f).SetLoops(2, LoopType.Yoyo);
        UpdateScore(levelMultiplier);
        SetPopData();
    }


    private void UpdateScore(int scoreMultiplier)
    {
        var popValue = MathF.Pow(2, popLevel);
        var scoreIncreaseAmount = popValue;
        
        GameManager.I.UpdateScore((int)scoreIncreaseAmount);
    }

    public void DropThePiece(int dropCount)
    {
        row -= dropCount;
        var targetRow = BoardManager.Instance.GetPositionFromCoordinate(column, row).y;
        var dropTime = (float)dropCount/3;
        transform.DOLocalMoveY(targetRow, dropTime);
        
        // Delete
        name = "( " + column + ", " + row + " )";

       
    }
    public void DestroyThis()
    {
        BoardManager.Instance.RemovePopFromList(column,row);
        Destroy(gameObject);
    }


    public void UIPop(int level)
    {
        popBg.color = popColors[level];
        
        var popValue = MathF.Pow(2, level);
        popValueText.text = popValue.ToString(CultureInfo.InvariantCulture);
    }
}