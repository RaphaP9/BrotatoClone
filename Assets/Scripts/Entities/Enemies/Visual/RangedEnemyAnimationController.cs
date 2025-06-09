using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyAnimationController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;
    [Space]
    [SerializeField] private EnemyMovement enemyMovement;
    [SerializeField] private EnemySpawningHandler enemySpawningHandler;
    [SerializeField] private EnemyHealth enemyHealth;
    [SerializeField] private EnemyShoot enemyShoot;

    private const string SPEED_FLOAT = "Speed";

    private const string MOVEMENT_BLEND_TREE_NAME = "MovementBlendTree";

    private const string SPAWN_ANIMATION_NAME = "Spawn";
    private const string DEATH_ANIMATION_NAME = "Death";
    private const string AIM_ANIMATION_NAME = "Aim";
    private const string SHOOT_ANIMATION_NAME = "Shoot";
    private const string POST_SHOOT_ANIMATION_NAME = "PostShoot";

    private bool hasDied = false;

    private void OnEnable()
    {
        enemySpawningHandler.OnThisEnemySpawnStart += EnemySpawningHandler_OnThisEnemySpawnStart;
        enemySpawningHandler.OnThisEnemySpawnComplete += EnemySpawningHandler_OnThisEnemySpawnComplete;

        enemyShoot.OnThisEnemyAim += EnemyShoot_OnThisEnemyAim;
        enemyShoot.OnThisEnemyShoot += EnemyShoot_OnThisEnemyShoot;
        enemyShoot.OnThisEnemyPostShoot += EnemyShoot_OnThisEnemyPostShoot;
        enemyShoot.OnThisEnemyStopShooting += EnemyShoot_OnThisEnemyStopShooting;

        enemyHealth.OnThisEnemyDeath += EnemyHealth_OnThisEnemyDeath;
    }

    private void OnDisable()
    {
        enemySpawningHandler.OnThisEnemySpawnStart -= EnemySpawningHandler_OnThisEnemySpawnStart;
        enemySpawningHandler.OnThisEnemySpawnComplete -= EnemySpawningHandler_OnThisEnemySpawnComplete;

        enemyShoot.OnThisEnemyAim -= EnemyShoot_OnThisEnemyAim;
        enemyShoot.OnThisEnemyShoot -= EnemyShoot_OnThisEnemyShoot;
        enemyShoot.OnThisEnemyPostShoot -= EnemyShoot_OnThisEnemyPostShoot;
        enemyShoot.OnThisEnemyStopShooting -= EnemyShoot_OnThisEnemyStopShooting;

        enemyHealth.OnThisEnemyDeath -= EnemyHealth_OnThisEnemyDeath;
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

    private void EnemyShoot_OnThisEnemyAim(object sender, EnemyShoot.OnEnemyShootEventArgs e)
    {
        if (hasDied) return;
        animator.Play(AIM_ANIMATION_NAME);
    }

    private void EnemyShoot_OnThisEnemyShoot(object sender, EnemyShoot.OnEnemyShootEventArgs e)
    {
        if (hasDied) return;
        animator.Play(SHOOT_ANIMATION_NAME);
    }

    private void EnemyShoot_OnThisEnemyPostShoot(object sender, EnemyShoot.OnEnemyShootEventArgs e)
    {
        if (hasDied) return;
        animator.Play(POST_SHOOT_ANIMATION_NAME);
    }

    private void EnemyShoot_OnThisEnemyStopShooting(object sender, EnemyShoot.OnEnemyShootEventArgs e)
    {
        if (hasDied) return;
        animator.Play(MOVEMENT_BLEND_TREE_NAME);
    }

    private void EnemyHealth_OnThisEnemyDeath(object sender, System.EventArgs e)
    {
        hasDied = true;
        animator.Play(DEATH_ANIMATION_NAME);
    }

    #endregion
}
