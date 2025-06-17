using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EspinasDeAyahuascaAnimationController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private EspinasDeAyahuascaHandler espinasDeAyahuascaHandler;

    private const string SHOOT_ANIMATION_NAME = "Shoot";

    private void OnEnable()
    {
        espinasDeAyahuascaHandler.OnEspinasDeAyahuascaAttack += EspinasDeAyahuascaHandler_OnEspinasDeAyahuascaAttack;
    }

    private void OnDisable()
    {
        espinasDeAyahuascaHandler.OnEspinasDeAyahuascaAttack -= EspinasDeAyahuascaHandler_OnEspinasDeAyahuascaAttack;
    }

    private void Update()
    {
        HandleAnimatorSpeed();
    }

    private void HandleAnimatorSpeed()
    {
        animator.speed = espinasDeAyahuascaHandler.GetAttackSpeedRatioToBaseSpeed();
    }

    private void EspinasDeAyahuascaHandler_OnEspinasDeAyahuascaAttack(object sender, System.EventArgs e)
    {
        animator.Play(SHOOT_ANIMATION_NAME);
    }
}
