using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TitleUI : MonoBehaviour
{
    [SerializeField] private WorldSpaceUIManager worldSpaceUI;
    [SerializeField] private MainUI mainUI;
    [SerializeField] private Button playButton;
    [SerializeField] private GameObject titleText, tapAnywhereText;

    private Tweener textTweener;

    void Start() {
        textTweener = tapAnywhereText.transform.DOScale(new Vector3(0.95f,0.95f,0.95f), 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
        playButton.onClick.AddListener(StartButtonClicked);
    }

    void Update() {

    }

    private void StartButtonClicked() {
        GameManager.SetPlayerMovement(true);
        textTweener.Kill();
        tapAnywhereText.transform.DOScale(Vector3.zero, 0.5f);
        titleText.transform.DOScale(Vector3.zero, 0.5f).OnComplete(() => gameObject.SetActive(false));
        worldSpaceUI.ToggleAll();
        mainUI.Toggle();
    }
}
