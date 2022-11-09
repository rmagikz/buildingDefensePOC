using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public string roomName;
    [SerializeField] private GameObject wall;
    public Transform targetPos;
    public Transform lookAt;
    public int index;

    private bool isWallEnabled = true;

    public void ToggleWall() {
        isWallEnabled = !isWallEnabled;
        if (isWallEnabled) wall.SetActive(true);
        else wall.SetActive(false);
    }
}
