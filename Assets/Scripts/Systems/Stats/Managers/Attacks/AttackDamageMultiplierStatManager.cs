using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDamageMultiplierStatManager : StatManager
{
    public static AttackDamageMultiplierStatManager Instance { get; private set; }

    public static event EventHandler<OnAttackDamageMultiplierStatEventArgs> OnAttackDamageMultiplierStatInitialized;
    public static event EventHandler<OnAttackDamageMultiplierStatEventArgs> OnAttackDamageMultiplierStatUpdated;

    [Header("Value")]
    [SerializeField] private float attackDamageMultiplierStat;

    public class OnAttackDamageMultiplierStatEventArgs : EventArgs
    {
        public float attackDamageMultiplierStat;
    }

    protected override void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one AttackDamageMultiplierStatManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    protected override void InitializeStat()
    {
        ProcessAttackDamageMultiplierStat();
        OnAttackDamageMultiplierStatInitialized?.Invoke(this, new OnAttackDamageMultiplierStatEventArgs { attackDamageMultiplierStat = attackDamageMultiplierStat });
    }

    protected override void UpdateStat()
    {
        ProcessAttackDamageMultiplierStat();
        OnAttackDamageMultiplierStatUpdated?.Invoke(this, new OnAttackDamageMultiplierStatEventArgs { attackDamageMultiplierStat = attackDamageMultiplierStat });
    }

    private void ProcessAttackDamageMultiplierStat()
    {
        float calculatedStatValue = CalculateStatValue(BaseStats.attackDamageMultiplier, GeneralStatsUtilities.MIN_ATTACK_DAMAGE_MULTIPLIER, GeneralStatsUtilities.MAX_ATTACK_DAMAGE_MULTIPLIER);
        float roundedValue = GeneralStatsUtilities.RoundFloatStatToNDecimalPlaces(calculatedStatValue, 2);
        SetAttackDamageMultiplierStat(roundedValue);
    }

    private void SetAttackDamageMultiplierStat(float value) => attackDamageMultiplierStat = value;
}
