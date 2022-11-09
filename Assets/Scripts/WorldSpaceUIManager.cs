using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldSpaceUIManager : MonoBehaviour
{
    [SerializeField] private Button barracks, logistics, engineering, misc;
    [SerializeField] private CameraManager cameraManager;
    [SerializeField] private BuildingManager buildingManager;

    void Start() {
        barracks.onClick.AddListener(() => cameraManager.LookAtRoom(buildingManager.barracks));
        logistics.onClick.AddListener(() => cameraManager.LookAtRoom(buildingManager.logistics));
        engineering.onClick.AddListener(() => cameraManager.LookAtRoom(buildingManager.engineering));
        misc.onClick.AddListener(() => cameraManager.LookAtRoom(buildingManager.misc));
    }
}
