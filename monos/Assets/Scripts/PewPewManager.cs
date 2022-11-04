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
    public float windowFireRate = 1f;
    public float targetAcquisitionDelay = 5f;
    public float firingQueueDelay = 0.5f;
    private float timeSinceTargetAcquired = 0f;
    private float timeSinceFiringQueue = 0f;

    public Queue<Window> firingQueue = new Queue<Window>();

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
        if (Time.time > timeSinceTargetAcquired) 
        {
            AsssignTargets();
            timeSinceTargetAcquired = Time.time + targetAcquisitionDelay;
        }

        if (Time.time > timeSinceFiringQueue)
        {
            if(firingQueue.TryDequeue(out Window window)) {
                window.Shoot();
                window.inQueue = false;
                window.Reload();
                timeSinceFiringQueue = Time.time + firingQueueDelay;
            }
        }
    }

    private void AsssignTargets() {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject nearestTarget = null;

        float lowestDistance = Mathf.Infinity;
        float currentDistance = 0f;

        for (int i = 0; i < windows.Length; i++) {
            for (int j = 0; j < enemies.Length; j++) {
                currentDistance = Vector3.Distance(windows[i].position, enemies[j].transform.position);
                if (currentDistance < lowestDistance) {
                    if (Physics.Raycast(windows[i].position, enemies[j].transform.position + new Vector3(0,1,0) - windows[i].position, out RaycastHit hit)) {
                        //Debug.Log(hit.transform.name);
                        if (hit.transform.tag == "Enemy" && enemies[j].GetComponent<Enemy>().targetedCount < 1) {
                            lowestDistance = currentDistance;
                            nearestTarget = enemies[j];
                        }
                    }
                }
            }

            if (nearestTarget != null) windows[i].GetComponent<Window>().SetTarget(nearestTarget);
        }
    }

    public void EnterQueue(Window window) {
        firingQueue.Enqueue(window);
    }
}
