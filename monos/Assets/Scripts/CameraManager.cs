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

    private Vector3 bezierLeft = new Vector3(8,0,0);
    private Vector3 bezierRight = new Vector3(-8,0,0);

    private Coroutine rotateAroundCoroutine;

    void Start()
    {
        PlayerManager.keyAheld += () => cmCamera.gameObject.transform.RotateAround(building.transform.position, new Vector3(0,1,0), 0.3f);
        PlayerManager.keyDheld += () => cmCamera.gameObject.transform.RotateAround(building.transform.position, new Vector3(0,1,0), -0.3f);
        PlayerManager.keyQpressed += () => LookAtRoom(1);
        PlayerManager.keyEpressed += () => LookAtRoom(2);
        PlayerManager.keyEscapePressed += () => LookAtBuilding();

        PlayerManager.touchSwipe += (d,t) => rotateAroundCoroutine = StartCoroutine(RotateAround(d, t));
        PlayerManager.touchUp += HandleTouchUp;
    }

    
    void Update()
    {
        if (!PlayerManager.playerMovementEnabled) return;
        cmCamera.transform.LookAt(building.transform);
    }

    public void LookAtRoom(int index) 
    {
        PlayerManager.playerMovementEnabled = false;

        if (currentRoom != null) currentRoom.ToggleWall();
        else previousCameraPosition = cmCamera.transform.position;

        Room target = Array.Find(GameObject.FindObjectsOfType<Room>(), room => room.index == index);
        if (target == null) return;

        currentRoom = target;

        float moveDirection = cmCamera.WorldToViewportPoint(target.lookAt.position).x > 0.5f ? -0.15f : 0.15f;
        cmCamera.transform.DOLookAt(target.lookAt.position, 0.2f).OnComplete(() => FindRoom(target, moveDirection));
        target.ToggleWall();
    }

    public void LookAtBuilding() 
    {
        if (currentRoom != null) currentRoom.ToggleWall();
        currentRoom = null;

        Vector3 p0 = cmCamera.transform.position;
        Vector3 p2 = previousCameraPosition;
        Vector3 p1 = GetDesiredVector(p0,p2);

        //DisplayBezier(p0, p1, p2);

        float t = 0;
        cmCamera.transform.DODynamicLookAt(building.transform.position, 2f).OnUpdate(() => {
            t += Time.deltaTime/2f;
            GetCurve(out Vector3 result, p0, p1, p2, t);
            cmCamera.transform.position = result;
        }).OnComplete(() => PlayerManager.playerMovementEnabled = true);
    }

    private IEnumerator RotateAround(SwipeDirection direction, float distance) {
        float sensitivity = 0.3f * distance/100f;
        float incrementDirection = direction == SwipeDirection.LEFT ? -sensitivity : sensitivity;
        for (int i = 0; i < 60; i++) {
            cmCamera.gameObject.transform.RotateAround(building.transform.position, new Vector3(0,1,0), incrementDirection/60f);
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    private void HandleTouchUp(bool b, Vector3 v) {
        if (rotateAroundCoroutine != null) {
            StopCoroutine(rotateAroundCoroutine);
            rotateAroundCoroutine = null;
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

    private Vector3 GetDesiredVector(Vector3 p0, Vector3 p2) 
    {
        float distanceToLeft = Vector3.Distance(p0, bezierLeft);
        float distanceToRight = Vector3.Distance(p0, bezierRight);

        Vector3 desiredPosition = distanceToLeft < distanceToRight ? bezierLeft : bezierRight;
        return new Vector3 (
            desiredPosition.x,
            p2.y - (p2.y - p0.y)/2,
            desiredPosition.y
        );
    }

    private void DisplayBezier(Vector3 p0, Vector3 p1, Vector3 p2) 
    {
        GameObject primitive = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Instantiate(primitive, p0 , Quaternion.identity);
        Instantiate(primitive, p1 , Quaternion.identity);
        Instantiate(primitive, p2 , Quaternion.identity);
    }

    private void FindRoom(Room target, float moveDirection) 
    {
        while (true) 
        {
            cmCamera.transform.RotateAround(building.transform.position, Vector3.up, moveDirection);
            cmCamera.transform.LookAt(target.lookAt.position);
            if (Physics.Raycast(cmCamera.transform.position, cmCamera.transform.forward, out RaycastHit hit, layermask)) 
            {
                if (hit.transform == target.lookAt) break;
            }
        }
        cmCamera.transform.DOMove(target.targetPos.position, 1f);
        cmCamera.transform.DODynamicLookAt(target.lookAt.position, 1f);
    }
}