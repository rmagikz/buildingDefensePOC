using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Helicopter : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    public GameObject minigun, minigunBarrel, minigunStand, mainRotor, rearRotor;
    public Transform targetPos, lookAt;

    void Start() {
        audioSource.volume = 0;
    }

    public void FadeInVolume() {
        audioSource.DOFade(2f, 0.5f);
    }

}
