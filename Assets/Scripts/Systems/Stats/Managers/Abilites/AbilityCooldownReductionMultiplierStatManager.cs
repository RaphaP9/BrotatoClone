using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityCooldownReductionMultiplierStatManager : StatManager
{
    public static AbilityCooldownReductionMultiplierStatManager Instance { get; private set; }

    public static event EventHandler<OnAbilityCooldownReductionMultiplierStatEventArgs> OnAbilityCooldownReductionMultiplierStatInitialized;
    public static event EventHandler<OnAbilityCooldownReductionMultiplierStatEventArgs> OnAbilityCooldownReductionMultiplierStatUpdated;

    [Header("Value")]
    [SerializeField] private float abilityCooldownReductionMultiplierStat;

    public float AbilityCooldownReductionStat => abilityCooldownReductionMultiplierStat;

    public class OnAbilityCooldownReductionMultiplierStatEventArgs : EventArgs
    {
        public float abilityCooldownReductionMultiplierStat;
    }

    protected override void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one AbilityCooldownReductionStatManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    protected override void InitializeStat()
    {
        ProcessAbilityCooldownReductionStat();
        OnAbilityCooldownReductionMultiplierStatInitialized?.Invoke(this, new OnAbilityCooldownReductionMultiplierStatEventArgs { abilityCooldownReductionMultiplierStat = abilityCooldownReductionMultiplierStat });
    }

    protected override void UpdateStat()
    {
        ProcessAbilityCooldownReductionStat();
        OnAbilityCooldownReductionMultiplierStatUpdated?.Invoke(this, new OnAbilityCooldownReductionMultiplierStatEventArgs { abilityCooldownReductionMultiplierStat = abilityCooldownReductionMultiplierStat });
    }

    private void ProcessAbilityCooldownReductionStat()
    {
        float calculatedStatValue = CalculateStatValue(BaseStats.abilityCooldownReductionMultiplier, GeneralStatsUtilities.MIN_ABILITY_COOLDOWN_REDUCTION, GeneralStatsUtilities.MAX_ABILITY_COOLDOWN_REDUCTION);
        float roundedValue = GeneralStatsUtilities.RoundFloatStatToNDecimalPlaces(calculatedStatValue, 2);
        SetAbilityCooldownReductionStat(roundedValue);
    }

    private void SetAbilityCooldownReductionStat(float value) => abilityCooldownReductionMultiplierStat = value;
    protected override StatType GetStatType() => StatType.AbilityCooldownReduction;
}
