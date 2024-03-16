using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance{ get; private set; }

    [SerializeField] private AudioSource [] audioSources;

    [SerializeField] private AudioClip[] sounds;

    private void Awake()
    {
        Instance = this;
    }
    

    private void Start()
    {
        GameManager.Instance.OnGamePhaseChange += OnGamePhaseChange;
    }

    private void OnGamePhaseChange(GamePhase obj)
    {
        if (obj == GamePhase.End)
        {
            PlayLevelEndSound();
        }
    }

    public void PlayPopSound(float connectedCount)
    {
        var audioSource = GetAudioSourceFromPool();
        audioSource.clip = sounds[0];
        audioSource.pitch = 1 + (connectedCount - 2) / 8;
        audioSource.Play();

    }


    private AudioSource GetAudioSourceFromPool()
    {
        foreach (var audioSource in audioSources)
        {
            if (!audioSource.isPlaying)
            {
                return audioSource;
            }
        }
        return audioSources[0];
    }

    public void PlayMergeSound()
    {
        var audioSource = GetAudioSourceFromPool();
        audioSource.pitch = 1;
        audioSource.clip = sounds[1];
        audioSource.Play();
    }

    private void PlayLevelEndSound()
    {
        var audioSource = GetAudioSourceFromPool();
        audioSource.pitch = 1;
        audioSource.clip = sounds[2];
        audioSource.Play();
    }
}