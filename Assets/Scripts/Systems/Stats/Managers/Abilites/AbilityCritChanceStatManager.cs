using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityCritChanceStatManager : StatManager
{
    public static AbilityCritChanceStatManager Instance { get; private set; }

    public static event EventHandler<OnAbilityCritChanceStatEventArgs> OnAbilityCritChanceStatInitialized;
    public static event EventHandler<OnAbilityCritChanceStatEventArgs> OnAbilityCritChanceStatUpdated;

    [Header("Value")]
    [SerializeField] private float abilityCritChanceStat;

    public float AbilityCritChanceStat => abilityCritChanceStat;

    public class OnAbilityCritChanceStatEventArgs : EventArgs
    {
        public float abilityCritChanceStat;
    }

    protected override void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one AbilityCritChanceStatManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    protected override void InitializeStat()
    {
        ProcessAbilityCritChanceStat();
        OnAbilityCritChanceStatInitialized?.Invoke(this, new OnAbilityCritChanceStatEventArgs { abilityCritChanceStat = abilityCritChanceStat });
    }

    protected override void UpdateStat()
    {
        ProcessAbilityCritChanceStat();
        OnAbilityCritChanceStatUpdated?.Invoke(this, new OnAbilityCritChanceStatEventArgs { abilityCritChanceStat = abilityCritChanceStat });
    }

    private void ProcessAbilityCritChanceStat()
    {
        float calculatedStatValue = CalculateStatValue(BaseStats.abilityCritChance, GeneralStatsUtilities.MIN_ABILITY_CRIT_CHANCE, GeneralStatsUtilities.MAX_ABILITY_CRIT_CHANCE);
        float roundedValue = GeneralStatsUtilities.RoundFloatStatToNDecimalPlaces(calculatedStatValue, 2);
        SetAbilityCritChanceMultiplierStat(roundedValue);
    }

    private void SetAbilityCritChanceMultiplierStat(float value) => abilityCritChanceStat = value;
}
