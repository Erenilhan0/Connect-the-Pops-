using System;
using System.Collections;
using System.Collections.Generic;
//using GameAnalyticsSDK;
using UnityEngine;

public class GameAnalyticsManager : MonoBehaviour
{
  /*
  public static GameAnalyticsManager I;
    
  public string output = "";
  public string stack = "";

  private void Awake()
  {
    if (I == null)
    {
      I = this;
      GameAnalytics.Initialize();
    }
  }
  
  
  void OnEnable()
  {
    Application.logMessageReceived += HandleLog;
  }

  void OnDisable()
  {
    Application.logMessageReceived -= HandleLog;
  }

  void HandleLog(string logString, string stackTrace, LogType type)
  {

    GAErrorSeverity severity = GAErrorSeverity.Critical;

    switch (type)
    {
      case LogType.Error:
        severity = GAErrorSeverity.Error;
        break;
      case LogType.Assert:
        severity = GAErrorSeverity.Info;
        break;
      case LogType.Warning:
        severity = GAErrorSeverity.Warning;
        break;
      case LogType.Log:
        severity = GAErrorSeverity.Debug;
        break;
      case LogType.Exception:
        severity = GAErrorSeverity.Critical;

        break;
      default:
        throw new ArgumentOutOfRangeException(nameof(type), type, null);
    }
    
    GameAnalytics.NewErrorEvent (severity, logString);
  }
  
  
  
  */
  
}
