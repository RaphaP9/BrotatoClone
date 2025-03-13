using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityCritEffectMultiplierStatManager : StatManager
{
    public static AbilityCritEffectMultiplierStatManager Instance { get; private set; }

    public static event EventHandler<OnAbilityCritEffectMultiplierStatEventArgs> OnAbilityCritEffectMultiplierStatInitialized;
    public static event EventHandler<OnAbilityCritEffectMultiplierStatEventArgs> OnAbilityCritEffectMultiplierStatUpdated;

    [Header("Value")]
    [SerializeField] private float abilityCritEffectMultiplierStat;

    public class OnAbilityCritEffectMultiplierStatEventArgs : EventArgs
    {
        public float abilityCritEffectMultiplierStat;
    }

    protected override void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one AbilityCritEffectMultiplierStatManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    protected override void InitializeStat()
    {
        ProcessAbilityCritEffectMultiplierStat();
        OnAbilityCritEffectMultiplierStatInitialized?.Invoke(this, new OnAbilityCritEffectMultiplierStatEventArgs { abilityCritEffectMultiplierStat = abilityCritEffectMultiplierStat });
    }

    protected override void UpdateStat()
    {
        ProcessAbilityCritEffectMultiplierStat();
        OnAbilityCritEffectMultiplierStatUpdated?.Invoke(this, new OnAbilityCritEffectMultiplierStatEventArgs { abilityCritEffectMultiplierStat = abilityCritEffectMultiplierStat });
    }

    private void ProcessAbilityCritEffectMultiplierStat()
    {
        float calculatedStatValue = CalculateStatValue(baseStats.abilityCritEffectMultiplier, GeneralStatsUtilities.MIN_ABILITY_CRIT_EFFECT_MULTIPLIER, GeneralStatsUtilities.MAX_ABILITY_CRIT_EFFECT_MULTIPLIER);
        float roundedValue = GeneralStatsUtilities.RoundFloatStatToNDecimalPlaces(calculatedStatValue, 2);
        SetAbilityCritEffectMultiplierStat(roundedValue);
    }

    private void SetAbilityCritEffectMultiplierStat(float value) => abilityCritEffectMultiplierStat = value;
}

