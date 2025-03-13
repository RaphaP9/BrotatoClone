using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorPercentageStatManager : StatManager
{
    public static ArmorPercentageStatManager Instance { get; private set; }

    public static event EventHandler<OnArmorPercentageStatEventArgs> OnArmorPercentageStatInitialized;
    public static event EventHandler<OnArmorPercentageStatEventArgs> OnArmorPercentageStatUpdated;

    [Header("Value")]
    [SerializeField] private float armorPercentageStat;

    public class OnArmorPercentageStatEventArgs : EventArgs
    {
        public float armorPercentageStat;
    }

    protected override void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one ArmorPercentageStatManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    protected override void InitializeStat()
    {
        ProcessArmorPercentageStat();
        OnArmorPercentageStatInitialized?.Invoke(this, new OnArmorPercentageStatEventArgs { armorPercentageStat = armorPercentageStat });
    }

    protected override void UpdateStat()
    {
        ProcessArmorPercentageStat();
        OnArmorPercentageStatUpdated?.Invoke(this, new OnArmorPercentageStatEventArgs { armorPercentageStat = armorPercentageStat });
    }

    private void ProcessArmorPercentageStat()
    {
        float calculatedStatValue = CalculateStatValue(baseStats.armorPercentage, GeneralStatsUtilities.MIN_ARMOR_PERCENTAGE, GeneralStatsUtilities.MAX_ARMOR_PERCENTAGE);
        float roundedValue = GeneralStatsUtilities.RoundFloatStatToNDecimalPlaces(calculatedStatValue, 2);
        SetHealthRegenStat(roundedValue);
    }

    private void SetHealthRegenStat(float value) => armorPercentageStat = value;
}
