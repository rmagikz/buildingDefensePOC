using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : MonoBehaviour
{
    private float nextFire = 0f;

    public bool canShoot = true;

    void Update()
    {
        if (Time.time > nextFire) canShoot = true;
        else canShoot = false;
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

                float distance = Vector3.Distance(transform.position, target);
                Vector3 spawnPoint = transform.position + direction * distance * 0.5f;
                PewPewManager pewman = FindObjectOfType<PewPewManager>();
                GameObject tracer = pewman.tracer;
                GameObject tracerInstance = Instantiate(tracer, spawnPoint, Quaternion.LookRotation(direction));
                tracerInstance.transform.localScale = new Vector3(0.005f, 1, distance / 10f);
                Destroy(tracerInstance, 0.1f);
                Instantiate(pewman.groundImpact, hit.point, Quaternion.identity);
                if (hit.transform.tag == "Enemy")
                    Destroy(hit.transform.gameObject, 0.1f);
            }
            return true;
        }
        return false;
    }
}
