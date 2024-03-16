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
    public PopData PopData;

    [SerializeField] private Image popBg;

    [SerializeField] private GameObject whiteCircleGo;

    [SerializeField] private TextMeshProUGUI popValueText;

    [SerializeField] private Color[] popColors;


    public void Init(int column, int row, int popLevel, bool randomSpawn, bool smartRefill)
    {
        if (randomSpawn)
        {
            if (row == 4 && smartRefill)
            {
                var sameLevelPop = BoardManager.Instance.GetSameLevelPop(column, row);
                
                if (sameLevelPop.PopData.Column == 0  && sameLevelPop.PopData.Row == 0)
                {
                    PopData.PopLevel = Random.Range(1, BoardManager.Instance.BoardMaxLevel);
                }
                else
                {
                    PopData.PopLevel = BoardManager.Instance.GetSameLevelPop(column, row).PopData.PopLevel;
                }
            }
            else
            {
                PopData.PopLevel = Random.Range(1, BoardManager.Instance.BoardMaxLevel);
            }
        }
        else
        {
            PopData.PopLevel = popLevel;
        }

        PopData.Column = column;

        PopData.Row = row;

        transform.localScale = Vector3.zero;

        transform.localPosition = BoardManager.Instance.GetPositionFromCoordinate(PopData.Column, PopData.Row);

        SetPopData();

        SpawnAnimation();
    }

    private void SetPopData()
    {
        popBg.color = popColors[PopData.PopLevel - 1];

        var popValue = MathF.Pow(2, PopData.PopLevel);

        if (popValue >= 1000)
        {
            popValueText.text = Mathf.FloorToInt(popValue / 1000) + "K";
        }
        else
        {
            popValueText.text = popValue.ToString(CultureInfo.InvariantCulture);
        }

        if (PopData.PopLevel > 9)
        {
            whiteCircleGo.SetActive(true);
        }
    }

    private void SpawnAnimation()
    {
        transform.DOScale(Vector3.one, 0.2f).SetDelay(.15f);
    }

    public IEnumerator UpgradeThePop(int levelMultiplier, int connectedPopCount)
    {
        transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), .15f).SetLoops(2, LoopType.Yoyo);

        transform.SetAsLastSibling();

        yield return new WaitForSeconds(.2f);

        PopData.PopLevel += levelMultiplier;

        SoundManager.Instance.PlayMergeSound();
        
        StartCoroutine(UpdateScore(connectedPopCount));

        SetPopData();
    }


    public Color GetPopColor()
    {
        return popColors[PopData.PopLevel - 1];
    }

    private IEnumerator UpdateScore(int connectedPopCount)
    {
        yield return new WaitForSeconds(.1f);

        var scoreIncreaseAmount = MathF.Pow(2, PopData.PopLevel);

        ScoreController.UpdateScore((int)scoreIncreaseAmount, connectedPopCount);
    }

    public void DropThePop(int dropCount)
    {
        PopData.Row -= dropCount;

        var targetRow = BoardManager.Instance.GetPositionFromCoordinate(PopData.Column, PopData.Row).y;

        var dropTime = (float)dropCount / 10;

        transform.DOLocalMoveY(targetRow, dropTime);
    }

    public void DestroyThePop(Vector3 mergePosition)
    {
        BoardManager.Instance.RemovePopFromList(PopData.Column, PopData.Row);

        transform.DOLocalMove(mergePosition, .18f).OnComplete((() => Destroy(gameObject)));
    }


    public void UIPop(int level)
    {
        popBg.color = popColors[level - 1];

        var popValue = MathF.Pow(2, level);

        if (popValue >= 1000)
        {
            popValueText.text = Mathf.FloorToInt(popValue / 1000) + "K";
        }
        else
        {
            popValueText.text = popValue.ToString(CultureInfo.InvariantCulture);
        }

        whiteCircleGo.SetActive(level > 9);
    }
}

[Serializable]
public class PopData
{
    public int Column;

    public int Row;

    public int PopLevel;
}