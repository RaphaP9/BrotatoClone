using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponInventoryContentsHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private WeaponInventoryUI weaponInventoryUI;
    [SerializeField] private List<Image> borders;

    [Header("Settings")]
    [SerializeField] private Color commonColor;
    [SerializeField] private Color uncommonColor;
    [SerializeField] private Color rareColor;
    [SerializeField] private Color epicColor;
    [SerializeField] private Color legendaryColor;

    [Header("UI Components")]
    [SerializeField] private Image weaponImage;

    private void OnEnable()
    {
        weaponInventoryUI.OnWeaponInventorySet += WeaponInventoryUI_OnWeaponInventorySet;
    }

    private void OnDisable()
    {
        weaponInventoryUI.OnWeaponInventorySet -= WeaponInventoryUI_OnWeaponInventorySet;
    }

    private void SetContents(WeaponInventoryIdentified weaponInventoryIdentified)
    {
        SetWeaponImage(weaponInventoryIdentified.weaponSO.sprite);
        SetBordersColor(weaponInventoryIdentified.weaponSO);
    }

    private void SetWeaponImage(Sprite sprite) => weaponImage.sprite = sprite;

    private void SetBordersColor(InventoryObjectSO inventoryObjectSO)
    {
        Color color;

        switch (inventoryObjectSO.objectRarity)
        {
            case InventoryObjectRarity.Common:
            default:
                color = commonColor;
                break;
            case InventoryObjectRarity.Uncommon:
                color = uncommonColor;
                break;
            case InventoryObjectRarity.Rare:
                color = rareColor;
                break;
            case InventoryObjectRarity.Epic:
                color = epicColor;
                break;
            case InventoryObjectRarity.Legendary:
                color = legendaryColor;
                break;
        }

        GeneralUIUtilities.SetImagesColor(borders, color);
    }

    private void WeaponInventoryUI_OnWeaponInventorySet(object sender, WeaponInventoryUI.OnWeaponInventorySetEventArgs e)
    {
        SetContents(e.weaponInventoryIdentified);
    }
}
