using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private ObjectPool enemyPool;

    public float spawnInterval = 1f;
    //public bool canSpawn = true;
    private float timeSinceSpawn = 0f;

    void Update() {
        if (Time.time > timeSinceSpawn && PlayerManager.waveInProgress) {
            enemyPool.Spawn();
            timeSinceSpawn = Time.time + spawnInterval;
        }
    }
}
