using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSpeedStatManager : StatManager
{
    public static MoveSpeedStatManager Instance { get; private set; }

    public static event EventHandler<OnMoveSpeedStatEventArgs> OnMoveSpeedStatInitialized;
    public static event EventHandler<OnMoveSpeedStatEventArgs> OnMoveSpeedStatUpdated;

    [Header("Value")]
    [SerializeField] private float moveSpeedStat;

    public float MoveSpeedStat => moveSpeedStat;

    public class OnMoveSpeedStatEventArgs : EventArgs
    {
        public float moveSpeedStat;
    }

    protected override void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one MoveSpeedStatManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    protected override void InitializeStat()
    {
        ProcessMoveSpeedStat();
        OnMoveSpeedStatInitialized?.Invoke(this, new OnMoveSpeedStatEventArgs { moveSpeedStat = moveSpeedStat });
    }

    protected override void UpdateStat()
    {
        ProcessMoveSpeedStat();
        OnMoveSpeedStatUpdated?.Invoke(this, new OnMoveSpeedStatEventArgs { moveSpeedStat = moveSpeedStat });
    }

    private void ProcessMoveSpeedStat()
    {
        float calculatedStatValue = CalculateStatValue(BaseStats.moveSpeed, GeneralStatsUtilities.MIN_MOVE_SPEED, GeneralStatsUtilities.MAX_MOVE_SPEED);
        float roundedValue = GeneralStatsUtilities.RoundFloatStatToNDecimalPlaces(calculatedStatValue, 2);
        SetMoveSpeedStat(roundedValue);
    }

    private void SetMoveSpeedStat(float value) => moveSpeedStat = value;

    protected override StatType GetStatType() => StatType.MoveSpeed;
}