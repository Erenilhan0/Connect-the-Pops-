﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class PlayerProgress
{
    public int Score;
    
    public int CurrentLevel;
    
    public PopData [] PopDatas = new PopData[25];
    
}
