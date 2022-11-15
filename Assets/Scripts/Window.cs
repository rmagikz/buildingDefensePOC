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

    public float targetDistance = Mathf.Infinity;

    void Start() {
        pewman = FindObjectOfType<PewPewManager>();
    }

    void Update()
    {
        GetTarget();
        if (CanEnqueue()) {
            pewman.firingQueue.Add(this);
            inQueue = true;
        }
    }

    public void SetPriorityTarget(GameObject target) {
        if (currentTarget != null) currentTarget.GetComponent<Enemy>().hasDied -= OnEnemyDied;
        currentTarget = target;
        target.GetComponent<Enemy>().hasDied += OnEnemyDied;
        targetDistance = Vector3.Distance(transform.position, target.transform.position);
        nextFire = 0f;
        inQueue = false;
    }

    public bool CanEnqueue() {
        if (currentTarget != null && Time.time > nextFire && inQueue == false) return true;
        return false;
    }

    public void Reload() {
        nextFire = Time.time + pewman.windowFireRate;
        inQueue = false;
    }

    public void Shoot() 
    {
        if (currentTarget == null) return;
        Vector3 target = currentTarget.transform.position + new Vector3(0,1,0);
        Vector3 direction = (target - transform.position).normalized;

        if (Physics.Raycast(transform.position, direction, out RaycastHit hit)) 
        {
            Debug.DrawLine(transform.position, currentTarget.transform.position, Color.red, 1f);
            if (hit.transform.tag != "Enemy") {currentTarget = null; Debug.Log("UR MOM TWO"); return;}
            float distance = Vector3.Distance(transform.position, target);
            Vector3 spawnPoint = transform.position + direction * distance * 0.5f;
            GameObject tracerInstance = Instantiate(pewman.tracer, spawnPoint, Quaternion.LookRotation(direction));
            tracerInstance.transform.localScale = new Vector3(0.005f, 1, distance / 10f);
            Destroy(tracerInstance, 0.1f);
            AudioManager.instance.PlayOneShot(AudioManager.instance.gunshotSound);
            hit.transform.gameObject.GetComponent<Enemy>().TakeDamage(5);
            Instantiate(pewman.enemyImpact, hit.point, Quaternion.identity);
            AudioManager.instance.PlayOneShot(AudioManager.instance.enemyImpactSound);
            // Instantiate(pewman.groundImpact, hit.point, Quaternion.identity);
            // AudioManager.instance.PlayOneShot(AudioManager.instance.gunshotSound);
            Reload();
        }
    }

    private void GetTarget() {
        if (currentTarget != null) return;
        float smallestDistance = Mathf.Infinity;
        float currentDistance;
        GameObject nearestTarget = null;
        for (int i = 0; i < pewman.enemies.Count; i++) {
            currentDistance = Vector3.Distance(transform.position, pewman.enemies[i].transform.position);
            if (currentDistance < smallestDistance) {
                Vector3 target = pewman.enemies[i].transform.position + new Vector3(0,1,0);
                Vector3 direction = (target - transform.position).normalized;
                if (Physics.Raycast(transform.position, direction, out RaycastHit hit)) {
                    if (hit.transform.tag == "Enemy") {
                        smallestDistance = currentDistance;
                        targetDistance = currentDistance;
                        nearestTarget = pewman.enemies[i];
                    }
                }
            }
        }

        if (nearestTarget != null) {
            currentTarget = nearestTarget;
            nearestTarget.GetComponent<Enemy>().hasDied += OnEnemyDied;
        }
    }

    private void OnEnemyDied(Enemy enemy) {
        enemy.hasDied -= OnEnemyDied;
        currentTarget = null;
        targetDistance = Mathf.Infinity;
    }
}
