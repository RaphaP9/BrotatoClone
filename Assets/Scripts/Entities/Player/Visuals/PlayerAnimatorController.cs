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
    private const string TAKE_DAMAGE_ANIMATION_NAME = "TakeDamage";
    private const string DEATH_ANIMATION_NAME = "Death";

    private void OnEnable()
    {
        PlayerDash.OnPlayerDash += PlayerDash_OnPlayerDash;
        PlayerDash.OnPlayerDashStopped += PlayerDash_OnPlayerDashStopped;
        //PlayerHealth.OnPlayerTakeDamage += PlayerHealth_OnPlayerTakeDamage;
        //PlayerHealth.OnPlayerDeath += PlayerHealth_OnPlayerDeath;
    }

    private void OnDisable()
    {
        PlayerDash.OnPlayerDash -= PlayerDash_OnPlayerDash;
        PlayerDash.OnPlayerDashStopped -= PlayerDash_OnPlayerDashStopped;
        //PlayerHealth.OnPlayerTakeDamage -= PlayerHealth_OnPlayerTakeDamage;
        //PlayerHealth.OnPlayerDeath -= PlayerHealth_OnPlayerDeath;
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

    private void PlayerDash_OnPlayerDash(object sender, PlayerDash.OnPlayerDashEventArgs e)
    {
        ResetTriggers();
        animator.Play(DASH_ANIMATION_NAME);
    }

    private void PlayerDash_OnPlayerDashStopped(object sender, PlayerDash.OnPlayerDashEventArgs e)
    {
        ResetTriggers();
        animator.Play(MOVEMENT_BLEND_TREE_NAME);
    }

}
