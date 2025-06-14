using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeHealerEnemyAnimationController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;
    [Space]
    [SerializeField] private EnemyMovement enemyMovement;
    [SerializeField] private EnemySpawningHandler enemySpawningHandler;
    [SerializeField] private EnemyHealth enemyHealth;
    [SerializeField] private EnemyAttack enemyAttack;
    [SerializeField] private MeleeEnemyRegen meleeEnemyRegen;

    private const string SPEED_FLOAT = "Speed";

    private const string MOVEMENT_BLEND_TREE_NAME = "MovementBlendTree";

    private const string SPAWN_ANIMATION_NAME = "Spawn";
    private const string DEATH_ANIMATION_NAME = "Death";
    private const string CHARGE_ANIMATION_NAME = "Charge";
    private const string ATTACK_ANIMATION_NAME = "Attack";
    private const string POST_ATTACK_ANIMATION_NAME = "PostAttack";
    private const string REGEN_ANIMATION_NAME = "Regen";

    private bool hasDied = false;

    private void OnEnable()
    {
        enemySpawningHandler.OnThisEnemySpawnStart += EnemySpawningHandler_OnThisEnemySpawnStart;
        enemySpawningHandler.OnThisEnemySpawnComplete += EnemySpawningHandler_OnThisEnemySpawnComplete;

        enemyAttack.OnThisEnemyCharge += EnemyAttack_OnThisEnemyCharge;
        enemyAttack.OnThisEnemyAttack += EnemyAttack_OnThisEnemyAttack;
        enemyAttack.OnThisEnemyPostAttack += EnemyAttack_OnThisEnemyPostShoot;
        enemyAttack.OnThisEnemyStopAttacking += EnemyShoot_OnThisEnemyStopAttacking;

        enemyHealth.OnThisEnemyDeath += EnemyHealth_OnThisEnemyDeath;

        meleeEnemyRegen.OnThisEnemyRegenStart += MeleeEnemyRegen_OnThisEnemyRegenStart;
        meleeEnemyRegen.OnThisEnemyRenegInterruptedFullHealth += MeleeEnemyRegen_OnThisEnemyRenegInterruptedFullHealth;
    }

    private void OnDisable()
    {
        enemySpawningHandler.OnThisEnemySpawnStart -= EnemySpawningHandler_OnThisEnemySpawnStart;
        enemySpawningHandler.OnThisEnemySpawnComplete -= EnemySpawningHandler_OnThisEnemySpawnComplete;

        enemyAttack.OnThisEnemyCharge -= EnemyAttack_OnThisEnemyCharge;
        enemyAttack.OnThisEnemyAttack -= EnemyAttack_OnThisEnemyAttack;
        enemyAttack.OnThisEnemyPostAttack -= EnemyAttack_OnThisEnemyPostShoot;
        enemyAttack.OnThisEnemyStopAttacking -= EnemyShoot_OnThisEnemyStopAttacking;

        enemyHealth.OnThisEnemyDeath -= EnemyHealth_OnThisEnemyDeath;

        meleeEnemyRegen.OnThisEnemyRegenStart -= MeleeEnemyRegen_OnThisEnemyRegenStart;
        meleeEnemyRegen.OnThisEnemyRenegInterruptedFullHealth -= MeleeEnemyRegen_OnThisEnemyRenegInterruptedFullHealth;
    }

    private void Update()
    {
        HandleSpeedBlend();
    }

    private void HandleSpeedBlend()
    {
        animator.SetFloat(SPEED_FLOAT, enemyMovement.GetSpeed());
    }

    #region Subscriptions
    private void EnemySpawningHandler_OnThisEnemySpawnStart(object sender, EnemySpawningHandler.OnEnemySpawnEventArgs e)
    {
        animator.Play(SPAWN_ANIMATION_NAME);
    }

    private void EnemySpawningHandler_OnThisEnemySpawnComplete(object sender, EnemySpawningHandler.OnEnemySpawnEventArgs e)
    {
        if (hasDied) return;
        animator.Play(MOVEMENT_BLEND_TREE_NAME);
    }

    private void EnemyAttack_OnThisEnemyCharge(object sender, EnemyAttack.OnEnemyAttackEventArgs e)
    {
        if (hasDied) return;
        animator.Play(CHARGE_ANIMATION_NAME);
    }

    private void EnemyAttack_OnThisEnemyAttack(object sender, EnemyAttack.OnEnemyAttackEventArgs e)
    {
        if (hasDied) return;
        animator.Play(ATTACK_ANIMATION_NAME);
    }

    private void EnemyAttack_OnThisEnemyPostShoot(object sender, EnemyAttack.OnEnemyAttackEventArgs e)
    {
        if (hasDied) return;
        animator.Play(POST_ATTACK_ANIMATION_NAME);
    }

    private void EnemyShoot_OnThisEnemyStopAttacking(object sender, EnemyAttack.OnEnemyAttackEventArgs e)
    {
        if (hasDied) return;
        animator.Play(MOVEMENT_BLEND_TREE_NAME);
    }

    private void EnemyHealth_OnThisEnemyDeath(object sender, System.EventArgs e)
    {
        hasDied = true;
        animator.Play(DEATH_ANIMATION_NAME);
    }

    private void MeleeEnemyRegen_OnThisEnemyRegenStart(object sender, System.EventArgs e)
    {
        if(hasDied) return;
        animator.Play(REGEN_ANIMATION_NAME);
    }

    private void MeleeEnemyRegen_OnThisEnemyRenegInterruptedFullHealth(object sender, System.EventArgs e)
    {
        if (hasDied) return;
        animator.Play(MOVEMENT_BLEND_TREE_NAME);
    }
    #endregion
}
