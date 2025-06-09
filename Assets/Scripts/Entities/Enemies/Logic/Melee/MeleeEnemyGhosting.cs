using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyGhosting : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private EnemyHealth enemyHealth;
    [SerializeField] private EnemySpawningHandler enemySpawningHandler;
    [SerializeField] private EnemyAttack enemyAttack;

    [Header("Settings")]
    [SerializeField, Range(0f,5f)] private float timeToGhostAfterSpawning;
    [SerializeField, Range(0f, 5f)] private float timeToGhostAfterTakingDamage;
    [SerializeField, Range(0f, 5f)] private float timeToGhostAfterAttacking;

    [Header("States - Runtime Filled")]
    [SerializeField] private GhostingState ghostingState;

    private enum GhostingState {NotGhosted, Ghosted}

    public static event EventHandler OnEnemyGhosting;
    public event EventHandler OnThisEnemyGhosting;

    public static event EventHandler OnEnemyGhostingInterrupted;
    public event EventHandler OnThisEnemyGhostingInterrupted;

    private void OnEnable()
    {
        enemySpawningHandler.OnThisEnemySpawnComplete += EnemySpawningHandler_OnThisEnemySpawnComplete;
        enemyAttack.OnThisEnemyCharge += EnemyAttack_OnThisEnemyCharge;
        enemyAttack.OnThisEnemyStopAttacking += EnemyAttack_OnThisEnemyStopAttacking;

        enemyHealth.OnThisEnemyTakeRegularDamage += EnemyHealth_OnThisEnemyTakeRegularDamage;
        enemyHealth.OnThisEnemyTakeBleedDamage += EnemyHealth_OnThisEnemyTakeBleedDamage;

        enemyHealth.OnThisEnemyDeath += EnemyHealth_OnThisEnemyDeath;
    }

    private void OnDisable()
    {
        enemySpawningHandler.OnThisEnemySpawnComplete -= EnemySpawningHandler_OnThisEnemySpawnComplete;
        enemyAttack.OnThisEnemyCharge -= EnemyAttack_OnThisEnemyCharge;
        enemyAttack.OnThisEnemyStopAttacking -= EnemyAttack_OnThisEnemyStopAttacking;

        enemyHealth.OnThisEnemyTakeRegularDamage -= EnemyHealth_OnThisEnemyTakeRegularDamage;
        enemyHealth.OnThisEnemyTakeBleedDamage -= EnemyHealth_OnThisEnemyTakeBleedDamage;

        enemyHealth.OnThisEnemyDeath -= EnemyHealth_OnThisEnemyDeath;
    }

    private void Start()
    {
        SetGhostingState(GhostingState.NotGhosted);
    }

    private IEnumerator GhostEnemyAfterTimeCoroutine(float time)
    {
        if (ghostingState == GhostingState.Ghosted) yield break;

        yield return new WaitForSeconds(time);
        GhostEnemy();
    }

    private void GhostEnemy()
    {
        if (ghostingState == GhostingState.Ghosted) return;

        SetGhostingState(GhostingState.Ghosted);
        enemyHealth.SetIsGhosted(true);

        OnEnemyGhosting?.Invoke(this, EventArgs.Empty);
        OnThisEnemyGhosting?.Invoke(this, EventArgs.Empty);
    }

    private void InterruptEnemyGhost()
    {
        if (ghostingState != GhostingState.Ghosted) return;

        SetGhostingState(GhostingState.NotGhosted);
        enemyHealth.SetIsGhosted(false);

        OnEnemyGhostingInterrupted?.Invoke(this, EventArgs.Empty);
        OnThisEnemyGhostingInterrupted?.Invoke(this, EventArgs.Empty);
    }

    private void SetGhostingState(GhostingState ghostingState) => this.ghostingState = ghostingState;

    #region Susbcriptions
    private void EnemySpawningHandler_OnThisEnemySpawnComplete(object sender, EnemySpawningHandler.OnEnemySpawnEventArgs e)
    {
        StopAllCoroutines();
        StartCoroutine(GhostEnemyAfterTimeCoroutine(timeToGhostAfterSpawning));
    }

    private void EnemyAttack_OnThisEnemyCharge(object sender, EnemyAttack.OnEnemyAttackEventArgs e)
    {
        StopAllCoroutines();
        InterruptEnemyGhost();       
    }

    private void EnemyAttack_OnThisEnemyStopAttacking(object sender, EnemyAttack.OnEnemyAttackEventArgs e)
    {
        StopAllCoroutines();
        StartCoroutine(GhostEnemyAfterTimeCoroutine(timeToGhostAfterAttacking));
    }

    private void EnemyHealth_OnThisEnemyTakeBleedDamage(object sender, EnemyHealth.OnEnemyTakeDamageEventArgs e)
    {
        StopAllCoroutines();
        InterruptEnemyGhost();
        StartCoroutine(GhostEnemyAfterTimeCoroutine(timeToGhostAfterTakingDamage));
    }

    private void EnemyHealth_OnThisEnemyTakeRegularDamage(object sender, EnemyHealth.OnEnemyTakeDamageEventArgs e)
    {
        StopAllCoroutines();
        InterruptEnemyGhost();
        StartCoroutine(GhostEnemyAfterTimeCoroutine(timeToGhostAfterTakingDamage));
    }

    private void EnemyHealth_OnThisEnemyDeath(object sender, EnemyHealth.OnEnemyDeathEventArgs e)
    {
        StopAllCoroutines();
        InterruptEnemyGhost();
    }
    #endregion
}
