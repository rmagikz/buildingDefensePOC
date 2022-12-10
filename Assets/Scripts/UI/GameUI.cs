using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class GameUI : MonoBehaviour
{
    [SerializeField] private Button helicopterButton, otherButton;

    public static event Action HelicopterStarted;

    void Start() {
        EnemyManager.WaveEnded += Toggle;
        helicopterButton.onClick.AddListener(StartHelicopter);
        Toggle();
    }

    public void Toggle() {
        if (helicopterButton.gameObject.activeInHierarchy) {
            helicopterButton.transform.DOScale(Vector3.zero, 0.5f).OnComplete(() => helicopterButton.gameObject.SetActive(false));
            otherButton.transform.DOScale(Vector3.zero, 0.5f).OnComplete(() => otherButton.gameObject.SetActive(false));
        }
        else {
            helicopterButton.gameObject.SetActive(true);
            otherButton.gameObject.SetActive(true);
            helicopterButton.transform.DOScale(Vector3.one, 0.5f);
            otherButton.transform.DOScale(Vector3.one, 0.5f);
        }
    }

    private void StartHelicopter() {
        Toggle();
        HelicopterStarted?.Invoke();
    }
}

