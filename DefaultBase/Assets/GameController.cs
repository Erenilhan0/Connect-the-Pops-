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

    private List<Pop> connectedPieces = new List<Pop>();
    [SerializeField] private LineRenderer lineRenderer;

    private float selectedInstanceID;

    [SerializeField] private Pop uiPop;

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
        uiPop.transform.localScale = Vector3.zero;
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

        var selectedPop = results[0].gameObject.GetComponentInParent<Pop>();
        var lastPop = connectedPieces[^1];

        if (!CanConnect(new Vector2(lastPop.column, lastPop.row),
                new Vector2(selectedPop.column, selectedPop.row))) return;

        if (selectedPop.popLevel != connectedPieces[0].popLevel) return;

        if (!connectedPieces.Contains(selectedPop))
        {
            connectedPieces.Add(selectedPop);
        }
        else if (connectedPieces[^2] == selectedPop)
        {
            connectedPieces.Remove(lastPop);
        }
        
        SoundManager.Instance.PlayPopSound(connectedPieces.Count);

        
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
        var popColor = connectedPieces[0].GetPopColor();
        lineRenderer.startColor = popColor;
        lineRenderer.endColor = lineRenderer.startColor;
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

    private void SetUIPop()
    {
        if (uiPop.transform.localScale == Vector3.zero)
        {
            uiPop.transform.DOScale(Vector3.one, .25f);
        }

        if (connectedPieces.Count == 1)
        {
            uiPop.transform.DOScale(Vector3.zero, .25f);
        }

        uiPop.UIPop(ScoreMultiplier(connectedPieces.Count) + connectedPieces[0].popLevel);
    }

    private void DestroyPops()
    {
        uiPop.transform.DOScale(Vector3.zero, .25f);

        if (connectedPieces.Count <= 1) return;

        connectedPieces[^1].UpgradeThePiece(ScoreMultiplier(connectedPieces.Count));

        for (int i = 0; i < connectedPieces.Count - 1; i++)
        {
            connectedPieces[i].DestroyThis();
        }

        connectedPieces.Clear();

        SetLinePosition();
        StartCoroutine(BoardManager.Instance.DecreaseRow());
    }


    private static int ScoreMultiplier(int connectedPopCount)
    {
        return connectedPopCount switch
        {
            < 4 => 1,
            < 8 => 2,
            < 16 => 3,
            < 32 => 4,
            _ => 5
        };
    }
}