using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header ("Main UI")]
    [SerializeField] private GameObject mainUI;
    [SerializeField] private Button logisticsRoom, commsRoom, armoryRoom;
    [Header ("Room UI")]
    [SerializeField] private GameObject roomUI;
    [SerializeField] private Button upgrade1, upgrade2, backButton;
    [Header ("Dependencies")]
    [SerializeField] private CameraManager cameraManager;
    [SerializeField] private BuildingManager buildingManager;

    private bool mainUIenabled = false;

    void Start() {
        ToggleUI();

        CameraManager.roomReached += OnRoomReached;
        CameraManager.buildingReached += OnBuildingReached;

        logisticsRoom.onClick.AddListener(LogisticsClicked);
        commsRoom.onClick.AddListener(CommsClicked);
        armoryRoom.onClick.AddListener(ArmoryClicked);

        backButton.onClick.AddListener(BackClicked);
    }

    void LogisticsClicked() {
        cameraManager.LookAtRoom(buildingManager.logistics);
        mainUI.SetActive(false);
    }

    void CommsClicked() {
        cameraManager.LookAtRoom(buildingManager.comms);
        mainUI.SetActive(false);
    }

    void ArmoryClicked() {
        cameraManager.LookAtRoom(buildingManager.armory);
        mainUI.SetActive(false);
    }

    void BackClicked() {
        cameraManager.LookAtBuilding();
        roomUI.SetActive(false);
    }

    void ToggleUI() {
        if (mainUIenabled) {
            mainUIenabled = false;
            mainUI.SetActive(false);
            roomUI.SetActive(true);
            return;
        }
        mainUIenabled = true;
        mainUI.SetActive(true);
        roomUI.SetActive(false);
    }

    void OnRoomReached(Room room) {
        ToggleUI();
    }

    void OnBuildingReached() {
        ToggleUI();
    }
}
