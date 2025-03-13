using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSpeedStatManager : StatManager
{
    public static AttackSpeedStatManager Instance { get; private set; }

    public static event EventHandler<OnAttackSpeedStatEventArgs> OnAttackSpeedStatInitialized;
    public static event EventHandler<OnAttackSpeedStatEventArgs> OnAttackSpeedStatUpdated;

    [Header("Value")]
    [SerializeField] private float attackSpeedStat;

    public class OnAttackSpeedStatEventArgs : EventArgs
    {
        public float attackSpeedStat;
    }

    protected override void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one AttackSpeedStatManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    protected override void InitializeStat()
    {
        ProcessAttackSpeedStat();
        OnAttackSpeedStatInitialized?.Invoke(this, new OnAttackSpeedStatEventArgs { attackSpeedStat = attackSpeedStat });
    }

    protected override void UpdateStat()
    {
        ProcessAttackSpeedStat();
        OnAttackSpeedStatUpdated?.Invoke(this, new OnAttackSpeedStatEventArgs { attackSpeedStat = attackSpeedStat });
    }

    private void ProcessAttackSpeedStat()
    {
        float calculatedStatValue = CalculateStatValue(baseStats.attackSpeed, GeneralStatsUtilities.MIN_ATTACK_SPEED, GeneralStatsUtilities.MAX_ATTACK_SPEED);
        float roundedValue = GeneralStatsUtilities.RoundFloatStatToNDecimalPlaces(calculatedStatValue, 2);
        SetAttackSpeedStat(roundedValue);
    }

    private void SetAttackSpeedStat(float value) => attackSpeedStat = value;
}
