using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SopladorDeSerpienteAnimationController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private SopladorDeSerpienteHandler sopladorDeSerpienteHandler;

    private const string SHOOT_ANIMATION_NAME = "Shoot";

    private void OnEnable()
    {
        sopladorDeSerpienteHandler.OnSopladorDeSerpienteAttack += SopladorDeSerpienteHandler_OnSopladorDeSerpienteAttack;
    }

    private void OnDisable()
    {
        sopladorDeSerpienteHandler.OnSopladorDeSerpienteAttack -= SopladorDeSerpienteHandler_OnSopladorDeSerpienteAttack;
    }

    private void SopladorDeSerpienteHandler_OnSopladorDeSerpienteAttack(object sender, System.EventArgs e)
    {
        animator.Play(SHOOT_ANIMATION_NAME);
    }
}
