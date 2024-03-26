using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;


public class GameController : MonoBehaviour
{
    private PointerEventData _pointerEventData;

    private List <Pop> _connectedPops = new();

    private float _selectedInstanceID;

    [SerializeField] private LineRenderer lineRenderer;

    [SerializeField] private Pop uiPop;

    private void Update()
    {
        if (GameManager.Instance.CurrentGamePhase != GamePhase.Game) return;
        
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
        
        _selectedInstanceID = 0;
        
        _connectedPops.Clear();
        
        var eventData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };
        
        var results = new List<RaycastResult>();
        
        EventSystem.current.RaycastAll(eventData, results);

        if (results.Count(r => r.gameObject.layer == 6) <= 0) return;

        var firstPop = results[0].gameObject.GetComponentInParent<Pop>();
        
        _connectedPops.Add(firstPop);
        
        _selectedInstanceID = results[0].gameObject.GetInstanceID();
        
        SoundManager.Instance.PlayPopSound(_connectedPops.Count);

        SetLineData();
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

        if (instanceID == _selectedInstanceID) return;

        _selectedInstanceID = instanceID;
        
        if (_connectedPops.Count < 1) return;

        var selectedPop = results[0].gameObject.GetComponentInParent<Pop>();
        
        var lastPop = _connectedPops[^1];

        if (!CanConnect(new Vector2(lastPop.PopData.Column, lastPop.PopData.Row),
                new Vector2(selectedPop.PopData.Column, selectedPop.PopData.Row))) return;

        if (selectedPop.PopData.PopLevel != _connectedPops[0].PopData.PopLevel) return;

        if (!_connectedPops.Contains(selectedPop))
        {
            _connectedPops.Add(selectedPop);
            
            SoundManager.Instance.PlayPopSound(_connectedPops.Count);
        }
        else if (_connectedPops.Count >= 2 &&_connectedPops[^2] == selectedPop)
        {
            _connectedPops.Remove(lastPop);
            
            SoundManager.Instance.PlayPopSound(_connectedPops.Count);
        }
        
        SetUIPop();
        
        SetLinePositions();
    }


    private static bool CanConnect(Vector2 lastConnectedPop, Vector2 targetPop)
    {
        var difX = lastConnectedPop.x - targetPop.x;
        
        var difY = lastConnectedPop.y - targetPop.y;

        return MathF.Abs(difX) <= 1 && MathF.Abs(difY) <= 1;
    }

    private void SetLineData()
    {
        var popColor = _connectedPops[0].GetPopColor();
        
        lineRenderer.startColor = popColor;
        
        lineRenderer.endColor = lineRenderer.startColor;
    }


    private void SetLinePositions()
    {
        lineRenderer.positionCount = _connectedPops.Count;
        
        var positions = new Vector3[_connectedPops.Count];

        for (int i = 0; i < positions.Length; i++)
        {
            positions[i] = _connectedPops[i].transform.localPosition;
        }

        lineRenderer.SetPositions(positions);
    }

    private void SetUIPop()
    {
        if (uiPop.transform.localScale == Vector3.zero)
        {
            uiPop.transform.DOScale(Vector3.one, .25f);
        }

        if (_connectedPops.Count == 1)
        {
            uiPop.transform.DOScale(Vector3.zero, .25f);
        }

        uiPop.UIPop(ScoreMultiplier(_connectedPops.Count) + _connectedPops[0].PopData.PopLevel);
    }

    private void DestroyPops()
    {
        uiPop.transform.DOScale(Vector3.zero, .25f);

        if (_connectedPops.Count <= 1) return;

        StartCoroutine(_connectedPops[^1].UpgradeThePop(ScoreMultiplier(_connectedPops.Count),
            _connectedPops.Count));

        var mergePosition = 
            BoardManager.Instance.GetPositionFromCoordinate(_connectedPops[^1].PopData.Column, 
                _connectedPops[^1].PopData.Row);
        
        for (int i = 0; i < _connectedPops.Count - 1; i++)
        {
            _connectedPops[i].DestroyThePop(mergePosition);
        }

        _connectedPops.Clear();

        SetLinePositions();
        
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