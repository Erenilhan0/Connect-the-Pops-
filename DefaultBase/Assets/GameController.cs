using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;


public class GameController : MonoBehaviour
{
    private PointerEventData pointerEventData;
    
    [SerializeField] private List<Pop> connectedPieces;
    [SerializeField] private LineRenderer lineRenderer;
    
    private float selectedInstanceID;

    [SerializeField] private Pop popToMerge;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SelectFirstPop();
        }

        if (Input.GetMouseButton(0))
        {
            SelectNextPop();
        }

        if (Input.GetMouseButtonUp(0))
        {
            DestroyPops();
        }
    }

    private void SelectFirstPop()
    {
        popToMerge.transform.localScale = Vector3.zero;
        selectedInstanceID = 0;
        connectedPieces.Clear();

        
        var eventData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        if (results.Count(r => r.gameObject.layer == 6) <= 0) return;

        var firstPop = results[0].gameObject.GetComponentInParent<Pop>();
        connectedPieces.Add(firstPop);
        selectedInstanceID = results[0].gameObject.GetInstanceID();
        SetLineRenderer();
    }

    private void SelectNextPop()
    {
        var eventData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };
        
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        if (results.Count(r => r.gameObject.layer == 6) <= 0) return;
        
        var instanceID = results[0].gameObject.GetInstanceID();

        if (instanceID == selectedInstanceID) return;
       
        selectedInstanceID = instanceID;

        if (connectedPieces.Count < 1) return;

        var nextPop = results[0].gameObject.GetComponentInParent<Pop>();
        var lastPop = connectedPieces[^1];

        if (!CanConnect(new Vector2(lastPop.column, lastPop.row),
                new Vector2(nextPop.column, nextPop.row))) return;

        if (nextPop.popLevel != connectedPieces[0].popLevel) return;

        if (connectedPieces.Contains(nextPop)) return;

        connectedPieces.Add(nextPop);
        SetUIPop();
        SetLinePosition();
    }


    private static bool CanConnect(Vector2 lastConnectedPop, Vector2 targetPop)
    {
        var difX = lastConnectedPop.x - targetPop.x;
        var difY = lastConnectedPop.y - targetPop.y;

        return MathF.Abs(difX) <= 1 && MathF.Abs(difY) <= 1;
    }

    private void SetLineRenderer()
    {
        lineRenderer.startColor = connectedPieces[0].popColors[connectedPieces[0].popLevel];
        lineRenderer.endColor = connectedPieces[0].popColors[connectedPieces[0].popLevel];
    }


    private void SetLinePosition()
    {
        lineRenderer.positionCount = connectedPieces.Count;
        var positions = new Vector3[connectedPieces.Count];

        for (int i = 0; i < positions.Length; i++)
        {
            positions[i] = connectedPieces[i].transform.localPosition;
        }

        lineRenderer.SetPositions(positions);
    }

    public void SetUIPop()
    {
        if (popToMerge.transform.localScale == Vector3.zero)
        {
            popToMerge.transform.DOScale(Vector3.one, .25f);
        }
        popToMerge.UIPop(GetScoreMultiplier(connectedPieces.Count)+connectedPieces[0].popLevel);
    }

    private void DestroyPops()
    {
        if(connectedPieces.Count <= 1) return;
        
        popToMerge.transform.DOScale(Vector3.zero, .25f);

        connectedPieces[^1].UpgradeThePiece(GetScoreMultiplier(connectedPieces.Count));

        for (int i = 0; i < connectedPieces.Count - 1; i++)
        {
            connectedPieces[i].DestroyThis();
        }

        connectedPieces.Clear();
        SetLinePosition();
        StartCoroutine(BoardManager.Instance.DecreaseRow());
    }


    private static int GetScoreMultiplier(int connectedPopCount)
    {
        if (connectedPopCount < 4)
        {
            return 1;
        }
        if (connectedPopCount < 6)
        {
            return 2;
        }
        if (connectedPopCount < 8)
        {
            return 3;
        }
        if (connectedPopCount < 10)
        {
            return 4;
        }

        if (connectedPopCount <12)
        {
            return 5;
        }

        if (connectedPopCount < 14)
        {
            return 6;
        }
        return 1;
    }
}