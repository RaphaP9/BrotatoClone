using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtoAnimationController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private EnemyMovement enemyMovement;
    [SerializeField] private EnemyHealth enemyHealth;
    [SerializeField] private EnemySpawningHandler enemySpawningHandler;

    private const string SPAWN_TRIGGER = "Spawn";
    private const string DEATH_TRIGGER = "Death";

    private const string SPEED_FLOAT = "Speed";

    private const string MOVEMENT_BLEND_TREE_NAME = "MovementBlendTree";
    private const string SPAWN_ANIMATION_NAME = "Spawn";
    private const string DEATH_ANIMATION_NAME = "Death";

    private const float BLEED_DAMAGE_ANIMATION_DURATION = 0.25f;

    private void OnEnable()
    {
        enemySpawningHandler.OnThisEnemySpawnStart += EnemySpawningHandler_OnThisEnemySpawnStart;
        enemySpawningHandler.OnThisEnemySpawnComplete += EnemySpawningHandler_OnThisEnemySpawnComplete;
        enemyHealth.OnThisEnemyDeath += EnemyHealth_OnThisEnemyDeath;
    }

    private void Update()
    {
        HandleSpeedBlend();
    }

    private void HandleSpeedBlend()
    {
        animator.SetFloat(SPEED_FLOAT, enemyMovement.GetSpeed());
    }

    private void ResetTriggers()
    {
        animator.ResetTrigger(SPAWN_TRIGGER);
        animator.ResetTrigger(DEATH_TRIGGER);
    }

    #region Subscriptions
    private void EnemySpawningHandler_OnThisEnemySpawnStart(object sender, EnemySpawningHandler.OnEnemySpawnEventArgs e)
    {
        ResetTriggers();
        animator.Play(SPAWN_ANIMATION_NAME);
    }

    private void EnemySpawningHandler_OnThisEnemySpawnComplete(object sender, EnemySpawningHandler.OnEnemySpawnEventArgs e)
    {
        ResetTriggers();
        animator.Play(MOVEMENT_BLEND_TREE_NAME);
    }

    private void EnemyHealth_OnThisEnemyDeath(object sender, System.EventArgs e)
    {
        ResetTriggers();
        animator.Play(DEATH_ANIMATION_NAME);
    }
    #endregion


}
