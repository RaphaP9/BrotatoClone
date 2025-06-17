using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonesGemelosDeTotoraAnimationController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private CanonesGemelosDeTotoraHandler canonesGemelosDeTotoraHandler;

    private const string SHOOT_ANIMATION_NAME = "Shoot";

    private void OnEnable()
    {
        canonesGemelosDeTotoraHandler.OnCanonesGemelosDeTotoraAttack += CanonesGemelosDeTotoraHandler_OnCanonesGemelosDeTotoraAttack;
    }

    private void OnDisable()
    {
        canonesGemelosDeTotoraHandler.OnCanonesGemelosDeTotoraAttack -= CanonesGemelosDeTotoraHandler_OnCanonesGemelosDeTotoraAttack;
    }

    private void Update()
    {
        HandleAnimatorSpeed();
    }

    private void HandleAnimatorSpeed()
    {
        animator.speed = canonesGemelosDeTotoraHandler.GetAttackSpeedRatioToBaseSpeed();
    }

    private void CanonesGemelosDeTotoraHandler_OnCanonesGemelosDeTotoraAttack(object sender, System.EventArgs e)
    {
        animator.Play(SHOOT_ANIMATION_NAME);
    }
}
