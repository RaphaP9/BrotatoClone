using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRangeStatManager : StatManager
{
    public static AttackRangeStatManager Instance { get; private set; }

    public static event EventHandler<OnAttackRangeStatEventArgs> OnAttackRangeStatInitialized;
    public static event EventHandler<OnAttackRangeStatEventArgs> OnAttackRangeStatUpdated;

    [Header("Value")]
    [SerializeField] private float attackRangeStat;

    public class OnAttackRangeStatEventArgs : EventArgs
    {
        public float attackRangeStat;
    }

    protected override void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one AttackRangeStatManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    protected override void InitializeStat()
    {
        ProcessAttackRangeStat();
        OnAttackRangeStatInitialized?.Invoke(this, new OnAttackRangeStatEventArgs { attackRangeStat = attackRangeStat });
    }

    protected override void UpdateStat()
    {
        ProcessAttackRangeStat();
        OnAttackRangeStatUpdated?.Invoke(this, new OnAttackRangeStatEventArgs { attackRangeStat = attackRangeStat });
    }

    private void ProcessAttackRangeStat()
    {
        float calculatedStatValue = CalculateStatValue(BaseStats.attackRange, GeneralStatsUtilities.MIN_ATTACK_RANGE, GeneralStatsUtilities.MAX_ATTACK_RANGE);
        float roundedValue = GeneralStatsUtilities.RoundFloatStatToNDecimalPlaces(calculatedStatValue, 2);
        SetAttackRangeStat(roundedValue);
    }

    private void SetAttackRangeStat(float value) => attackRangeStat = value;
}

