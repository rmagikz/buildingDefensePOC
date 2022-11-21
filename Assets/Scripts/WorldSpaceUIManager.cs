using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class WorldSpaceUIManager : MonoBehaviour
{
    [SerializeField] private Button barracks, logistics, engineering, misc;
    [SerializeField] private CameraManager cameraManager;
    [SerializeField] private BuildingManager buildingManager;
    [SerializeField] private UpgradeUIManager upgradeUIManager;
    [SerializeField] private MainUI mainUI;

    private bool buttonsEnabled = true;

    void Start() {
        barracks.onClick.AddListener(BarracksClicked);
        logistics.onClick.AddListener(LogisticsClicked);
        engineering.onClick.AddListener(EngineeringClicked);
        misc.onClick.AddListener(MiscClicked);

        ToggleAll();
    }

    public void Toggle(Button p) {
        if (p.enabled) {
            p.enabled = false;
            p.transform.DOScale(new Vector3(0,0,0), .5f);
        } else {
            p.enabled = true;
            p.transform.DOScale(new Vector3(1,1,1), .5f);
        }
    }

    public void ToggleAll() {
        Toggle(barracks);
        Toggle(logistics);
        Toggle(engineering);
        Toggle(misc);
    }

    private void BarracksClicked() {
        cameraManager.LookAtRoom(buildingManager.barracks);
        //Toggle(barracks);
        ToggleAll();
        upgradeUIManager.Toggle();
        mainUI.Toggle();
    }

    private void LogisticsClicked() {
        cameraManager.LookAtRoom(buildingManager.logistics);
        //Toggle(logistics);
        ToggleAll();
        upgradeUIManager.Toggle();
        mainUI.Toggle();
    }

    private void EngineeringClicked() {
        cameraManager.LookAtRoom(buildingManager.engineering);
        //Toggle(engineering);
        ToggleAll();
        upgradeUIManager.Toggle();
        mainUI.Toggle();
    }

    private void MiscClicked() {
        cameraManager.LookAtRoom(buildingManager.misc);
        //Toggle(misc);
        ToggleAll();
        upgradeUIManager.Toggle();
    }
}
