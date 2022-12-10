using UnityEngine;

public partial class GameManager : MonoBehaviour
{
    [SerializeField] EnemyManager enemyManager;
    [SerializeField] WorldSpaceUIManager worldSpaceUIManager;
    [SerializeField] CameraManager cameraManager;


    [SerializeField] GameObject helicopter;

    private UpgradesData upgrades;

    public static bool playerMovementEnabled {get; private set;}
    public static bool waveInProgress {get; private set;}

    void Start() {
        CheckSaves();
        SoundManager.Instance.PlaySound(ClipName.MenuMusic);
        SoundManager.Instance.FadeVolume(ClipName.MenuMusic, 0, 1f, 1f);

        MainUI.WaveStarted += OnWaveStarted; // when player clicks wave button
        EnemyManager.WaveEnded += OnWaveEnded; //all enemies spawned are dead or player lost
        UpgradeUI.UpgradeClicked += OnUpgradeClicked; //an upgrade has been clicked
        GameUI.HelicopterStarted += HelicopterClicked;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.A)) {
            HelicopterClicked();
        }
    }

    public static void SetPlayerMovement(bool state) {
        playerMovementEnabled = state;
    }

    private void CheckSaves() {
        if (false) {
            //upgrades = saves;
        } else {
            upgrades = new UpgradesData();
        }
    }

    private void OnWaveStarted() {
        waveInProgress = true;
        SoundManager.Instance.FadeTracks(ClipName.MenuMusic, ClipName.GameplayMusic, 2f);
    }

    private void OnWaveEnded() {
        SoundManager.Instance.FadeTracks(ClipName.GameplayMusic, ClipName.MenuMusic, 2f);
        UpgradesData.wave++;
        waveInProgress = false;
        worldSpaceUIManager.ToggleAll();
        IncreaseDifficulty();
    }

    private void HelicopterClicked() {
        playerMovementEnabled = false;
        HelicopterManager.HM.SpawnHelicopter();
        cameraManager.LookAtHelicopter(HelicopterManager.HM.BeginStrafe);
    }

    private void OnUpgradeClicked(Upgrade upgrade) {
        switch (upgrade) {
            case Upgrade.FireRate:
                UpgradesData.playerCurrency -= UpgradesData.upgradesData[upgrade];
                UpgradesData.windowFireRate -= windowFireRate * 0.1f;
                UpgradesData.upgradesData[upgrade] += 100;
                break;
            case Upgrade.FiringQueue:
                UpgradesData.playerCurrency -= UpgradesData.upgradesData[upgrade];
                UpgradesData.firingQueueDelay -= firingQueueDelay * 0.1f;
                UpgradesData.upgradesData[upgrade] += 100;
                break;
        }
    }

    private void IncreaseDifficulty() {
        UpgradesData.enemiesToSpawn += (int)(enemiesToSpawn * 0.2f);
        UpgradesData.enemySpawnRate -= enemySpawnRate * 0.05f;
    }
}
