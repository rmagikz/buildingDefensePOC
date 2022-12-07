using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using DG.Tweening;

public class CameraManager : MonoBehaviour
{

    [SerializeField] private Camera cmCamera;
    [SerializeField] private UICutsceneHelper uICutsceneHelper;
    [SerializeField] private GameObject building;

    private Vector3 previousCameraPosition;
    private Room currentRoom = null;

    private bool inHelicopter = false;

    public Material materialRed;

    public Camera CmCamera { get => cmCamera; set => cmCamera = value; }

    public static event Action<Room> roomReached;
    public static event Action buildingReached;

    public float cameraTime;

    void Start()
    {
        Cutscenes.SetCamera(cmCamera);

        InputManager.touchSwipe += (touch) => {
            if (GameManager.playerMovementEnabled)
                StartCoroutine(RotateAround(touch));
        };
    }

    
    void Update()
    {
        if (GameManager.playerMovementEnabled)
            cmCamera.transform.LookAt(building.transform);
    }

    public void LookAtRoom(Room room, Action OnComplete = null) 
    {
        GameManager.SetPlayerMovement(false);

        if (currentRoom != null) currentRoom.ToggleWall();
        else previousCameraPosition = cmCamera.transform.position;

        currentRoom = room;

        Cutscenes.SetCallback(1f, OnComplete);
        Cutscenes.StartCutscene(room.targetPos.position, room.lookAt.position, building.transform.position);

        room.ToggleWall();
    }

    public void LookAtBuilding(Action OnComplete = null) 
    {
        if (currentRoom != null) currentRoom.ToggleWall();

        Cutscenes.SetCallback(1f, () => {GameManager.SetPlayerMovement(true); OnComplete?.Invoke();});
        Cutscenes.StartCutscene(previousCameraPosition, building.transform.position, currentRoom.lookAt.position);

        currentRoom = null;
    }

    public void LookAtHelicopter(Action onComplete = null) {
        currentRoom = null;

        Vector3 targetPos = HelicopterManager.HM.heliScript.targetPos.position;
        Vector3 lookAt = HelicopterManager.HM.heliScript.lookAt.position;

        Cutscenes.SetCallback(0.5f, () => uICutsceneHelper.FadeToBlack(0.5f));
        Cutscenes.SetCallback(1f, onComplete);

        Cutscenes.StartCutscene(targetPos, lookAt, building.transform.position);
    }

    private IEnumerator RotateAround(Touch touch) {
        float deltaPos = touch.deltaPosition.x;
        float sensitivity = deltaPos / (Screen.width/360);
        for (int i = 0; i < 60; i++) {
            cmCamera.transform.RotateAround(building.transform.position, Vector3.up, sensitivity/60f);
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
}