using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Robot : Enemy
{

    private Material glowMaterial;

    [SerializeField] Color peepee;
    [SerializeField] Color poopoo;

    [SerializeField] GameObject enemyModel;
    [SerializeField] DropPod dropPod;

    private Vector3 landPosition;
    private bool hasLanded = false;

    override protected void Start() {
        base.Start();
        dropPod.hasLanded += OnDropPodLanded;
        glowMaterial = transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().materials[1];
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        landPosition = transform.position;
        dropPod.Init(landPosition);
    }

    void OnDisable() {
        enemyModel.SetActive(false);
        dropPod.gameObject.SetActive(true);
        hasLanded = false;
        canBeTargeted = false;
    }

    protected override void Update()
    {
        if (hasLanded)
            base.Update();
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

    private void OnDropPodLanded() {
        hasLanded = true;
        canBeTargeted = true;
        enemyModel.SetActive(true);
    }
}
