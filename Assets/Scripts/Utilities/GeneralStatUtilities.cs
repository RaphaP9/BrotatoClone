using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GeneralStatUtilities 
{
    public static int MIN_MAX_HEALTH = 10;
    public static int MAX_MAX_HEALTH = 100;

    public static float MIN_MOVE_SPEED = 1f;
    public static float MAX_MOVE_SPEED = 10f;

    public enum StatRoundingType
    {
        Round,
        Ceil,
        Floor
    }


    public static int RoundFloatStatToInt(float statValue, StatRoundingType statRoundingType )
    {
        switch (statRoundingType)
        {
            case StatRoundingType.Round:
            default:
                return Mathf.RoundToInt(statValue);
            case StatRoundingType.Ceil:
                return Mathf.CeilToInt(statValue);
            case StatRoundingType.Floor:
                return Mathf.FloorToInt(statValue);
        }
    }

    public static float RoundFloatStatToNDecimalPlaces(float statValue, int decimalPlaces) => Mathf.Round(statValue * Mathf.Pow(10, decimalPlaces)) / Mathf.Pow(10, decimalPlaces);
}
