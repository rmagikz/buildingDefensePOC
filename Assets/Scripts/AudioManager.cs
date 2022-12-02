using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip gunshotSound;
    public AudioClip groundImpactSound;
    public AudioClip enemyImpactSound;
    public AudioClip minigunFire;
    public AudioClip helicopterRotor;

    private AudioSource audioSource;
    private AudioSource oneShotAudioSource;

    public static AudioManager instance;

    private float oneShotDelay = 0;

    void Start() {
        audioSource = GetComponents<AudioSource>()[0];
        oneShotAudioSource = GetComponents<AudioSource>()[1];

        if (instance == null) instance = this;
    }

    public void PlayOneShot(AudioClip audioClip) {
        oneShotAudioSource.PlayOneShot(audioClip);
    }

    public void PlayClip(AudioClip audioClip, float volume = 1.0f, bool forceStrict = true) {
        if (audioSource.isPlaying && forceStrict) return;
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.Play();
    }

    public void StopClip() {
        audioSource.Stop();
    }
}
