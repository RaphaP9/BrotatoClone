using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyRegen : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private EnemyHealth enemyHealth;
    [SerializeField] private EnemySpawningHandler enemySpawningHandler;
    [SerializeField] private EnemyAttack enemyAttack;

    [Header("Settings")]
    [SerializeField, Range(1, 5)] private int regeneration;
    [SerializeField, Range(0.5f, 5f)] private float regenInterval;
    [Space]
    [SerializeField, Range(0f, 5f)] private float timeToRegenerateAfterSpawning;
    [SerializeField, Range(0f, 5f)] private float timeToRegenerateAfterTakingDamage;
    [SerializeField, Range(0f, 5f)] private float timeToRegenerateAfterAttacking;

    [Header("States - Runtime Filled")]
    [SerializeField] private RegenState regenState;

    private enum RegenState { NotRegenerating, Regenerating }

    public static event EventHandler OnEnemyRegenStart;
    public event EventHandler OnThisEnemyRegenStart;

    public static event EventHandler OnEnemyRenegInterrupted;
    public event EventHandler OnThisEnemyRenegInterrupted;

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
        SetGhostingState(RegenState.NotRegenerating);
    }

    private IEnumerator RegenerateEnemyAfterTimeCoroutine(float time)
    {
        if (regenState == RegenState.Regenerating) yield break;

        SetGhostingState(RegenState.Regenerating);

        yield return new WaitForSeconds(time);

        OnThisEnemyRegenStart?.Invoke(this, EventArgs.Empty);
        OnEnemyRegenStart?.Invoke(this, EventArgs.Empty);

        while (true)
        {
            enemyHealth.Heal(regeneration);
            yield return new WaitForSeconds(regenInterval);
        }
    }

    private void InterruptEnemyHeal()
    {
        if (regenState != RegenState.Regenerating) return;

        SetGhostingState(RegenState.NotRegenerating);

        StopAllCoroutines();

        OnEnemyRenegInterrupted?.Invoke(this, EventArgs.Empty);
        OnThisEnemyRenegInterrupted?.Invoke(this, EventArgs.Empty);
    }

    private void SetGhostingState(RegenState ghostingState) => this.regenState = ghostingState;

    #region Susbcriptions
    private void EnemySpawningHandler_OnThisEnemySpawnComplete(object sender, EnemySpawningHandler.OnEnemySpawnEventArgs e)
    {
        StopAllCoroutines();
        StartCoroutine(RegenerateEnemyAfterTimeCoroutine(timeToRegenerateAfterSpawning));
    }

    private void EnemyAttack_OnThisEnemyCharge(object sender, EnemyAttack.OnEnemyAttackEventArgs e)
    {
        InterruptEnemyHeal();
    }

    private void EnemyAttack_OnThisEnemyStopAttacking(object sender, EnemyAttack.OnEnemyAttackEventArgs e)
    {
        StartCoroutine(RegenerateEnemyAfterTimeCoroutine(timeToRegenerateAfterAttacking));
    }

    private void EnemyHealth_OnThisEnemyTakeBleedDamage(object sender, EnemyHealth.OnEnemyTakeDamageEventArgs e)
    {
        InterruptEnemyHeal();
        StartCoroutine(RegenerateEnemyAfterTimeCoroutine(timeToRegenerateAfterTakingDamage));
    }

    private void EnemyHealth_OnThisEnemyTakeRegularDamage(object sender, EnemyHealth.OnEnemyTakeDamageEventArgs e)
    {
        InterruptEnemyHeal();
        StartCoroutine(RegenerateEnemyAfterTimeCoroutine(timeToRegenerateAfterTakingDamage));
    }

    private void EnemyHealth_OnThisEnemyDeath(object sender, EnemyHealth.OnEnemyDeathEventArgs e)
    {
        InterruptEnemyHeal();
    }
    #endregion
}
