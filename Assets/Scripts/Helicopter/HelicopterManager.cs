using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicopterManager : MonoBehaviour
{
    [SerializeField] CameraManager cameraManager;
    [SerializeField] BuildingManager buildingManager;
    [SerializeField] GameObject heliPrefab;
    [SerializeField] HelicopterSound helicopterSound;
    [SerializeField] GameObject tracer;

    private float rotationSpeed = 2f;
    private float flightTime = 0f;
    
    private bool canBegin = false;
    private bool spinDown = true;
    private float spin = 0.1f;
    private float fireRate = 0.1f;
    private float timeSinceFire = 0;

    private Ray touchRay;
    private RaycastHit rayHit;

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

        if (spinDown) {
            spin -= Time.deltaTime;
            if (spin <= 0.1f) spin = 0.1f;
        } else {
            spin += Time.deltaTime;
            if (spin >= 1f) spin = 1f;
        }
        helicopterSound.minigunSpin(spin, 200, 500);

        if (Time.time < flightTime) {
            heliGO.transform.RotateAround(buildingManager.BuildingPosition, Vector3.up, rotationSpeed * Time.deltaTime);
            heliScript.minigunBarrel.transform.eulerAngles += new Vector3(0,0,spin*2);
            cameraManager.CmCamera.transform.position = heliScript.targetPos.position;
            cameraManager.CmCamera.transform.LookAt(heliScript.lookAt);
        } else {
            EndStrafe();
        }

    }

    public void SpawnHelicopter() {
        heliGO = Instantiate(heliPrefab, new Vector3(0,7.5f, -22), Quaternion.identity);
        heliScript = heliGO.GetComponent<Helicopter>();
    }

    public void BeginStrafe() {
        InputManager.touchSwipe += HandleTouchDown;
        InputManager.touchUp += HandleTouchUp;
        canBegin = true;
        heliScript.minigunStand.SetActive(false);
        heliScript.minigun.transform.position -= new Vector3(0,0.3f,0.1f);
        flightTime = Time.time + GameManager.helicopterFuelTime;
    }

    public void EndStrafe() {
        InputManager.touchSwipe -= HandleTouchDown;
        InputManager.touchUp -= HandleTouchUp;
        GameManager.SetPlayerMovement(true);
        canBegin = false;
        flightTime = 0f;
        Destroy(heliGO);
    }

    private void HandleTouchDown(Touch touch) {
        touchRay = cameraManager.CmCamera.ScreenPointToRay(touch.position);
        Physics.Raycast(touchRay, out rayHit);
        heliScript.minigun.transform.LookAt(rayHit.point);
        Fire();
    }

    private void HandleTouchUp(Touch touch) {
        spinDown = true;
        AudioManager.instance.StopClip();
    }

    private void Fire() {
        if (SpinUp()) {
            Debug.Log("YEET");
            AudioManager.instance.PlayClip(AudioManager.instance.minigunFire, 0.1f);
            if (Time.time > timeSinceFire) {
                float distance = Vector3.Distance(rayHit.point, heliScript.minigunBarrel.transform.position);
                Vector3 tracerSpawnPoint = heliScript.minigunBarrel.transform.position + touchRay.direction * distance/2f;
                GameObject tracerInstance = GameObject.Instantiate(tracer, tracerSpawnPoint, Quaternion.LookRotation(touchRay.direction));
                tracerInstance.transform.localScale = new Vector3(0.005f, 1, distance / 10f);
                Destroy(tracerInstance, 0.1f);
                timeSinceFire = Time.time + fireRate;
            }
        }
    }

    private bool SpinUp() {
        spinDown = false;
        if (spin >= 1f) return true;
        return false;
    }
}
