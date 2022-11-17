using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public enum SwipeDirection {UP, DOWN, LEFT, RIGHT};

public class PlayerManager : MonoBehaviour
{
    public static event Action keyAheld;
    public static event Action keyDheld;
    public static event Action keyQpressed;
    public static event Action keyEpressed;
    public static event Action keyEscapePressed;

    public static event Action mouse0held;

    public static event Action<Touch> touchSwipe;
    public static event Action<Touch> touchDown;
    public static event Action<Touch> touchUp;
    public static event Action touchMoving;

    public static bool playerMovementEnabled = false;
    public static bool waveInProgress = false;

    private Vector3 fp;   //First touch position
    private Vector3 lp;   //Last touch position
    private Vector3 dc;
    private float minDragDistance;  //minimum distance for a swipe to be registered
    private float distanceDragged;
    private bool wasSwiping = false;

    void Start()
    {
        minDragDistance = Screen.height * 5 / 100;
    }

    void Update()
    {
        if (!playerMovementEnabled) return;
        if (Input.touchCount == 1) // GET TOUCH INPUT!!!!
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                touchDown?.Invoke(touch);
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                touchSwipe?.Invoke(touch);
            }
            else if (touch.phase == TouchPhase.Ended) 
            {
                touchUp?.Invoke(touch);
            }
        }
    }


}
