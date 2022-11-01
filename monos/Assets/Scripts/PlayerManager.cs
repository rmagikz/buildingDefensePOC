using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerManager : MonoBehaviour
{

    [SerializeField] private CinemachineFreeLook cmCamera;
    [SerializeField] private GameObject building;
    
    void Start()
    {
        
    }

    
    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            cmCamera.gameObject.transform.RotateAround(building.transform.position, new Vector3(0,1,0), -0.3f);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            cmCamera.gameObject.transform.RotateAround(building.transform.position, new Vector3(0,1,0), 0.3f);
        }
    }
}
