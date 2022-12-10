using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DropPod : MonoBehaviour
{
    [SerializeField] float speed = 15;

    private bool hasInit = false;
    private Vector3 m_targetPosition;

    public Action hasLanded;

    public void Init(Vector3 targetPosition) {
        m_targetPosition = targetPosition;
        gameObject.transform.position = new Vector3(UnityEngine.Random.Range(-40,40), 10, UnityEngine.Random.Range(-40,40));
        gameObject.transform.LookAt(targetPosition);
        hasInit = true;
    }

    void Update() {
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, m_targetPosition, speed*Time.deltaTime);
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.transform.tag != "Ground") return;
        hasLanded?.Invoke();
        gameObject.SetActive(false);
    }
}
