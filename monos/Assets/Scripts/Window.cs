using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : MonoBehaviour
{
    private float nextFire = 0f;
    private GameObject currentTarget;

    public bool canShoot = true;

    void Update()
    {
        if (Time.time > nextFire) canShoot = true;
        else canShoot = false;

        if (currentTarget == null) return;
        Shoot(currentTarget.transform.position + new Vector3(0,1,0));
    }

    public bool SetTarget(GameObject _target, bool priority = false) {
        if (currentTarget != null && priority == false) return false;
        currentTarget = _target;
        currentTarget.GetComponent<Enemy>().targetedCount++;
        return true;
    }

    public bool Shoot(Vector3 target) 
    {
        if (canShoot) 
        {
            nextFire = Time.time + PewPewManager.fireRate;
            
            Vector3 direction = (target - transform.position).normalized;
            if (Physics.Raycast(transform.position, direction, out RaycastHit hit)) 
            {
                // GameObject primitive = GameObject.CreatePrimitive(PrimitiveType.Cube);
                // Instantiate(primitive, hit.point, Quaternion.identity);
                if (hit.transform.tag == "PlayerBuilding") {currentTarget = null; return false;}
                float distance = Vector3.Distance(transform.position, target);
                Vector3 spawnPoint = transform.position + direction * distance * 0.5f;
                PewPewManager pewman = FindObjectOfType<PewPewManager>();
                GameObject tracer = pewman.tracer;
                GameObject tracerInstance = Instantiate(tracer, spawnPoint, Quaternion.LookRotation(direction));
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
            return true;
        }
        return false;
    }
}
