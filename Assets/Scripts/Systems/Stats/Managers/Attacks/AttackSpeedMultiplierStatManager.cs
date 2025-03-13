using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSpeedMultiplierStatManager : StatManager
{
    public static AttackSpeedMultiplierStatManager Instance { get; private set; }

    public static event EventHandler<OnAttackSpeedMultiplierStatEventArgs> OnAttackSpeedMultiplierStatInitialized;
    public static event EventHandler<OnAttackSpeedMultiplierStatEventArgs> OnAttackSpeedMultiplierStatUpdated;

    [Header("Value")]
    [SerializeField] private float attackSpeedMultiplierStat;

    public class OnAttackSpeedMultiplierStatEventArgs : EventArgs
    {
        public float attackSpeedMultiplierStat;
    }

    protected override void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one AttackSpeedMultiplierStatManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    protected override void InitializeStat()
    {
        ProcessAttackSpeedMultiplierStat();
        OnAttackSpeedMultiplierStatInitialized?.Invoke(this, new OnAttackSpeedMultiplierStatEventArgs { attackSpeedMultiplierStat = attackSpeedMultiplierStat });
    }

    protected override void UpdateStat()
    {
        ProcessAttackSpeedMultiplierStat();
        OnAttackSpeedMultiplierStatUpdated?.Invoke(this, new OnAttackSpeedMultiplierStatEventArgs { attackSpeedMultiplierStat = attackSpeedMultiplierStat });
    }

    private void ProcessAttackSpeedMultiplierStat()
    {
        float calculatedStatValue = CalculateStatValue(BaseStats.attackSpeedMultiplier, GeneralStatsUtilities.MIN_ATTACK_SPEED_MULTIPLIER, GeneralStatsUtilities.MAX_ATTACK_SPEED_MULTIPLIER);
        float roundedValue = GeneralStatsUtilities.RoundFloatStatToNDecimalPlaces(calculatedStatValue, 2);
        SetAttackSpeedMultiplierStat(roundedValue);
    }

    private void SetAttackSpeedMultiplierStat(float value) => attackSpeedMultiplierStat = value;
}
