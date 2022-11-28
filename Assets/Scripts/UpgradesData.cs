using System.Collections.Generic;
using UnityEngine;

public enum Upgrade {FireRate, FiringQueue}

public partial class GameManager {

    // private UpgradesData class with a bunch of getters for other classes to access while maintaining set access only from within GameManager
    // no friend class in c# :(

    public static Dictionary<Upgrade, int> upgradesData {get => UpgradesData.upgradesData;}
    public static float firingQueueDelay { get => UpgradesData.firingQueueDelay;}
    public static int enemiesToSpawn { get => UpgradesData.enemiesToSpawn;}
    public static float enemySpawnRate { get => UpgradesData.enemySpawnRate;}
    public static float windowFireRate { get => UpgradesData.windowFireRate;}
    public static float helicopterFuelTime { get => UpgradesData.helicopterFuelTime;}
    public static int playerCurrency { get => UpgradesData.playerCurrency;}
    public static int wave { get => UpgradesData.wave; }

    private class UpgradesData
    {
        public static Dictionary<Upgrade, int> upgradesData = new Dictionary<Upgrade, int>() {
            {Upgrade.FireRate, 100},
            {Upgrade.FiringQueue, 150}
        };

        public static float windowFireRate = 1f;
        public static float firingQueueDelay = 0.5f;
        public static float enemySpawnRate = 1f;
        public static int enemiesToSpawn = 10;
        public static float helicopterFuelTime = 10f;
        public static int playerCurrency = 130;
        public static int wave = 1;
    }
}

