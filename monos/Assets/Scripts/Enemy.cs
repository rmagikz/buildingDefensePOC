using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Enemy : MonoBehaviour
{
    private Vector3 targetPosition;
    
    public ObjectPool parentPool;
    public float health = 10;

    private Tweener moveTween;

    void Start() {
        targetPosition = GameObject.FindGameObjectWithTag("PlayerBuilding").transform.position;
    }

    void OnEnable() {
        health = 10;

        float radius = Random.Range(20f,25f);
        float theta = Random.Range(0f, Mathf.PI*2);
        float x = radius * Mathf.Cos(theta);
        float y = radius * Mathf.Sin(theta);

        transform.position = new Vector3(x, 0, y);

        gameObject.transform.LookAt(targetPosition);
        moveTween = gameObject.transform.DOMove(targetPosition, 20f);
    }

    public void Destroy() {
        moveTween.Kill();
        if (parentPool) parentPool.Remove(gameObject);
        else Destroy(gameObject);
    }

    public void TakeDamage(float damage) {
        health -= damage;
        if (health <= 0) Destroy();
    }
}
