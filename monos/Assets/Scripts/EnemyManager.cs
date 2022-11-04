using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private ObjectPool enemyPool;

    public float spawnInterval = 1f;
    private float timeSinceSpawn = 0f;

    void Update() {
        if (Time.time > timeSinceSpawn) {
            enemyPool.Spawn();
            timeSinceSpawn = Time.time + spawnInterval;
        }
    }
}
