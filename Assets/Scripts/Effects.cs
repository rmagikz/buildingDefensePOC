using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effects : MonoBehaviour
{
    public GameObject tracer;
    public GameObject enemyImpact;
    public GameObject groundImpact;

    public static Effects Instance;

    void Start() {
        if (Instance != null && Instance != this) {
            Destroy(this);
        } else {
            Instance = this;
        }
    }
}
