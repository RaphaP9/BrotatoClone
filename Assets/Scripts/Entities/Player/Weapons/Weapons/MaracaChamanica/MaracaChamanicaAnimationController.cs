using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaracaChamanicaAnimationController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private MaracaChamanicaHandler maracaChamanicaHandler;

    private const string ATTACK_ANIMATION_NAME = "Attack";

    private void OnEnable()
    {
        maracaChamanicaHandler.OnMaracaChamanicaAttack += MaracaChamanicaHandler_OnMaracaChamanicaAttack;
    }

    private void OnDisable()
    {
        maracaChamanicaHandler.OnMaracaChamanicaAttack -= MaracaChamanicaHandler_OnMaracaChamanicaAttack;
    }

    private void Update()
    {
        HandleAnimatorSpeed();
    }

    private void HandleAnimatorSpeed()
    {
        animator.speed = maracaChamanicaHandler.GetAttackSpeedRatioToBaseSpeed();
    }

    private void MaracaChamanicaHandler_OnMaracaChamanicaAttack(object sender, System.EventArgs e)
    {
        animator.Play(ATTACK_ANIMATION_NAME);
    }
}
