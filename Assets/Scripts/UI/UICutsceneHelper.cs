using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UICutsceneHelper : MonoBehaviour
{
    [SerializeField] private Image panelImage;

    public void FadeToBlack(float duration) {
        panelImage.DOFade(1, duration).OnComplete(() => panelImage.DOFade(0, duration).SetDelay(0.5f));
    }
}
