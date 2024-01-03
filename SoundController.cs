using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource audioSource2;
    [SerializeField] private AudioSource audioSource3;
    [SerializeField] private AudioSource bgAudioSource;

    [SerializeField] private AudioClip punchClip;
    [SerializeField] private AudioClip deadClip;
    [SerializeField] private AudioClip birdClip;
    [SerializeField] private AudioClip walkingClip;
    [SerializeField] private AudioClip gaspClip;
    [SerializeField] private AudioClip hurtClip;
    [SerializeField] private AudioClip grapplingClip;
    [SerializeField] private AudioClip successClip;

    public void PunchSound()
    {
        audioSource2.clip = punchClip;
        audioSource2.Play();
    }

    public void DeadSound()
    {
        audioSource.clip = deadClip;
        audioSource.Play();
    }
    
    public void WalkingSound()
    {
        audioSource.clip = walkingClip;
        audioSource.Play();
    }

    public void GaspSound()
    {
        audioSource2.clip = gaspClip;
        audioSource2.Play();
    }

    public void HurtSound()
    {
        audioSource.clip = hurtClip;
        audioSource.Play();
    }

    public void GrapplingSound()
    {
        audioSource3.clip = grapplingClip;
        audioSource3.Play();
    }

    public void SuccessSound()
    {
        audioSource3.clip = successClip;
        audioSource3.Play();
    }

    public void Start()
    {
        bgAudioSource.clip = birdClip;
        bgAudioSource.loop = true;
        bgAudioSource.Play();
    }

}
