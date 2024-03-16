using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance { get; private set; }

    // Board Values
    public int BoardMaxLevel;
    public int BoardWidth;
    public int BoardHeight;

    private float _offset = 60;

    private Vector3 _startPos = new(-120, -140, 0);

    private bool _availableMove;


    // Pop Values
    [SerializeField] private Pop popPrefab;

    private Pop[,] allPops;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GameManager.Instance.OnGamePhaseChange += OnGamePhaseChange;

        allPops = new Pop[BoardWidth, BoardHeight];

        if (SaveLoad.I.PlayerProgress.Score > 0)
        {
            LoadTheBoard();
        }
        else
        {
            SpawnTheBoard();
        }

        FindAvailableMoves();
    }

    private void OnGamePhaseChange(GamePhase obj)
    {
        if (obj == GamePhase.End)
        {
            SaveTheBoard();
        }
    }


    private void SaveTheBoard()
    {
        var popList = new List<Pop>();

        for (int i = 0; i < BoardWidth; i++)
        {
            for (int j = 0; j < BoardHeight; j++)
            {
                popList.Add(allPops[i, j]);
            }
        }

        for (int z = 0; z < popList.Count; z++)
        {
            SaveLoad.I.PlayerProgress.PopDatas[z] = popList[z].PopData;
        }

        SaveLoad.I.SaveToJson();
    }

    private void SpawnTheBoard()
    {
        for (int i = 0; i < BoardWidth; i++)
        {
            for (int j = 0; j < BoardHeight; j++)
            {
                PopSpawn(i, j, 0, true, false);
            }
        }

        SaveTheBoard();
    }

    private void LoadTheBoard()
    {
        for (int i = 0; i < BoardWidth * BoardHeight; i++)
        {
            var popData = SaveLoad.I.PlayerProgress.PopDatas[i];

            PopSpawn(popData.Column, popData.Row, popData.PopLevel, false, false);
        }
    }

    private void PopSpawn(int column, int row, int popLevel, bool randomSpawn, bool smartRefill)
    {
        var pop = Instantiate(popPrefab, transform);

        allPops[column, row] = pop;

        pop.Init(column, row, popLevel, randomSpawn, smartRefill);
    }

    public Pop GetSameLevelPop(int column, int row)
    {
        if (column - 1 > 0 && allPops[column - 1, row] != null && allPops[column - 1, row].PopData.PopLevel < 6)
        {
            return allPops[column - 1, row];
        }

        if (column + 1 < 5 && allPops[column + 1, row] != null && allPops[column + 1, row].PopData.PopLevel < 6)
        {
            return allPops[column + 1, row];
        }

        if (allPops[column, row - 1] != null && allPops[column, row - 1].PopData.PopLevel < 6)
        {
            return allPops[column, row - 1];
        }

        if (column + 1 < 5 && row - 1 > 0 && allPops[column + 1, row - 1] != null &&
            allPops[column + 1, row - 1].PopData.PopLevel < 6)
        {
            return allPops[column + 1, row - 1];
        }

        if (column - 1 > 0 && row - 1 > 0 && allPops[column - 1, row - 1] != null &&
            allPops[column - 1, row - 1].PopData.PopLevel < 6)
        {
            return allPops[column - 1, row - 1];
        }

        return allPops[0, 0];
    }

    public Vector3 GetPositionFromCoordinate(int column, int row)
    {
        return _startPos + new Vector3(column * _offset, row * _offset, 0);
    }

    public void RemovePopFromList(int column, int row)
    {
        allPops[column, row] = null;
    }

    public IEnumerator DecreaseRow()
    {
        yield return new WaitForSeconds(.2f);

        var emptyCellCount = 0;

        for (int i = 0; i < BoardWidth; i++)
        {
            for (int j = 0; j < BoardHeight; j++)
            {
                if (allPops[i, j] == null)
                {
                    emptyCellCount++;
                }
                else if (emptyCellCount > 0)
                {
                    var pop = allPops[i, j];

                    RemovePopFromList(pop.PopData.Column, pop.PopData.Row);

                    pop.DropThePop(emptyCellCount);

                    allPops[pop.PopData.Column, pop.PopData.Row] = pop;
                }
            }

            emptyCellCount = 0;
        }

        RefillTheBoard();
    }


    private void RefillTheBoard()
    {
        FindAvailableMoves();

        var smartRefillCount = 0;

        for (int i = 0; i < BoardWidth; i++)
        {
            for (int j = 0; j < BoardHeight; j++)
            {
                if (allPops[i, j] != null) continue;

                if (j == 4 && smartRefillCount == 0 && !_availableMove)
                {
                    smartRefillCount++;

                    PopSpawn(i, j, 0, true, true);
                }
                else
                {
                    PopSpawn(i, j, 0, true, false);
                }
            }
        }
    }

    private void FindAvailableMoves()
    {
        for (int i = 0; i < BoardWidth; i++)
        {
            for (int j = 0; j < BoardHeight; j++)
            {
                if (allPops[i, j] == null) continue;
                
                if (AvailablePop(allPops[i, j]))
                {
                    return;
                }
            }
        }
    }

    private bool AvailablePop(Pop pop)
    {
        var column = pop.PopData.Column;

        var row = pop.PopData.Row;

        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                if (column - i < 0 || row - j < 0 || column + i > 4 || row + j > 4) continue;

                Pop popInRange = null;

                if (allPops[column - i, row - j] != null)
                {
                    popInRange = allPops[column - i, row - j];
                }
                else if (allPops[column + i, row + j])
                {
                    popInRange = allPops[column + i, row + j];
                }
                
                if (popInRange == null) continue;
                
                if (popInRange == pop || popInRange.PopData.PopLevel != pop.PopData.PopLevel) continue;

                _availableMove = true;

                return _availableMove;
            }
        }

        _availableMove = false;

        return _availableMove;
    }

    private void OnApplicationQuit()
    {
        SaveTheBoard();
    }
}