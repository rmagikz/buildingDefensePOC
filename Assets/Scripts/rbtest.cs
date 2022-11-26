using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rbtest : MonoBehaviour
{
    Rigidbody rb;
    Vector3 initialPos;
    float acceleration;
    Vector3 force;

    float initialX;
    float finalX;
    float initialVx;
    float finalVx;

    GameObject pp;

    void Start()
    {
        force = new Vector3(1f, 10f, 0);
        rb = GetComponent<Rigidbody>();
        acceleration = Physics.gravity.magnitude;

        pp = GameObject.CreatePrimitive(PrimitiveType.Cube);
        pp.GetComponent<BoxCollider>().enabled = false;
        pp.transform.localScale = new Vector3(0.3f,0.3f,0.3f);

        Debug.Log(Physics.gravity);
        Debug.Log(Physics.gravity.magnitude);
        Debug.Log(force.magnitude);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.A)) {
            rb.AddForce(force, ForceMode.Impulse);
        }

        if (Input.GetKeyDown(KeyCode.D)) {
            ImpactPoint();
        }
    }

    void ImpactPoint() {
        float theta = Mathf.Atan(force.y/force.x);
        float x = (Mathf.Pow(force.magnitude,2) * Mathf.Sin(2 * theta))/acceleration;

        pp.transform.position = new Vector3(x + transform.position.x,0,0);
    }

}
