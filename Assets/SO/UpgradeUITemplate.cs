using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeUITemplate", menuName = "ScriptableObjects/UpgradeUITemplate", order = 1)]
public class UpgradeUITemplate : ScriptableObject
{
    public string roomName;
    public Sprite roomSprite;

    public string upgrade1Name;
    public Sprite upgrade1Sprite;

    public string upgrade2Name;
    public Sprite upgrade2Sprite;

    public Upgrade[] upgradeIDs = new Upgrade[2];
}
