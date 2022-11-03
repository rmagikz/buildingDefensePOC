using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static event Action keyAheld;
    public static event Action keyDheld;
    public static event Action keyQpressed;
    public static event Action keyEpressed;
    public static event Action keyEscapePressed;

    public static event Action mouse0held;

    public static bool playerMovementEnabled = true;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) keyQpressed?.Invoke();
        if (Input.GetKeyDown(KeyCode.E)) keyEpressed?.Invoke();
        if (Input.GetKeyDown(KeyCode.Escape)) keyEscapePressed?.Invoke();
        if (!playerMovementEnabled) return;
        if (Input.GetKey(KeyCode.A)) keyAheld?.Invoke();
        if (Input.GetKey(KeyCode.D)) keyDheld?.Invoke();
        if (Input.GetKey(KeyCode.Mouse0)) mouse0held?.Invoke();
    }


}
