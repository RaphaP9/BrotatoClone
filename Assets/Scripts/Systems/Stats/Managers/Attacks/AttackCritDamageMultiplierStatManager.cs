using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCritDamageMultiplierStatManager : StatManager
{
    public static AttackCritDamageMultiplierStatManager Instance { get; private set; }

    public static event EventHandler<OnAttackCritDamageMultiplierStatEventArgs> OnAttackCritDamageMultiplierStatInitialized;
    public static event EventHandler<OnAttackCritDamageMultiplierStatEventArgs> OnAttackCritDamageMultiplierStatUpdated;

    [Header("Value")]
    [SerializeField] private float attackCritDamageMultiplierStat;

    public class OnAttackCritDamageMultiplierStatEventArgs : EventArgs
    {
        public float attackCritDamageMultiplierStat;
    }

    protected override void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one AttackCritDamageMultiplierStatManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    protected override void InitializeStat()
    {
        ProcessAttackCritDamageMultiplierStat();
        OnAttackCritDamageMultiplierStatInitialized?.Invoke(this, new OnAttackCritDamageMultiplierStatEventArgs { attackCritDamageMultiplierStat = attackCritDamageMultiplierStat });
    }

    protected override void UpdateStat()
    {
        ProcessAttackCritDamageMultiplierStat();
        OnAttackCritDamageMultiplierStatUpdated?.Invoke(this, new OnAttackCritDamageMultiplierStatEventArgs { attackCritDamageMultiplierStat = attackCritDamageMultiplierStat });
    }

    private void ProcessAttackCritDamageMultiplierStat()
    {
        float calculatedStatValue = CalculateStatValue(baseStats.attackCritDamageMultiplier, GeneralStatsUtilities.MIN_ATTACK_CRIT_DAMAGE_MULTIPLIER, GeneralStatsUtilities.MAX_ATTACK_CRIT_DAMAGE_MULTIPLIER);
        float roundedValue = GeneralStatsUtilities.RoundFloatStatToNDecimalPlaces(calculatedStatValue, 2);
        SetAttackCritDamageMultiplierStat(roundedValue);
    }

    private void SetAttackCritDamageMultiplierStat(float value) => attackCritDamageMultiplierStat = value;
}
