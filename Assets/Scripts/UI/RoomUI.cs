using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoomUI : MonoBehaviour
{
    public GameObject mainUI;
    public TMP_Text title, upgrade1, upgrade2;

    void OnEnable() {
        mainUI.SetActive(false);
    }
    
    void OnDisable() {
        mainUI.SetActive(true);
    }
}
