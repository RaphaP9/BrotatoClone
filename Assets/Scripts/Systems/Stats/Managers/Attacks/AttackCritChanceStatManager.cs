using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCritChanceStatManager : StatManager
{
    public static AttackCritChanceStatManager Instance { get; private set; }

    public static event EventHandler<OnAttackCritChanceStatEventArgs> OnAttackCritChanceStatInitialized;
    public static event EventHandler<OnAttackCritChanceStatEventArgs> OnAttackCritChancerStatUpdated;

    [Header("Value")]
    [SerializeField] private float attackCritChanceStat;

    public class OnAttackCritChanceStatEventArgs : EventArgs
    {
        public float attackCritChanceStat;
    }

    protected override void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one AttackCritChanceStatManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    protected override void InitializeStat()
    {
        ProcessAttackCritChanceStat();
        OnAttackCritChanceStatInitialized?.Invoke(this, new OnAttackCritChanceStatEventArgs { attackCritChanceStat = attackCritChanceStat });
    }

    protected override void UpdateStat()
    {
        ProcessAttackCritChanceStat();
        OnAttackCritChancerStatUpdated?.Invoke(this, new OnAttackCritChanceStatEventArgs { attackCritChanceStat = attackCritChanceStat });
    }

    private void ProcessAttackCritChanceStat()
    {
        float calculatedStatValue = CalculateStatValue(BaseStats.attackCritChance, GeneralStatsUtilities.MIN_ATTACK_CRIT_CHANCE, GeneralStatsUtilities.MAX_ATTACK_CRIT_CHANCE);
        float roundedValue = GeneralStatsUtilities.RoundFloatStatToNDecimalPlaces(calculatedStatValue, 2);
        SetAttackCritChanceMultiplierStat(roundedValue);
    }

    private void SetAttackCritChanceMultiplierStat(float value) => attackCritChanceStat = value;
}
