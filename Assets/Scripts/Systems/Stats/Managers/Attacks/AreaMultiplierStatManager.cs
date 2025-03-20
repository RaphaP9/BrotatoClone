using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaMultiplierStatManager : StatManager
{
    public static AreaMultiplierStatManager Instance { get; private set; }

    public static event EventHandler<OnAreaMultiplierStatEventArgs> OnAreaMultiplierStatInitialized;
    public static event EventHandler<OnAreaMultiplierStatEventArgs> OnAreaMultiplierStatUpdated;

    [Header("Value")]
    [SerializeField] private float areaMultiplierStat;

    public float AreaMultiplierStat => areaMultiplierStat;

    public class OnAreaMultiplierStatEventArgs : EventArgs
    {
        public float areaMultiplierStat;
    }

    protected override void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one AreaMultiplierStatManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    protected override void InitializeStat()
    {
        ProcessAreaMultiplierStat();
        OnAreaMultiplierStatInitialized?.Invoke(this, new OnAreaMultiplierStatEventArgs { areaMultiplierStat = areaMultiplierStat });
    }

    protected override void UpdateStat()
    {
        ProcessAreaMultiplierStat();
        OnAreaMultiplierStatUpdated?.Invoke(this, new OnAreaMultiplierStatEventArgs { areaMultiplierStat = areaMultiplierStat });
    }

    private void ProcessAreaMultiplierStat()
    {
        float calculatedStatValue = CalculateStatValue(BaseStats.areaMultiplier, GeneralStatsUtilities.MIN_AREA_MULTIPLIER, GeneralStatsUtilities.MAX_AREA_MULTIPLIER);
        float roundedValue = GeneralStatsUtilities.RoundFloatStatToNDecimalPlaces(calculatedStatValue, 2);
        SetAreaMultiplierStat(roundedValue);
    }

    private void SetAreaMultiplierStat(float value) => areaMultiplierStat = value;
    protected override StatType GetStatType() => StatType.AreaMultiplier;
}

