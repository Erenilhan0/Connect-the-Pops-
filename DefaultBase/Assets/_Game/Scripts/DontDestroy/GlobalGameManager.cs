using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalGameManager : MonoBehaviour
{

    public static GlobalGameManager I;
    public int currentLevel =>SaveLoad.I.playerProgress.currentLevel;

    private void Awake()
    {
        I = this;
    }

    private void Start()
    {

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            SceneManager.LoadScene(1);

        }
    }
}
