using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public static SoundManager Instance;
    [SerializeField] private AudioSource audioSource;
    
    private void Awake()
    {
        Instance = this;
    }

   
    public void PlayPopSound(float connectedCount)
    {
        audioSource.Play();

        audioSource.pitch = 1 + connectedCount/5;
    }
}
