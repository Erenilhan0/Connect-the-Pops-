using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance { get; private set; }

    public int boardMaxLevel;
    public int boardWidth;
    public int boardHeight;
    public float offset;

    public Pop popPrefab;

    public Vector3 startPos;

    private Pop[,] allPops;

  
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        allPops = new Pop[boardWidth, boardHeight];
        
        SetUpTheBoard();
    }

    private void SetUpTheBoard()
    {
        for (int i = 0; i < boardWidth; i++)
        {
            for (int j = 0; j < boardHeight; j++)
            {
                PopSpawn(i, j);
            }
        }
    }

    private void PopSpawn(int column, int row)
    {
        var pop = Instantiate(popPrefab, transform);
        allPops[column, row] = pop;
        pop.Init(column, row);
    }

    public Vector3 GetPositionFromCoordinate(int column, int row)
    {
        return startPos + new Vector3(column * offset, row * offset, 0);
    }
    
    public void RemovePopFromList(int column, int row)
    {
        allPops[column, row] = null;
    }

    public IEnumerator DecreaseRow()
    {
        var emptyCellCount = 0;

        for (int i = 0; i < boardWidth; i++)
        {
            for (int j = 0; j < boardHeight; j++)
            {
                if (allPops[i, j] == null)
                {
                    emptyCellCount++;
                }
                else if (emptyCellCount > 0)
                {
                    var pop = allPops[i, j];
                    RemovePopFromList(pop.column,pop.row);
                    pop.DropThePiece(emptyCellCount);
                    allPops[pop.column,  pop.row] = pop;
                }
            }
            emptyCellCount = 0;
        }

        yield return new WaitForSeconds(.35f);
        RefillTheBoard();
    }

    private void RefillTheBoard()
    {
        for (int i = 0; i < boardWidth; i++)
        {
            for (int j = 0; j < boardHeight; j++)
            {
                if (allPops[i, j] == null)
                {
                   PopSpawn(i, j);
                }
            }
        }
    }
}