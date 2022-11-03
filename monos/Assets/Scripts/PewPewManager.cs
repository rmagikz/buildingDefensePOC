using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PewPewManager : MonoBehaviour
{
    [SerializeField] private Camera cmCamera;
    [SerializeField] private GameObject windowsParent;
    
    private Transform[] windows;

    public GameObject tracer;
    public ParticleSystem groundImpact;
    public ParticleSystem enemyImpact;
    public static float fireRate = 2f;
    public float soldierDelay = 0.5f;
    public float soldierReady = 0f;

    void Start()
    {
        PlayerManager.mouse0held += Trigger;

        windows = new Transform[windowsParent.transform.childCount];
        for (int i = 0; i < windowsParent.transform.childCount; i++) 
        {
            windows[i] = windowsParent.transform.GetChild(i);
            windows[i].gameObject.AddComponent<Window>();
        }
    }

    private void Trigger() 
    {
        if (Time.time > soldierReady) 
        {
            ShootFromNearest();
            soldierReady = Time.time + soldierDelay;
        }
    }

    private void ShootFromNearest() 
    {
        Ray stw = cmCamera.ScreenPointToRay(Input.mousePosition);
        float lowestDistance = Mathf.Infinity;
        Transform nearest = null;
        if (Physics.Raycast(stw.origin, stw.direction, out RaycastHit hit)) 
        {
            for (int i = 0; i < windows.Length; i++) 
            {
                float distance = Vector3.Distance(windows[i].position, hit.point);
                if (distance < lowestDistance && windows[i].GetComponent<Window>().canShoot) 
                {
                    lowestDistance = distance;
                    nearest = windows[i];
                }
            }
            if (nearest != null)
                nearest.GetComponent<Window>().Shoot(hit.point);
        }
    }
}
