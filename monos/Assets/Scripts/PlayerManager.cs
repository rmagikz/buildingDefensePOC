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

    public static event Action<SwipeDirection, float> touchSwipe;
    public static event Action touchDown;
    public static event Action<bool, Vector3> touchUp;
    public static event Action touchMoving;

    public static bool playerMovementEnabled = true;

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
        if (Input.touchCount == 1) // GET TOUCH INPUT!!!!
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                wasSwiping = false;
                fp = touch.position;
                lp = touch.position;
                touchDown?.Invoke();
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                lp = touch.position;
                distanceDragged = Vector3.Distance(fp, lp);
                    wasSwiping = true;
                    if (Mathf.Abs(lp.x - fp.x) > Mathf.Abs(lp.y - fp.y))
                    {
                        if (/*(lp.x > fp.x) && */(lp.x > dc.x))
                        {
                            touchSwipe?.Invoke(SwipeDirection.RIGHT, distanceDragged);
                        }
                        else
                        {
                            touchSwipe?.Invoke(SwipeDirection.LEFT, distanceDragged);
                        }
                    }
                    else
                    {
                        if (/*(lp.y > fp.y) &&*/ (lp.y > dc.y))
                        {
                            touchSwipe?.Invoke(SwipeDirection.UP, distanceDragged);
                        }
                        else
                        {
                            touchSwipe?.Invoke(SwipeDirection.DOWN, distanceDragged);
                        }
                    }
                dc = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended) 
            {
                touchUp?.Invoke(wasSwiping, touch.position);
            }
        }

        if (Input.GetKeyDown(KeyCode.Q)) keyQpressed?.Invoke();
        if (Input.GetKeyDown(KeyCode.E)) keyEpressed?.Invoke();
        if (Input.GetKeyDown(KeyCode.Escape)) keyEscapePressed?.Invoke();
        if (!playerMovementEnabled) return;
        if (Input.GetKey(KeyCode.A)) keyAheld?.Invoke();
        if (Input.GetKey(KeyCode.D)) keyDheld?.Invoke();
        if (Input.GetKey(KeyCode.Mouse0)) mouse0held?.Invoke();
    }


}
