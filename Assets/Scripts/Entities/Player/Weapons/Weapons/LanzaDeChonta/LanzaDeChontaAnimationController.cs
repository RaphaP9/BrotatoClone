using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanzaDeChontaAnimationController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private LanzaDeChontaHandler lanzaDeChontaHandler;

    private const string ATTACK_ANIMATION_NAME = "Attack";

    private void OnEnable()
    {
        lanzaDeChontaHandler.OnLanzaDeChontaAttack += LanzaDeChontaHandler_OnLanzaDeChontaAttack;
    }

    private void OnDisable()
    {
        lanzaDeChontaHandler.OnLanzaDeChontaAttack -= LanzaDeChontaHandler_OnLanzaDeChontaAttack;
    }

    private void LanzaDeChontaHandler_OnLanzaDeChontaAttack(object sender, System.EventArgs e)
    {
        animator.Play(ATTACK_ANIMATION_NAME);
    }
}
