using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DagaDeCaobaAnimationController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private DagaDeCaobaHandler dagaDeCaobaHandler;

    private const string ATTACK_ANIMATION_NAME = "Attack";

    private void OnEnable()
    {
        dagaDeCaobaHandler.OnDagaDeCaobaAttack += DagaDeCaobaHandler_OnDagaDeCaobaAttack;
    }

    private void OnDisable()
    {
        dagaDeCaobaHandler.OnDagaDeCaobaAttack -= DagaDeCaobaHandler_OnDagaDeCaobaAttack;
    }

    private void Update()
    {
        HandleAnimatorSpeed();
    }

    private void HandleAnimatorSpeed()
    {
        animator.speed = dagaDeCaobaHandler.GetAttackSpeedRatioToBaseSpeed();
    }

    private void DagaDeCaobaHandler_OnDagaDeCaobaAttack(object sender, System.EventArgs e)
    {
        animator.Play(ATTACK_ANIMATION_NAME);
    }
}
