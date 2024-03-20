using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioSource audioSource;
    public AudioClip tap;
    public AudioClip powerUpSfx;
    public AudioClip buzzer;

    private void Awake()
    {
        Instance = this;
    }

    public void PlaySfx(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
