using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class WorldSpaceUIManager : MonoBehaviour
{
    [SerializeField] private Button barracks, logistics, engineering, misc;
    [SerializeField] private CameraManager cameraManager;
    [SerializeField] private BuildingManager buildingManager;
    [SerializeField] private UpgradeUI upgradeUI;
    [SerializeField] private MainUI mainUI;

    private bool buttonsEnabled = true;

    void Start() {
        barracks.onClick.AddListener(() => ButtonClicked(buildingManager.Barracks, upgradeUI.BarracksTemplate));
        logistics.onClick.AddListener(() => ButtonClicked(buildingManager.Logistics, upgradeUI.LogisticsTemplate));
        engineering.onClick.AddListener(() => ButtonClicked(buildingManager.Engineering, upgradeUI.EngineeringTemplate));
        misc.onClick.AddListener(() => ButtonClicked(buildingManager.Misc, upgradeUI.MiscTemplate));

        ToggleAll();
    }

    public void Toggle(Button p) {
        if (p.enabled) {
            p.enabled = false;
            p.transform.DOScale(new Vector3(0,0,0), .5f);
        } else {
            p.enabled = true;
            p.transform.DOScale(new Vector3(1,1,1), .5f);
        }
    }

    public void ToggleAll() {
        Toggle(barracks);
        Toggle(logistics);
        Toggle(engineering);
        Toggle(misc);
    }

    private void ButtonClicked(Room room, UpgradeUITemplate template) {
        cameraManager.LookAtRoom(room, () => {upgradeUI.LoadTemplate(template); upgradeUI.Toggle();});
        mainUI.Toggle();
        ToggleAll();
    }
}
