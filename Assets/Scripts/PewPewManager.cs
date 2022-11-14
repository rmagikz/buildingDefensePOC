using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

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

        for (int i = 0; i < windows.Length; i++) {
            float lowestDistance = Mathf.Infinity;
            float currentDistance = 0f;
            GameObject nearestTarget = null;

            for (int j = 0; j < enemies.Length; j++) {
                if (enemies[j].GetComponent<Enemy>().targetedCount > 1) continue;
                currentDistance = Vector3.Distance(windows[i].position, enemies[j].transform.position);
                
                if (currentDistance < lowestDistance) {
                    if (Physics.Raycast(windows[i].position, enemies[j].transform.position + new Vector3(0,1,0) - windows[i].position, out RaycastHit hit)) {
                        if (hit.transform.tag == "Enemy") {
                            lowestDistance = currentDistance;
                            nearestTarget = enemies[j];
                        }
                    }
                }
            }

            if (nearestTarget != null && windows[i].GetComponent<Window>().SetTarget(nearestTarget)) {
                nearestTarget.GetComponent<Enemy>().targetedCount++;
            }
        }
    }

    public void EnterQueue(Window window) {
        firingQueue.Enqueue(window);
    }
}
