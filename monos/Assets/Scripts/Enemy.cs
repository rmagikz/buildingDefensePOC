using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Enemy : MonoBehaviour
{   
    public ObjectPool parentPool;
    public float health = 10;

    public int targetedCount = 0;

    private Vector3 targetPosition;

    void Start() {
        targetPosition = GameObject.FindGameObjectWithTag("PlayerBuilding").transform.position;
    }

    void OnEnable() {
        health = 10;
        targetedCount = 0;

        float radius = Random.Range(30f,40f);
        float theta = Random.Range(0f, Mathf.PI*2);
        float x = radius * Mathf.Cos(theta);
        float y = radius * Mathf.Sin(theta);

        transform.position = new Vector3(x, 0, y);

        gameObject.transform.LookAt(targetPosition);
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, targetPosition, 0.3f);
    }

    public void Destroy() {
        if (parentPool) parentPool.Remove(gameObject);
        else Destroy(gameObject);
    }

    public bool TakeDamage(float damage) {
        health -= damage;
        if (health <= 0) {Destroy(); return true;}
        return false;
    }
}
