using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UpgradeUI : MonoBehaviour
{
    [SerializeField] private WorldSpaceUIManager worldSpaceUIManager;
    [SerializeField] private MainUI mainUI;
    [SerializeField] private CameraManager cameraManager;
    [SerializeField] private Button upgrade1, upgrade2, backButton;
    [SerializeField] private GameObject roomTitle;

    [SerializeField] private UpgradeUITemplate barracksTemplate;
    [SerializeField] private UpgradeUITemplate logisticsTemplate;
    [SerializeField] private UpgradeUITemplate engineeringTemplate;
    [SerializeField] private UpgradeUITemplate miscTemplate;

    private UpgradeUITemplate currentTemplate;
    private Upgrade[] upgradeIDs;

    private bool panelEnabled = true;

    public UpgradeUITemplate BarracksTemplate { get => barracksTemplate;}
    public UpgradeUITemplate LogisticsTemplate { get => logisticsTemplate;}
    public UpgradeUITemplate EngineeringTemplate { get => engineeringTemplate;}
    public UpgradeUITemplate MiscTemplate { get => miscTemplate;}

    public static event Action<Upgrade> UpgradeClicked;

    void Start() {
        Toggle();
        backButton.onClick.AddListener(BackClicked);
        upgrade1.onClick.AddListener(Upgrade1Clicked);
        upgrade2.onClick.AddListener(Upgrade2Clicked);
    }
    
    public void Toggle() {
        if (panelEnabled) {
            panelEnabled = false;
            backButton.transform.DOScale(new Vector3(0,0,0), 0.5f);
            upgrade1.transform.DOScale(new Vector3(0,0,0), 0.5f);
            upgrade2.transform.DOScale(new Vector3(0,0,0), 0.5f);
            roomTitle.transform.DOScale(new Vector3(0,0,0), 0.5f);
        } else {
            panelEnabled = true;
            backButton.transform.DOScale(new Vector3(1,1,1), 0.5f);
            upgrade1.transform.DOScale(new Vector3(1,1,1), 0.5f);
            upgrade2.transform.DOScale(new Vector3(1,1,1), 0.5f);
            roomTitle.transform.DOScale(new Vector3(1,1,1), 0.5f);
            CheckUpgradeAvailability();
        }
    }

    private void BackClicked() {
        Toggle();
        cameraManager.LookAtBuilding(() => {
            worldSpaceUIManager.ToggleAll();
            mainUI.Toggle();
        });
    }

    private void Upgrade1Clicked() {
        UpgradeClicked?.Invoke(upgradeIDs[0]);
        CheckUpgradeAvailability();
    }

    private void Upgrade2Clicked() {
        UpgradeClicked?.Invoke(upgradeIDs[1]);
        CheckUpgradeAvailability();
    }

    private void CheckUpgradeAvailability() {
        if (GameManager.upgradesData[upgradeIDs[0]] > GameManager.playerCurrency) {
            upgrade1.enabled = false;
        } else {
            upgrade1.enabled = true;
        }
        if (GameManager.upgradesData[upgradeIDs[1]] > GameManager.playerCurrency) {
            upgrade2.enabled = false;
        } else {
            upgrade2.enabled = true;
        }
    }

    public void LoadTemplate(UpgradeUITemplate template) {
        currentTemplate = template;

        roomTitle.transform.GetChild(0).GetComponent<Image>().sprite = template.roomSprite;
        roomTitle.transform.GetChild(1).GetComponent<TMP_Text>().text = template.roomName;

        upgrade1.transform.GetChild(1).GetComponent<TMP_Text>().text = template.upgrade1Name;
        upgrade1.transform.GetChild(2).GetComponent<Image>().sprite = template.upgrade1Sprite;

        upgrade2.transform.GetChild(1).GetComponent<TMP_Text>().text = template.upgrade2Name;
        upgrade2.transform.GetChild(2).GetComponent<Image>().sprite = template.upgrade2Sprite;

        upgradeIDs = template.upgradeIDs;
    }
}
