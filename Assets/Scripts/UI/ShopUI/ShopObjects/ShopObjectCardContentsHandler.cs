using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopObjectCardContentsHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private ShopObjectCardUI shopObjectCardUI;

    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI objectNameText;
    [SerializeField] private Image objectImage;
    [SerializeField] private TextMeshProUGUI objectClassificationText;

    [SerializeField] private List<Image> borders;

    [Header("Settings")]
    [SerializeField] private Color commonColor;
    [SerializeField] private Color uncommonColor;
    [SerializeField] private Color rareColor;
    [SerializeField] private Color epicColor;
    [SerializeField] private Color legendaryColor;

    private void OnEnable()
    {
        shopObjectCardUI.OnInventoryObjectSet += ShopObjectCardUI_OnInventoryObjectSet;
    }
    private void OnDisable()
    {
        shopObjectCardUI.OnInventoryObjectSet -= ShopObjectCardUI_OnInventoryObjectSet;
    }

    public void CompleteSetUI(InventoryObjectSO inventoryObjectSO)
    {
        SetObjectNameText(inventoryObjectSO);
        SetObjectImage(inventoryObjectSO);
        SetObjectClassificationText(inventoryObjectSO);
        SetBordersColor(inventoryObjectSO);
    }

    private void SetObjectNameText(InventoryObjectSO inventoryObjectSO) => objectNameText.text = inventoryObjectSO.inventoryObjectName;
    private void SetObjectImage(InventoryObjectSO inventoryObjectSO) => objectImage.sprite = inventoryObjectSO.sprite;
    private void SetObjectClassificationText(InventoryObjectSO inventoryObjectSO) => objectClassificationText.text = GeneralUIUtilities.MapInventoryObjectRarityType(inventoryObjectSO);
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


    private void ShopObjectCardUI_OnInventoryObjectSet(object sender, ShopObjectCardUI.OnInventoryObjectEventArgs e)
    {
        CompleteSetUI(e.inventoryObjectSO);
    }

}
