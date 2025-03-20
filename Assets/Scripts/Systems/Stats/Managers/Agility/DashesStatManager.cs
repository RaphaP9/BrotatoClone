using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashesStatManager : StatManager
{
    public static DashesStatManager Instance { get; private set; }

    public static event EventHandler<OnDashesStatEventArgs> OnDashesStatInitialized;
    public static event EventHandler<OnDashesStatEventArgs> OnDashesStatUpdated;

    [Header("Value")]
    [SerializeField] private int dashesStat;

    public int DashesStat => dashesStat;

    public class OnDashesStatEventArgs : EventArgs
    {
        public int dashesStat;
    }

    protected override void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one DashesStatManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    protected override void InitializeStat()
    {
        ProcessDashesStat();
        OnDashesStatInitialized?.Invoke(this, new OnDashesStatEventArgs { dashesStat = dashesStat });
    }

    protected override void UpdateStat()
    {
        ProcessDashesStat();
        OnDashesStatUpdated?.Invoke(this, new OnDashesStatEventArgs { dashesStat = dashesStat });
    }

    private void ProcessDashesStat()
    {
        float calculatedStatValue = CalculateStatValue(BaseStats.dashes, GeneralStatsUtilities.MIN_DASHES, GeneralStatsUtilities.MAX_DASHES);
        int roundedValue = GeneralStatsUtilities.RoundFloatStatToInt(calculatedStatValue, GeneralStatsUtilities.StatRoundingType.Ceil);
        SetDashesStat(roundedValue);
    }

    private void SetDashesStat(int value) => dashesStat = value;
    protected override StatType GetStatType() => StatType.Dashes;
}
