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
    public float firingQueueDelay = 0.5f;
    private float timeSinceFiringQueue = 0f;

    public List<GameObject> enemies;

    public List<Window> firingQueue = new List<Window>();

    void Start()
    {
        int windowsCount = windowsParent.transform.childCount;
        windows = new Transform[windowsCount];
        for (int i = 0; i < windowsCount; i++) 
        {
            windows[i] = windowsParent.transform.GetChild(i);
            windows[i].gameObject.AddComponent<Window>();
        }

        PlayerManager.touchDown += HandleTouchDown;
    }

    void Update() {
        enemies = GameObject.FindGameObjectsWithTag("Enemy").ToList();

        if (Time.time > timeSinceFiringQueue)
        {
            float lowestDistance = Mathf.Infinity;
            float currentDistance = -1f;
            Window nearestWindow = null;

            for (int i = 0; i < firingQueue.Count; i++) {
                if (firingQueue[i].targetDistance < lowestDistance) {
                    currentDistance = firingQueue[i].targetDistance;
                    nearestWindow = firingQueue[i];
                }
            }

            if (nearestWindow != null) {
                nearestWindow.Shoot();
                firingQueue.Remove(nearestWindow);
            }

            timeSinceFiringQueue = Time.time + firingQueueDelay;
        }
    }

    private void HandleTouchDown(Touch touch) {
        Vector3 direction = cmCamera.ScreenPointToRay(touch.position).direction;
        if (Physics.Raycast(cmCamera.gameObject.transform.position, direction, out RaycastHit hit)) {
            if (hit.transform.tag == "Enemy") {
                hit.transform.GetComponent<Enemy>().ShowAsPriority(true);
                firingQueue = new List<Window>();
                for (int i = 0; i < windows.Count(); i++) {
                    if (Physics.Raycast(windows[i].position, hit.transform.position + new Vector3(0,1,0) - windows[i].position, out RaycastHit hit2)) {
                        if (hit2.transform.tag == "Enemy") {
                            windows[i].GetComponent<Window>().SetPriorityTarget(hit.transform.gameObject);
                        }
                    }
                }
            }
        }
    }
}
