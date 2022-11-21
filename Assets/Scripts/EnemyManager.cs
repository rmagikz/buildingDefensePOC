using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private ObjectPool enemyPool;

    public static event Action WaveEnded;

    public float spawnInterval = 1f;

    public bool canSpawn = true;

    public int enemiesToSpawn;

    private float timeSinceSpawn = 0f;
    private int enemiesSpawned = 0;

    void Update() {
        if (Time.time > timeSinceSpawn && GameManager.waveInProgress && canSpawn) {
            if (enemiesSpawned > enemiesToSpawn) {StopSpawning(); return;}
            enemyPool.Spawn();
            enemiesSpawned++;
            timeSinceSpawn = Time.time + spawnInterval;
        }
    }

    public void StartSpawning() {
        canSpawn = true;
    }

    public void StopSpawning() {
        canSpawn = false;
        enemiesSpawned = 0;
        timeSinceSpawn = 0f;
        WaveEnded?.Invoke();
    }

    public void SetSpawnProperties(int count, float rate = 1f) {
        enemiesToSpawn = count;
        spawnInterval = rate;
    }
}
