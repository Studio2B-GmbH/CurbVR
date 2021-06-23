using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to relay an audioClip to a common audiosource shared between many objects
/// </summary>


public class PlayNarrative : MonoBehaviour
{
    [SerializeField]
    AudioSource audioSource;

    [SerializeField]
    AudioClip soundClip;

    [SerializeField]
    int startSecond;

    public void PlayAudio()
    {
        audioSource.clip = soundClip;

        audioSource.time = startSecond;
        

        audioSource.Play();                
    }

    public void PlayAudio(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    public void StopAudio()
    {
        audioSource.Stop();
    }
}
