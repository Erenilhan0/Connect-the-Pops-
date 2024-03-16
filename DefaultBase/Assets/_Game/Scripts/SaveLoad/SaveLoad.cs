using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Serialization;

public class SaveLoad : MonoBehaviour
{
    public static SaveLoad I;

    public PlayerProgress PlayerProgress;

    public string PlayerSaveName = "PlayerSave";


    private void Awake()
    {
        I = this;
        LoadFromJson();
    }


    #region SAVE/LOAD

    public void SaveToJson()
    {
        string content = JsonUtility.ToJson(PlayerProgress, false);

        PlayerPrefs.SetString(PlayerSaveName, content);

        PlayerPrefs.Save();
    }

    private void LoadFromJson()
    {
        if (PlayerPrefs.HasKey(PlayerSaveName))
        {
            string content = PlayerPrefs.GetString(PlayerSaveName);

            PlayerProgress = JsonUtility.FromJson<PlayerProgress>(content);
        }
        else
        {
            SaveToJson();
        }
    }

    #endregion SAVE/LOAD
}