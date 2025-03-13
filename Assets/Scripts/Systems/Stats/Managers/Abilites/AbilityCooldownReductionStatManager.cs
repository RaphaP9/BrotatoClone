using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityCooldownReductionStatManager : StatManager
{
    public static AbilityCooldownReductionStatManager Instance { get; private set; }

    public static event EventHandler<OnAbilityCooldownReductionStatEventArgs> OnAbilityCooldownReductionStatInitialized;
    public static event EventHandler<OnAbilityCooldownReductionStatEventArgs> OnAbilityCooldownReductionStatUpdated;

    [Header("Value")]
    [SerializeField] private float abilityCooldownReductionStat;

    public class OnAbilityCooldownReductionStatEventArgs : EventArgs
    {
        public float abilityCooldownReductionStat;
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
        OnAbilityCooldownReductionStatInitialized?.Invoke(this, new OnAbilityCooldownReductionStatEventArgs { abilityCooldownReductionStat = abilityCooldownReductionStat });
    }

    protected override void UpdateStat()
    {
        ProcessAbilityCooldownReductionStat();
        OnAbilityCooldownReductionStatUpdated?.Invoke(this, new OnAbilityCooldownReductionStatEventArgs { abilityCooldownReductionStat = abilityCooldownReductionStat });
    }

    private void ProcessAbilityCooldownReductionStat()
    {
        float calculatedStatValue = CalculateStatValue(baseStats.abilityCooldownReduction, GeneralStatsUtilities.MIN_ABILITY_COOLDOWN_REDUCTION, GeneralStatsUtilities.MAX_ABILITY_COOLDOWN_REDUCTION);
        float roundedValue = GeneralStatsUtilities.RoundFloatStatToNDecimalPlaces(calculatedStatValue, 2);
        SetAbilityCooldownReductionStat(roundedValue);
    }

    private void SetAbilityCooldownReductionStat(float value) => abilityCooldownReductionStat = value;
}
