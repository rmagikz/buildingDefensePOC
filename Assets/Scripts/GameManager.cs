using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] EnemyManager enemyManager;
    [SerializeField] MainUI mainUI;
    [SerializeField] WorldSpaceUIManager worldSpaceUIManager;

    public static bool playerMovementEnabled {get; private set;}
    public static bool waveInProgress {get; private set;}

    void Start() {
        MainUI.WaveStarted += OnWaveStarted;
        EnemyManager.WaveEnded += OnWaveEnded;
    }

    public static void SetPlayerMovement(bool state) {
        playerMovementEnabled = state;
    }

    private void OnWaveStarted() {
        waveInProgress = true;
        enemyManager.SetSpawnProperties(5);
        enemyManager.StartSpawning();
    }

    private void OnWaveEnded() {
        waveInProgress = false;
        mainUI.Toggle();
        worldSpaceUIManager.ToggleAll();
    }
}
