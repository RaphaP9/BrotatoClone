using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CerbatanaDeCazadorAnimationController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private CerbatanaDeCazadorHandler cerbatanaDeCazadorHandler;

    private const string SHOOT_ANIMATION_NAME = "Shoot";

    private void OnEnable()
    {
        cerbatanaDeCazadorHandler.OnCerbatanaDeCazadorAttack += CerbatanaDeCazadorHandler_OnCerbatanaDeCazadorAttack;
    }

    private void OnDisable()
    {
        cerbatanaDeCazadorHandler.OnCerbatanaDeCazadorAttack -= CerbatanaDeCazadorHandler_OnCerbatanaDeCazadorAttack;
    }

    private void CerbatanaDeCazadorHandler_OnCerbatanaDeCazadorAttack(object sender, System.EventArgs e)
    {
        animator.Play(SHOOT_ANIMATION_NAME);
    }
}
