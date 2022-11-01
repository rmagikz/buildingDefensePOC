using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Enemy : MonoBehaviour
{
    private Vector3 targetPosition;
    
    public ObjectPool parentPool;

    void Start() {
        targetPosition = GameObject.FindGameObjectWithTag("PlayerBuilding").transform.position;
    }

    void OnEnable() {
        float radius = Random.Range(20f,25f);
        float theta = Random.Range(0f, Mathf.PI*2);
        float x = radius * Mathf.Cos(theta);
        float y = radius * Mathf.Sin(theta);
        transform.position = new Vector3(x, 0, y);
        gameObject.transform.LookAt(targetPosition);
        gameObject.transform.DOMove(targetPosition, 10f);
    }

    public void Destroy() {
        if (parentPool) parentPool.Remove(gameObject);
        else Destroy(gameObject);
    }
}
