using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ClipType {SoundEffect, SoundClip};
public enum ClipName {MinigunSpin, RifleShot, HelicopterRotor, MinigunBurst, BulletImpactFlesh, BulletImpactGround};

[System.Serializable]
public class Sound
{
    public ClipName name;
    public ClipType type;
    public AudioClip clip;
    [Range (0.0f,1.0f)]
    public float volume = 1.0f;
    public bool loop;
    [HideInInspector]
    public AudioSource source;
}
