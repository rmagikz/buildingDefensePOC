using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UpgradeUIManager : MonoBehaviour
{
    [SerializeField] private WorldSpaceUIManager worldSpaceUIManager;
    [SerializeField] private CameraManager cameraManager;
    [SerializeField] private Button upgrade1, upgrade2, backButton;

    private bool panelEnabled = true;

    void Start() {
        Toggle();

        backButton.onClick.AddListener(BackClicked);
        upgrade1.onClick.AddListener(Upgrade1Clicked);
        upgrade2.onClick.AddListener(Upgrade2Clicked);
    }
    
    public void Toggle() {
        //gameObject.SetActive(true);
        if (panelEnabled) {
            panelEnabled = false;
            backButton.transform.DOScale(new Vector3(0,0,0), 0.5f);
            upgrade1.transform.DOScale(new Vector3(0,0,0), 0.5f);
            upgrade2.transform.DOScale(new Vector3(0,0,0), 0.5f);
        } else {
            panelEnabled = true;
            backButton.transform.DOScale(new Vector3(1,1,1), 0.5f);
            upgrade1.transform.DOScale(new Vector3(1,1,1), 0.5f);
            upgrade2.transform.DOScale(new Vector3(1,1,1), 0.5f);
        }
    }

    private void BackClicked() {
        Toggle();
        worldSpaceUIManager.ToggleAll();
        cameraManager.LookAtBuilding();
    }

    private void Upgrade1Clicked() {

    }

    private void Upgrade2Clicked() {

    }
}
