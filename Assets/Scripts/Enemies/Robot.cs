using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Robot : Enemy
{

    private Material glowMaterial;

    [SerializeField] Color peepee;
    [SerializeField] Color poopoo;

    override protected void Start() {
        base.Start();
        glowMaterial = gameObject.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().materials[1];
    }

    override public bool TakeDamage(float damage) {
        BlinkGlow();
        return base.TakeDamage(damage);
    }

    private void BlinkGlow() {
        glowMaterial.DOColor(peepee, "_EmissionColor", 0.2f).OnComplete(() => {
            glowMaterial.DOColor(poopoo, "_EmissionColor", 0.2f);
        });
    }
}
