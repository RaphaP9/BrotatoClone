using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeChanceStatManager : StatManager
{
    public static DodgeChanceStatManager Instance { get; private set; }

    public static event EventHandler<OnDodgeChanceStatEventArgs> OnDodgeChanceStatInitialized;
    public static event EventHandler<OnDodgeChanceStatEventArgs> OnDodgeChanceStatUpdated;

    [Header("Value")]
    [SerializeField] private float dodgeChanceStat;

    public float DodgeChanceStat => dodgeChanceStat;

    public class OnDodgeChanceStatEventArgs : EventArgs
    {
        public float dodgeChanceStat;
    }

    protected override void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one DodgeChanceStatManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    protected override void InitializeStat()
    {
        ProcessDodgeChanceStat();
        OnDodgeChanceStatInitialized?.Invoke(this, new OnDodgeChanceStatEventArgs { dodgeChanceStat = dodgeChanceStat });
    }

    protected override void UpdateStat()
    {
        ProcessDodgeChanceStat();
        OnDodgeChanceStatUpdated?.Invoke(this, new OnDodgeChanceStatEventArgs { dodgeChanceStat = dodgeChanceStat });
    }

    private void ProcessDodgeChanceStat()
    {
        float calculatedStatValue = CalculateStatValue(BaseStats.dodgeChance, GeneralStatsUtilities.MIN_DODGE_CHANCE, GeneralStatsUtilities.MAX_DODGE_CHANCE);
        float roundedValue = GeneralStatsUtilities.RoundFloatStatToNDecimalPlaces(calculatedStatValue, 2);
        SetDodgeChanceStat(roundedValue);
    }

    private void SetDodgeChanceStat(float value) => dodgeChanceStat = value;
    protected override StatType GetStatType() => StatType.DodgeChance;
}
