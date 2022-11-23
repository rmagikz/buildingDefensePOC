using System;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private ObjectPool enemyPool;

    public static event Action WaveEnded;

    private float timeSinceSpawn = 0f;
    private int enemiesSpawned = 0;
    private int enemiesKilled = 0;

    void Start() {
        Enemy.EnemyKilled += () => enemiesKilled++;
    }

    void Update() {
        if (GameManager.waveInProgress && Time.time > timeSinceSpawn && enemiesSpawned < GameManager.enemiesToSpawn) {
            enemyPool.Spawn();
            enemiesSpawned++;
            timeSinceSpawn = Time.time + GameManager.enemySpawnRate;
        }

        if (enemiesKilled == GameManager.enemiesToSpawn) EndWave();
    }

    public void EndWave() {
        enemiesSpawned = 0;
        enemiesKilled = 0;
        timeSinceSpawn = 0f;
        WaveEnded?.Invoke();
    }
}
