using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MainUI : MonoBehaviour
{
    [SerializeField] private WorldSpaceUIManager worldSpaceUIManager;
    [SerializeField] private Button waveButton;

    void Start() {
        waveButton.onClick.AddListener(StartWave);

        Toggle();
    }

    void Update() {

    }

    public void Toggle() {
        if (waveButton.gameObject.activeInHierarchy) {
            waveButton.transform.DOScale(Vector3.zero, 0.5f).OnComplete(() => waveButton.gameObject.SetActive(false));
        }
        else {
            waveButton.gameObject.SetActive(true);
            waveButton.transform.DOScale(Vector3.one, 0.5f);
        }
    }

    private void StartWave() {
        Toggle();
        worldSpaceUIManager.ToggleAll();
        PlayerManager.waveInProgress = true;
    }
}

