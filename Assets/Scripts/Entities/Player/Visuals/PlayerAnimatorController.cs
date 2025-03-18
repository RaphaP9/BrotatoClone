using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerMovement playerMovement;

    private const string DASH_TRIGGER = "Dash";
    private const string DEATH_TRIGGER = "Death";

    private const string SPEED_FLOAT = "Speed";

    private const string MOVEMENT_BLEND_TREE_NAME = "MovementBlendTree";
    private const string DASH_ANIMATION_NAME = "Dash";
    private const string TAKE_REGULAR_DAMAGE_ANIMATION_NAME = "TakeDamage";
    private const string TAKE_BLEED_DAMAGE_ANIMATION_NAME = "TakeDamage";
    private const string DEATH_ANIMATION_NAME = "Death";
    private const string HEAL_ANIMATION_NAME = "Heal";

    private const float REGULAR_DAMAGE_ANIMATION_DURATION = 0.35f;
    private const float BLEED_DAMAGE_ANIMATION_DURATION = 0.15f;
    private const float HEAL_ANIMATION_DURATION = 0.25f;

    private void OnEnable()
    {
        PlayerDash.OnPlayerDash += PlayerDash_OnPlayerDash;
        PlayerDash.OnPlayerDashStopped += PlayerDash_OnPlayerDashStopped;

        PlayerHealth.OnPlayerTakeRegularDamage += PlayerHealth_OnPlayerTakeRegularDamage;
        PlayerHealth.OnPlayerTakeBleedDamage += PlayerHealth_OnPlayerTakeBleedDamage;
        PlayerHealth.OnPlayerDeath += PlayerHealth_OnPlayerDeath;
        PlayerHealth.OnPlayerHeal += PlayerHealth_OnPlayerHeal;
    }

    private void OnDisable()
    {
        PlayerDash.OnPlayerDash -= PlayerDash_OnPlayerDash;
        PlayerDash.OnPlayerDashStopped -= PlayerDash_OnPlayerDashStopped;

        PlayerHealth.OnPlayerTakeRegularDamage -= PlayerHealth_OnPlayerTakeRegularDamage;
        PlayerHealth.OnPlayerTakeBleedDamage -= PlayerHealth_OnPlayerTakeBleedDamage;
        PlayerHealth.OnPlayerDeath -= PlayerHealth_OnPlayerDeath;
        PlayerHealth.OnPlayerHeal -= PlayerHealth_OnPlayerHeal;
    }

    private void Update()
    {
        HandleSpeedBlend();
    }

    private void HandleSpeedBlend()
    {
        animator.SetFloat(SPEED_FLOAT, playerMovement.DesiredSpeed);
    }

    private void ResetTriggers()
    {
        animator.ResetTrigger(DASH_TRIGGER);
        animator.ResetTrigger(DEATH_TRIGGER);
    }

    private IEnumerator PlayAnimationForSeconds(string targetAnimation, string finalAnimation, float duration)
    {
        animator.Play(targetAnimation);
        yield return new WaitForSeconds(duration);
        animator.Play(finalAnimation);
    }

    #region Subscriptions
    private void PlayerDash_OnPlayerDash(object sender, PlayerDash.OnPlayerDashEventArgs e)
    {
        StopAllCoroutines();
        ResetTriggers();

        animator.Play(DASH_ANIMATION_NAME);
    }

    private void PlayerDash_OnPlayerDashStopped(object sender, PlayerDash.OnPlayerDashEventArgs e)
    {
        StopAllCoroutines();
        ResetTriggers();

        animator.Play(MOVEMENT_BLEND_TREE_NAME);
    }

    private void PlayerHealth_OnPlayerTakeRegularDamage(object sender, EntityHealth.OnEntityTakeDamageEventArgs e)
    {
        StopAllCoroutines();
        ResetTriggers();

        StartCoroutine(PlayAnimationForSeconds(TAKE_REGULAR_DAMAGE_ANIMATION_NAME,MOVEMENT_BLEND_TREE_NAME,REGULAR_DAMAGE_ANIMATION_DURATION));
    }
    private void PlayerHealth_OnPlayerTakeBleedDamage(object sender, EntityHealth.OnEntityTakeDamageEventArgs e)
    {
        StopAllCoroutines();
        ResetTriggers();

        StartCoroutine(PlayAnimationForSeconds(TAKE_BLEED_DAMAGE_ANIMATION_NAME, MOVEMENT_BLEND_TREE_NAME, BLEED_DAMAGE_ANIMATION_DURATION));
    }

    private void PlayerHealth_OnPlayerDeath(object sender, EventArgs e)
    {
        StopAllCoroutines();
        ResetTriggers();

        animator.Play(DEATH_ANIMATION_NAME);
    }

    private void PlayerHealth_OnPlayerHeal(object sender, EntityHealth.OnEntityHealEventArgs e)
    {
        StopAllCoroutines();
        ResetTriggers();

        StartCoroutine(PlayAnimationForSeconds(HEAL_ANIMATION_NAME, MOVEMENT_BLEND_TREE_NAME, HEAL_ANIMATION_DURATION));
    }
    #endregion
}
