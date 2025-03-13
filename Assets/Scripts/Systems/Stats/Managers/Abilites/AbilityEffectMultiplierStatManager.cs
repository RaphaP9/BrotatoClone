using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityEffectMultiplierStatManager : StatManager
{
    public static AbilityEffectMultiplierStatManager Instance { get; private set; }

    public static event EventHandler<OnAbilityEffectMultiplierStatEventArgs> OnAbilityEffectMultiplierStatInitialized;
    public static event EventHandler<OnAbilityEffectMultiplierStatEventArgs> OnAbilityEffectMultiplierStatUpdated;

    [Header("Value")]
    [SerializeField] private float abilityEffectMultiplierStat;

    public class OnAbilityEffectMultiplierStatEventArgs : EventArgs
    {
        public float abilityEffectMultiplierStat;
    }

    protected override void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one AbilityEffectMultiplierStatManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    protected override void InitializeStat()
    {
        ProcessAbilityEffectMultiplierStat();
        OnAbilityEffectMultiplierStatInitialized?.Invoke(this, new OnAbilityEffectMultiplierStatEventArgs { abilityEffectMultiplierStat = abilityEffectMultiplierStat });
    }

    protected override void UpdateStat()
    {
        ProcessAbilityEffectMultiplierStat();
        OnAbilityEffectMultiplierStatUpdated?.Invoke(this, new OnAbilityEffectMultiplierStatEventArgs { abilityEffectMultiplierStat = abilityEffectMultiplierStat });
    }

    private void ProcessAbilityEffectMultiplierStat()
    {
        float calculatedStatValue = CalculateStatValue(BaseStats.abilityEffectMultiplier, GeneralStatsUtilities.MIN_ABILITY_EFFECT_MULTIPLIER, GeneralStatsUtilities.MAX_ABILITY_EFFECT_MULTIPLIER);
        float roundedValue = GeneralStatsUtilities.RoundFloatStatToNDecimalPlaces(calculatedStatValue, 2);
        SetAbilityEffectMultiplierStat(roundedValue);
    }

    private void SetAbilityEffectMultiplierStat(float value) => abilityEffectMultiplierStat = value;
}
