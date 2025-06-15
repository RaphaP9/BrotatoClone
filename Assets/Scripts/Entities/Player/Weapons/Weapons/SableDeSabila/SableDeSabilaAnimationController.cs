using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SableDeSabilaAnimationController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private SableDeSabilaHandler sableDeSabilaHandler;

    private const string ATTACK_ANIMATION_NAME = "Attack";

    private void OnEnable()
    {
        sableDeSabilaHandler.OnSableDeSabilaAttack += SableDeSabilaHandler_OnSableDeSabilaAttack;
    }

    private void OnDisable()
    {
        sableDeSabilaHandler.OnSableDeSabilaAttack -= SableDeSabilaHandler_OnSableDeSabilaAttack;
    }

    private void SableDeSabilaHandler_OnSableDeSabilaAttack(object sender, System.EventArgs e)
    {
        animator.Play(ATTACK_ANIMATION_NAME);
    }
}
