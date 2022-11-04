using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : MonoBehaviour
{
    private float nextFire = 0f;
    public GameObject currentTarget;
    private PewPewManager pewman;

    public bool canShoot = true;
    public bool inQueue = false;

    void Start() {
        pewman = FindObjectOfType<PewPewManager>();
    }

    void Update()
    {
        if (CanEnqueue()) {
            pewman.EnterQueue(this);
            inQueue = true;
        }
    }

    public bool SetTarget(GameObject _target, bool priority = false) {
        if (currentTarget != null && priority == false) return false;
        currentTarget = _target;
        currentTarget.GetComponent<Enemy>().targetedCount++;
        return true;
    }

    public bool CanEnqueue() {
        if (currentTarget != null && Time.time > nextFire && inQueue == false) return true;
        return false;
    }

    public void Reload() {
        nextFire = Time.time + pewman.windowFireRate;
    }

    public void Shoot() 
    {
        if (currentTarget == null) return;
        Vector3 target = currentTarget.transform.position + new Vector3(0,1,0);
        
        Vector3 direction = (target - transform.position).normalized;
        if (Physics.Raycast(transform.position, direction, out RaycastHit hit)) 
        {
            if (hit.transform.tag != "Enemy") {currentTarget = null; return;}
            float distance = Vector3.Distance(transform.position, target);
            Vector3 spawnPoint = transform.position + direction * distance * 0.5f;
            GameObject tracerInstance = Instantiate(pewman.tracer, spawnPoint, Quaternion.LookRotation(direction));
            tracerInstance.transform.localScale = new Vector3(0.005f, 1, distance / 10f);
            Destroy(tracerInstance, 0.1f);
            AudioManager.instance.PlayOneShot(AudioManager.instance.gunshotSound);
            if (hit.transform.tag == "Enemy") {
                if(hit.transform.gameObject.GetComponent<Enemy>().TakeDamage(5)) currentTarget = null;
                Instantiate(pewman.enemyImpact, hit.point, Quaternion.identity);
                AudioManager.instance.PlayOneShot(AudioManager.instance.enemyImpactSound);
            }
            else {
                Instantiate(pewman.groundImpact, hit.point, Quaternion.identity);
                AudioManager.instance.PlayOneShot(AudioManager.instance.gunshotSound);
            }
        }
    }
}
