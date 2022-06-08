using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;     
    public static AudioManager Instance { get => _instance; private set => _instance = value; }

    AudioSource _audioSource;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayOneShot(AudioClip clip, float volume)
    {
        _audioSource.PlayOneShot(clip, volume);
    }
}
