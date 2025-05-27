using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public static class GeneralUIUtilities
{
    #region Object Classification Consts
    private const string COMMON_WEAPON_TEXT = "Arma Común";
    private const string UNCOMMON_WEAPON_TEXT = "Arma Poco Común";
    private const string RARE_WEAPON_TEXT = "Arma Rara";
    private const string EPIC_WEAPON_TEXT = "Arma Épica";
    private const string LEGENDARY_WEAPON_TEXT = "Arma Legendaria";

    private const string COMMON_OBJECT_TEXT = "Objeto Común";
    private const string UNCOMMON_OBJECT_TEXT = "Objeto Poco Común";
    private const string RARE_OBJECT_TEXT = "Objeto Raro";
    private const string EPIC_OBJECT_TEXT = "Objeto Épico";
    private const string LEGENDARY_OBJECT_TEXT = "Objeto Legendario";
    #endregion

    #region Stat Description Text
    private const string MAX_HEALTH_STAT = "Vida Máxima";
    private const string HEALTH_REGEN_STAT = "Regen. de Vida";
    private const string ARMOR_PERCENTAGE_STAT = "Armadura";
    private const string DODGE_CHANCE_STAT = "Evasión";
    private const string MOVEMENT_SPEED_STAT = "Vel. de Movimiento";
    private const string DASHES_STAT = "Desplazamiento(s)";
    private const string ATTACK_AREA_STAT = "Area de Ataque";
    private const string ATTACK_SPEED_STAT = "Vel. de Ataque";
    private const string ATTACK_RANGE_STAT = "Rango de Ataque";
    private const string ATTACK_DAMAGE_MULT_STAT = "Daño de Ataque";
    private const string ATTACK_CRIT_CHANCE_STAT = "Prob. de Crítico";
    private const string ATTACK_CRIT_DAMAGE_MULTIPLIER_STAT = "Daño Crítico";
    private const string LIFESTEAL_STAT = "Robo de Vida";
    #endregion

    public static void SetCanvasGroupAlpha(CanvasGroup canvasGroup, float alpha) => canvasGroup.alpha = alpha;
    public static void SetImageFillRatio(Image image, float fillRatio) => image.fillAmount = fillRatio;
    public static void SetImageColor(Image image, Color color) => image.color = color;
    public static void SetImagesColor(List<Image> images, Color color)
    {
        foreach (Image image in images)
        {
            SetImageColor(image, color);
        }
    }

    public static string MapInventoryObjectRarityType(InventoryObjectSO inventoryObjectSO)
    {
        switch (inventoryObjectSO.GetInventoryObjectType())
        {
            case InventoryObjectType.Weapon:
            default:
                switch (inventoryObjectSO.objectRarity)
                {
                    case InventoryObjectRarity.Common:
                    default:
                        return COMMON_WEAPON_TEXT;
                    case InventoryObjectRarity.Uncommon:
                        return UNCOMMON_WEAPON_TEXT;
                    case InventoryObjectRarity.Rare:
                        return RARE_WEAPON_TEXT;
                    case InventoryObjectRarity.Epic:
                        return EPIC_WEAPON_TEXT;
                    case InventoryObjectRarity.Legendary:
                        return LEGENDARY_WEAPON_TEXT;
                }

            case InventoryObjectType.Object:
                switch (inventoryObjectSO.objectRarity)
                {
                    case InventoryObjectRarity.Common:
                    default:
                        return COMMON_OBJECT_TEXT;
                    case InventoryObjectRarity.Uncommon:
                        return UNCOMMON_OBJECT_TEXT;
                    case InventoryObjectRarity.Rare:
                        return RARE_OBJECT_TEXT;
                    case InventoryObjectRarity.Epic:
                        return EPIC_OBJECT_TEXT;
                    case InventoryObjectRarity.Legendary:
                        return LEGENDARY_OBJECT_TEXT;
                }
        }
    }

    public static string MapStatType(StatType statType)
    {
        switch (statType)
        {
            case StatType.MaxHealth:
            default:
                return MAX_HEALTH_STAT;
            case StatType.HealthRegen:
                return HEALTH_REGEN_STAT;
            case StatType.ArmorPercentage:
                return ARMOR_PERCENTAGE_STAT;
            case StatType.DodgeChance:
                return DODGE_CHANCE_STAT;
            case StatType.MoveSpeed:
                return MOVEMENT_SPEED_STAT;
            case StatType.Dashes:
                return DASHES_STAT;
            case StatType.AreaMultiplier:
                return ATTACK_AREA_STAT;
            case StatType.AttackSpeedMultiplier:
                return ATTACK_SPEED_STAT;
            case StatType.AttackRangeMultiplier:
                return ATTACK_RANGE_STAT;
            case StatType.AttackDamageMultiplier:
                return ATTACK_DAMAGE_MULT_STAT;
            case StatType.AttackCritChance:
                return ATTACK_CRIT_CHANCE_STAT;
            case StatType.AttackCritDamageMultiplier:
                return ATTACK_CRIT_DAMAGE_MULTIPLIER_STAT;
            case StatType.Lifesteal:
                return LIFESTEAL_STAT;
        }
    }
}
