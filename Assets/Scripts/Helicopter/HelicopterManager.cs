using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicopterManager : MonoBehaviour
{
    [SerializeField] CameraManager cameraManager;
    [SerializeField] BuildingManager buildingManager;
    [SerializeField] GameObject heliPrefab;

    private float rotationSpeed = 10f;
    private float flightTime = 0f;
    
    private bool canBegin = false;
    private bool spinDown = true;
    private bool touchDown = false;
    private float spin = 0.1f;
    private float fireRate = 0f;
    private float timeSinceFire = 0.1f;

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

        UpdateSpin();
        SoundManager.Instance.LerpPitch(ClipName.MinigunSpin, spin, 0.1f, 2f);

        if (touchDown) Fire();

        if (Time.time < flightTime && GameManager.waveInProgress) {
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
        InputManager.touchDown += HandleTouchDown;
        InputManager.touchUp += HandleTouchUp;
        canBegin = true;
        heliScript.minigunStand.SetActive(false);
        heliScript.minigun.transform.position -= new Vector3(0,0.3f,0.1f);
        flightTime = Time.time + GameManager.helicopterFuelTime;
        SoundManager.Instance.PlaySound(ClipName.HelicopterRotor);
        SoundManager.Instance.FadeVolume(ClipName.HelicopterRotor, 0f, 1f, 0.5f);
        SoundManager.Instance.PlaySound(ClipName.MinigunSpin);
    }

    public void EndStrafe() {
        InputManager.touchDown -= HandleTouchDown;
        InputManager.touchUp -= HandleTouchUp;
        GameManager.SetPlayerMovement(true);
        SoundManager.Instance.StopSound(ClipName.MinigunBurst);
        SoundManager.Instance.StopSound(ClipName.MinigunSpin);
        SoundManager.Instance.StopSound(ClipName.HelicopterRotor);
        canBegin = false;
        spinDown = true;
        touchDown = false;
        flightTime = 0f;
        Destroy(heliGO);
    }

    private void HandleTouchDown(Touch touch) {
        touchDown = true;
    }

    private void HandleTouchUp(Touch touch) {
        touchDown = false;
        spinDown = true;
        SoundManager.Instance.StopSound(ClipName.MinigunBurst);
    }

    private void Fire() {
        Vector3 touchRayDirection = cameraManager.CmCamera.ScreenPointToRay(Input.mousePosition).direction;

        Physics.Raycast(cameraManager.CmCamera.transform.position, touchRayDirection, out RaycastHit rayHit);
        heliScript.minigun.transform.LookAt(rayHit.point);

        if (IsSpunUp()) {
            float random() => Random.Range(-0.01f, 0.01f);
            Vector3 finalDirection = touchRayDirection + new Vector3(random(),random(),random());
            Physics.Raycast(cameraManager.CmCamera.transform.position, finalDirection, out RaycastHit shotHit);

            SoundManager.Instance.PlaySound(ClipName.MinigunBurst);

            if (Time.time > timeSinceFire) {
                if (shotHit.transform != null) {
                    if (shotHit.transform.tag == "Enemy") {
                        Enemy enemy = shotHit.transform.GetComponent<Enemy>();
                        Utils.SpawnImpact(Effects.Instance.enemyImpact, shotHit.point);
                        enemy.TakeDamage(1);
                    } else Utils.SpawnImpact(Effects.Instance.groundImpact, shotHit.point);
                    Vector3 offsetPos = heliScript.minigunBarrel.transform.position + (new Vector3(random(), random(), 0) * 5);
                    Utils.SpawnTracer(offsetPos, shotHit.point, Effects.Instance.tracer, 0.01f);
                }
                timeSinceFire = Time.time + fireRate;
            }
        }
    }

    private bool IsSpunUp() {
        spinDown = false;
        if (spin >= 1f) return true;
        return false;
    }

    private void UpdateSpin() {
        if (spinDown) {
            spin -= Time.deltaTime;
            if (spin <= 0.1f) spin = 0.1f;
        } else {
            spin += Time.deltaTime;
            if (spin >= 1f) spin = 1f;
        }
    }
}
