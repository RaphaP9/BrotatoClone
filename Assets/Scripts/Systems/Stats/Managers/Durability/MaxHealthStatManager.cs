using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MaxHealthStatManager : StatManager
{
    public static MaxHealthStatManager Instance { get; private set; }

    public static event EventHandler<OnMaxHealthStatEventArgs> OnMaxHealthStatInitialized;
    public static event EventHandler<OnMaxHealthStatEventArgs> OnMaxHealthStatUpdated;

    [Header("Value")]
    [SerializeField] private int maxHealthStat;

    public class OnMaxHealthStatEventArgs : EventArgs
    {
        public int maxHealthStat;
    }

    protected override void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one MaxHealthStatManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    protected override void InitializeStat()
    {
        ProcessMaxHealthStat();
        OnMaxHealthStatInitialized?.Invoke(this, new OnMaxHealthStatEventArgs { maxHealthStat = maxHealthStat });
    }

    protected override void UpdateStat()
    {
        ProcessMaxHealthStat();
        OnMaxHealthStatUpdated?.Invoke(this, new OnMaxHealthStatEventArgs { maxHealthStat = maxHealthStat });
    }

    private void ProcessMaxHealthStat()
    {
        float calculatedStatValue = CalculateStatValue(baseStats.maxHealth, GeneralStatsUtilities.MIN_MAX_HEALTH, GeneralStatsUtilities.MAX_MAX_HEALTH);
        int roundedValue = GeneralStatsUtilities.RoundFloatStatToInt(calculatedStatValue, GeneralStatsUtilities.StatRoundingType.Ceil);
        SetMaxHealthStat(roundedValue);
    }

    private void SetMaxHealthStat(int value) => maxHealthStat = value;
}
