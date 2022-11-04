using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PewPewManager : MonoBehaviour
{
    [SerializeField] private Camera cmCamera;
    [SerializeField] private GameObject windowsParent;
    [SerializeField] private GameObject building;
    
    private Transform[] windows;

    public GameObject tracer;
    public ParticleSystem groundImpact;
    public ParticleSystem enemyImpact;
    public static float fireRate = 5f;
    public float soldierDelay = 5f;
    public float soldierReady = 0f;

    void Start()
    {
        windows = new Transform[windowsParent.transform.childCount];
        for (int i = 0; i < windowsParent.transform.childCount; i++) 
        {
            windows[i] = windowsParent.transform.GetChild(i);
            windows[i].gameObject.AddComponent<Window>();
        }
    }

    void Update() {
        if (Time.time > soldierReady) 
        {
            FindTarget();
            soldierReady = Time.time + soldierDelay;
        }
    }

    private void FindTarget() 
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        Transform nearestWindow = null;
        GameObject nearestTarget = null;

        float lowestDistance = Mathf.Infinity;
        float currentDistance = 0f;

        for (int i = 0; i < enemies.Length; i++) {
            currentDistance = Vector3.Distance(building.transform.position, enemies[i].transform.position);
            if (currentDistance < lowestDistance && enemies[i].GetComponent<Enemy>().targetedCount < 2) {
                lowestDistance = currentDistance;
                nearestTarget = enemies[i];
            }
        }

        if (nearestTarget == null) return;

        lowestDistance = Mathf.Infinity;

        for (int i = 0; i < windows.Length; i++) {
            currentDistance = Vector3.Distance(windows[i].position, nearestTarget.transform.position);
            if (currentDistance < lowestDistance && windows[i].GetComponent<Window>().canShoot) {
                lowestDistance = currentDistance;
                nearestWindow = windows[i];
            }
        }

        if (nearestWindow != null) nearestWindow.GetComponent<Window>().SetTarget(nearestTarget);
    }
}
