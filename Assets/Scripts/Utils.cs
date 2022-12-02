using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
    public static void SpawnTracer(Vector3 from, Vector3 to, GameObject tracer, float timeToDestroy = 0.1f) {
        float distance = Vector3.Distance(from, to);
        Vector3 direction = (to - from).normalized;
        Vector3 tracerSpawnPoint = from + direction * distance/2f;
        GameObject tracerInstance = GameObject.Instantiate(tracer, tracerSpawnPoint, Quaternion.LookRotation(direction));
        tracerInstance.transform.localScale = new Vector3(0.005f, 1, distance / 10f);
        Destroy(tracerInstance, timeToDestroy);
    }

    public static void SpawnImpact(GameObject impact, Vector3 position) {
        Instantiate(impact, position, Quaternion.identity);
    }
}
