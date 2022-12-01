using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using DG.Tweening;

public class CameraManager : MonoBehaviour
{

    [SerializeField] private Camera cmCamera;
    [SerializeField] private GameObject building;

    public LayerMask layermask;

    private Vector3 previousCameraPosition;
    private Room currentRoom = null;

    private bool inHelicopter = false;

    public Material materialRed;

    public Camera CmCamera { get => cmCamera; set => cmCamera = value; }

    public static event Action<Room> roomReached;
    public static event Action buildingReached;

    void Start()
    {
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

    public void LookAtRoom(Room room, Action OnComplete) 
    {
        GameManager.SetPlayerMovement(false);

        if (currentRoom != null) currentRoom.ToggleWall();
        else previousCameraPosition = cmCamera.transform.position;

        currentRoom = room;

        StartCoroutine(BezierMove(room.targetPos.position, room.lookAt.position, building.transform.position, () => OnComplete?.Invoke()));
        room.ToggleWall();
    }

    public void LookAtBuilding() 
    {
        if (currentRoom != null) currentRoom.ToggleWall();

        StartCoroutine(BezierMove(previousCameraPosition, building.transform.position, currentRoom.lookAt.position, () => GameManager.SetPlayerMovement(true)));
        currentRoom = null;
    }

    public void LookAtHelicopter(Action onComplete) {
        currentRoom = null;

        Vector3 targetPos = HelicopterManager.HM.heliScript.targetPos.position;
        Vector3 lookAt = HelicopterManager.HM.heliScript.lookAt.position;

        StartCoroutine(BezierMove(targetPos, lookAt, building.transform.position, onComplete));
    }

    private IEnumerator RotateAround(Touch touch) {
        float deltaPos = touch.deltaPosition.x;
        float sensitivity = deltaPos / (Screen.width/360);
        for (int i = 0; i < 60; i++) {
            cmCamera.transform.RotateAround(building.transform.position, Vector3.up, sensitivity/60f);
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    private void GetCurve(out Vector3 result, Vector3 p0, Vector3 p1, Vector3 p2, float time) 
    {
        float tt = time * time;
        float u = 1f - time;
        float uu = u * u;

        result = u * p0;
        result += 2f * u * p1 * time;
        result += tt * p2;
    }

    private void DisplayBezier(Vector3 p0, Vector3 p1, Vector3 p2) // utility function to display bezier points
    {
        GameObject primitive = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Instantiate(primitive, p0 , Quaternion.identity);
        Instantiate(primitive, p1 , Quaternion.identity).GetComponent<MeshRenderer>().material = materialRed;
        Instantiate(primitive, p2 , Quaternion.identity);
        Destroy(primitive);
    }

    private IEnumerator BezierMove(Vector3 targetPos, Vector3 lookAt, Vector3 fromLookAt, Action OnComplete) {
        Vector3 p0 = cmCamera.transform.position;
        Vector3 p2 = targetPos;
        Vector3 p1 = (p0 + ((p2 - p0) / 2));
        p1.y -= Vector3.Distance(p0, p2)/4;

        float t = 0;
        while (true) {
            t += Time.deltaTime;
            GetCurve(out Vector3 result, p0, p1, p2, t);
            cmCamera.transform.position = result;
            Vector3 lookAtCorrected = t * lookAt + (1 - t) * fromLookAt;
            cmCamera.transform.LookAt(lookAtCorrected);

            if (t >= 1) {
                cmCamera.transform.position = targetPos;
                cmCamera.transform.LookAt(lookAt);
                OnComplete?.Invoke();
                yield break;
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
}