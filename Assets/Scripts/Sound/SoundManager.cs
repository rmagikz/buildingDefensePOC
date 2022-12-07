using UnityEngine;
using System;
using DG.Tweening;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public Sound[] sounds;

    private AudioSource effectsAudioSource;

    void Awake() {
        Instance = this; // single scene no need to null check and destroy

        effectsAudioSource = gameObject.AddComponent<AudioSource>();

        for (int i = 0; i < sounds.Length; i++) {
            if (sounds[i].type != ClipType.SoundClip) continue;
            sounds[i].source = gameObject.AddComponent<AudioSource>();
            sounds[i].source.clip = sounds[i].clip;
            sounds[i].source.volume = sounds[i].volume;
            sounds[i].source.loop = sounds[i].loop;
            sounds[i].source.playOnAwake = false;
        }
    }

    public void PlaySound(ClipName name, bool force = false) {
        Sound soundEffect = Array.Find(sounds, sound => sound.name == name);
        if (soundEffect.source.isPlaying == true && force == false) return;
        soundEffect.source.Play();
    }

    public void StopSound(ClipName name) {
        Sound soundEffect = Array.Find(sounds, sound => sound.name == name);
        soundEffect.source.Stop();
    }

    public void LerpPitch(ClipName name, float t, float min, float max) {
        Sound soundEffect = Array.Find(sounds, sound => sound.name == name);
        soundEffect.source.pitch = t * max + (1 - t) * min;
    }

    public void FadeInVolume(ClipName name, float endValue, float duration) {
        Sound soundEffect = Array.Find(sounds, sound => sound.name == name);
        soundEffect.source.DOFade(2f, 0.5f);
    }

    public void PlayEffect(ClipName name) {
        Sound soundEffect = Array.Find(sounds, sound => sound.name == name);
        effectsAudioSource.PlayOneShot(soundEffect.clip);
    }
}
