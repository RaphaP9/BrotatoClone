using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifestealStatManager : StatManager
{
    public static LifestealStatManager Instance { get; private set; }

    public static event EventHandler<OnLifestealStatEventArgs> OnLifestealStatInitialized;
    public static event EventHandler<OnLifestealStatEventArgs> OnLifestealStatUpdated;

    [Header("Value")]
    [SerializeField] private float lifestealStat;

    public float LifestealStat => lifestealStat;

    public class OnLifestealStatEventArgs : EventArgs
    {
        public float lifestealStat;
    }

    protected override void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one LifestealStatManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    protected override void InitializeStat()
    {
        ProcessLifestealStat();
        OnLifestealStatInitialized?.Invoke(this, new OnLifestealStatEventArgs { lifestealStat = lifestealStat });
    }

    protected override void UpdateStat()
    {
        ProcessLifestealStat();
        OnLifestealStatUpdated?.Invoke(this, new OnLifestealStatEventArgs { lifestealStat = lifestealStat });
    }

    private void ProcessLifestealStat()
    {
        float calculatedStatValue = CalculateStatValue(BaseStats.lifeSteal, GeneralStatsUtilities.MIN_LIFESTEAL, GeneralStatsUtilities.MAX_LIFESTEAL);
        float roundedValue = GeneralStatsUtilities.RoundFloatStatToNDecimalPlaces(calculatedStatValue, 2);
        SetLifestealStat(roundedValue);
    }

    private void SetLifestealStat(float value) => lifestealStat = value;
}

