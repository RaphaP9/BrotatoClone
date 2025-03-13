using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRangeMultiplierStatManager : StatManager
{
    public static AttackRangeMultiplierStatManager Instance { get; private set; }

    public static event EventHandler<OnAttackRangeMultiplierStatEventArgs> OnAttackRangeMultiplierStatInitialized;
    public static event EventHandler<OnAttackRangeMultiplierStatEventArgs> OnAttackRangeMultiplierStatUpdated;

    [Header("Value")]
    [SerializeField] private float attackRangeMultiplierStat;

    public class OnAttackRangeMultiplierStatEventArgs : EventArgs
    {
        public float attackRangeMultiplierStat;
    }

    protected override void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one AttackRangeMultiplierStatManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    protected override void InitializeStat()
    {
        ProcessAttackRangeMultiplierStat();
        OnAttackRangeMultiplierStatInitialized?.Invoke(this, new OnAttackRangeMultiplierStatEventArgs { attackRangeMultiplierStat = attackRangeMultiplierStat });
    }

    protected override void UpdateStat()
    {
        ProcessAttackRangeMultiplierStat();
        OnAttackRangeMultiplierStatUpdated?.Invoke(this, new OnAttackRangeMultiplierStatEventArgs { attackRangeMultiplierStat = attackRangeMultiplierStat });
    }

    private void ProcessAttackRangeMultiplierStat()
    {
        float calculatedStatValue = CalculateStatValue(BaseStats.attackRangeMultiplier, GeneralStatsUtilities.MIN_ATTACK_RANGE_MULTIPLIER, GeneralStatsUtilities.MAX_ATTACK_RANGE_MULTIPLIER);
        float roundedValue = GeneralStatsUtilities.RoundFloatStatToNDecimalPlaces(calculatedStatValue, 2);
        SetAttackRangeMultiplierStat(roundedValue);
    }

    private void SetAttackRangeMultiplierStat(float value) => attackRangeMultiplierStat = value;
}

