using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public enum SwipeDirection {UP, DOWN, LEFT, RIGHT};

public class InputManager : MonoBehaviour
{
    public static event Action<Touch> touchSwipe;
    public static event Action<Touch> touchDown;
    public static event Action<Touch> touchUp;
    public static event Action touchMoving;

    void Update()
    {
        if (Input.touchCount == 1)
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
