using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip gunshotSound;
    public AudioClip groundImpactSound;
    public AudioClip enemyImpactSound;

    private AudioSource audioSource;

    public static AudioManager instance;

    void Start() {
        audioSource = GetComponent<AudioSource>();

        if (instance == null) instance = this;
    }

    public void PlayOneShot(AudioClip audioClip) {
        audioSource.PlayOneShot(audioClip);
    }
}
