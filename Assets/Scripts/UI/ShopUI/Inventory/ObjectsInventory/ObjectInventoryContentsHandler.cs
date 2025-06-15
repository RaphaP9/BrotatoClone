using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectInventoryContentsHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private ObjectInventoryUI objectInventoryUI;
    [SerializeField] private List<Image> borders;

    [Header("Settings")]
    [SerializeField] private Color commonColor;
    [SerializeField] private Color uncommonColor;
    [SerializeField] private Color rareColor;
    [SerializeField] private Color epicColor;
    [SerializeField] private Color legendaryColor;

    [Header("UI Components")]
    [SerializeField] private Image objectImage;

    private void OnEnable()
    {
        objectInventoryUI.OnObjectInventorySet += ObjectInventoryUI_OnObjectInventorySet;
    }

    private void OnDisable()
    {
        objectInventoryUI.OnObjectInventorySet -= ObjectInventoryUI_OnObjectInventorySet;
    }

    private void SetContents(ObjectInventoryIdentified objectInventoryIdentified)
    {
        SetObjectImage(objectInventoryIdentified.objectSO.sprite);
        SetBordersColor(objectInventoryIdentified.objectSO);
    }

    private void SetObjectImage(Sprite sprite) => objectImage.sprite = sprite;

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

    private void ObjectInventoryUI_OnObjectInventorySet(object sender, ObjectInventoryUI.OnObjectInventorySetEventArgs e)
    {
        SetContents(e.objectInventoryIdentified);
    }
}
