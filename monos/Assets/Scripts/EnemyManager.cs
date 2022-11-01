using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private ObjectPool enemyPool;

    void Start()
    {
        StartCoroutine(SpawnEnemies(2f));
    }

    private IEnumerator SpawnEnemies(float interval) {
        while (true) {
        enemyPool.Spawn();
        yield return new WaitForSeconds(interval);
        }
    }
}
