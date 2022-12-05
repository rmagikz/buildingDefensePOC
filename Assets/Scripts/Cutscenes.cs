using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public static class Cutscenes
{
    private static Camera cmCamera;
    private static CameraManager cameraManager;

    private static Dictionary<float, Action> callbacks = new Dictionary<float, Action>();

    public static void SetCamera(Camera camera) {
        cmCamera = camera;
        cameraManager = camera.GetComponent<CameraManager>();
    }

    public static void StartCutscene(Vector3 targetPos, Vector3 lookAt, Vector3 fromLookAt) {
        cameraManager.StartCoroutine(BezierMove(targetPos,lookAt,fromLookAt));
    }

    public static void SetCallback(float time, Action callback) {
        if (time > 1) {Debug.Log("Callback time must be 1 or less"); return;}
        callbacks[time] = callback;
    }

    private static void InvokeCallback(float time) {
        callbacks[time]?.Invoke();
        callbacks[time] = null;
    }

    private static IEnumerator BezierMove(Vector3 targetPos, Vector3 lookAt, Vector3 fromLookAt) {
        Vector3 p0 = cmCamera.transform.position;
        Vector3 p2 = targetPos;
        Vector3 p1 = (p0 + ((p2 - p0) / 2));
        p1.y -= Vector3.Distance(p0, p2)/4;

        float cameraTime = 0;
        while (true) {
            cameraTime += Time.deltaTime;
            GetCurve(out Vector3 result, p0, p1, p2, cameraTime);
            cmCamera.transform.position = result;
            Vector3 lookAtCorrected = cameraTime * lookAt + (1 - cameraTime) * fromLookAt;
            cmCamera.transform.LookAt(lookAtCorrected);

            if (callbacks.ContainsKey(float.Parse(cameraTime.ToString("F1")))) InvokeCallback(float.Parse(cameraTime.ToString("F1"))); // cancer

            if (cameraTime >= 1) {
                cmCamera.transform.position = targetPos;
                cmCamera.transform.LookAt(lookAt);
                cameraTime = 0;
                yield break;
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    private static void GetCurve(out Vector3 result, Vector3 p0, Vector3 p1, Vector3 p2, float time) 
    {
        float tt = time * time;
        float u = 1f - time;
        float uu = u * u;

        result = u * p0;
        result += 2f * u * p1 * time;
        result += tt * p2;
    }
}
