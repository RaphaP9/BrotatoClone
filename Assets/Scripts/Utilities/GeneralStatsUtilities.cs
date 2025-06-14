using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GeneralStatsUtilities 
{
    #region Durability
    public static int MIN_MAX_HEALTH = 5;
    public static int MAX_MAX_HEALTH = 1000;

    public static int MIN_HEALTH_REGEN = 0;
    public static int MAX_HEALTH_REGEN = 100;

    public static float MIN_ARMOR_PERCENTAGE = 0f;
    public static float MAX_ARMOR_PERCENTAGE = 0.9f;

    public static float MIN_DODGE_CHANCE = 0f;
    public static float MAX_DODGE_CHANCE = 0.9f;
    #endregion

    #region Agility
    public static float MIN_MOVE_SPEED = 1f;
    public static float MAX_MOVE_SPEED = 20f;

    public static int MIN_DASHES = 0;
    public static int MAX_DASHES = 5;
    #endregion

    #region Attacks
    public static float MIN_AREA_MULTIPLIER = 0.5f;
    public static float MAX_AREA_MULTIPLIER = 4f;

    public static float MIN_ATTACK_SPEED_MULTIPLIER = 0.25f;
    public static float MAX_ATTACK_SPEED_MULTIPLIER = 4f;

    public static float MIN_ATTACK_RANGE_MULTIPLIER = 0.25f;
    public static float MAX_ATTACK_RANGE_MULTIPLIER = 4f;

    public static float MIN_ATTACK_DAMAGE_MULTIPLIER = 0.25f;
    public static float MAX_ATTACK_DAMAGE_MULTIPLIER = 4f;

    public static float MIN_ATTACK_CRIT_CHANCE = 0f;
    public static float MAX_ATTACK_CRIT_CHANCE = 1f;

    public static float MIN_ATTACK_CRIT_DAMAGE_MULTIPLIER = 0.25f;
    public static float MAX_ATTACK_CRIT_DAMAGE_MULTIPLIER = 4f;
    #endregion

    #region Lifesteal
    public static float MIN_LIFESTEAL = 0f;
    public static float MAX_LIFESTEAL = 1f;
    #endregion

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
