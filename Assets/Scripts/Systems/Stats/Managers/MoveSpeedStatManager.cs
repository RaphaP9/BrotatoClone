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
        float calculatedStatValue = CalculateStatValue(baseStats.moveSpeed, GeneralStatUtilities.MIN_MOVE_SPEED, GeneralStatUtilities.MAX_MOVE_SPEED);
        float roundedValue = GeneralStatUtilities.RoundFloatStatToNDecimalPlaces(calculatedStatValue, 2);
        SetMoveSpeedStat(roundedValue);
    }

    private void SetMoveSpeedStat(float value) => moveSpeedStat = value;
}