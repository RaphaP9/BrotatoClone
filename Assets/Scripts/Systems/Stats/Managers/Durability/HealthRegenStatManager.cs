using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthRegenStatManager : StatManager
{
    public static HealthRegenStatManager Instance { get; private set; }

    public static event EventHandler<OnHealthRegenStatEventArgs> OnHealthRegenStatInitialized;
    public static event EventHandler<OnHealthRegenStatEventArgs> OnHealthRegenStatUpdated;

    [Header("Value")]
    [SerializeField] private int healthRegenStat;

    public class OnHealthRegenStatEventArgs : EventArgs
    {
        public int healthRegenStat;
    }

    protected override void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one HealthRegenStatManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    protected override void InitializeStat()
    {
        ProcessHealthRegenStat();
        OnHealthRegenStatInitialized?.Invoke(this, new OnHealthRegenStatEventArgs { healthRegenStat = healthRegenStat });
    }

    protected override void UpdateStat()
    {
        ProcessHealthRegenStat();
        OnHealthRegenStatUpdated?.Invoke(this, new OnHealthRegenStatEventArgs { healthRegenStat = healthRegenStat });
    }

    private void ProcessHealthRegenStat()
    {
        float calculatedStatValue = CalculateStatValue(baseStats.healthRegen, GeneralStatsUtilities.MIN_HEALTH_REGEN, GeneralStatsUtilities.MAX_HEALTH_REGEN);
        int roundedValue = GeneralStatsUtilities.RoundFloatStatToInt(calculatedStatValue, GeneralStatsUtilities.StatRoundingType.Ceil);
        SetHealthRegenStat(roundedValue);
    }

    private void SetHealthRegenStat(int value) => healthRegenStat = value;
}
