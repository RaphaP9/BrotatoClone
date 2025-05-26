using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public static class GeneralUIUtilities
{
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
}
