using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicopterManager : MonoBehaviour
{
    [SerializeField] CameraManager cameraManager;
    [SerializeField] BuildingManager buildingManager;
    [SerializeField] GameObject heliPrefab;

    private float rotationSpeed = 0.3f;
    private float flightTime = 0f;
    
    private bool canBegin = false;

    public static HelicopterManager HM;

    public GameObject heliGO;
    public Helicopter heliScript;

    void Start()
    {
        HM = this;
    }

    void Update()
    {
        if (!canBegin) return;

        if (Time.time < flightTime) {
            heliGO.transform.RotateAround(buildingManager.BuildingPosition, Vector3.up, rotationSpeed * Time.deltaTime);
            cameraManager.CmCamera.transform.position = heliGO.transform.position;
            cameraManager.CmCamera.transform.rotation = heliGO.transform.rotation;
        } else {
            EndStrafe();
        }

    }

    public void SpawnHelicopter() {
        heliGO = Instantiate(heliPrefab, new Vector3(0,7.5f, -22), Quaternion.identity);
        heliScript = heliGO.GetComponent<Helicopter>();
    }

    public void BeginStrafe() {
        canBegin = true;
        flightTime = Time.time + GameManager.helicopterFuelTime;
    }

    public void EndStrafe() {
        GameManager.SetPlayerMovement(true);
        canBegin = false;
        flightTime = 0f;
        Destroy(heliGO);
    }
}
