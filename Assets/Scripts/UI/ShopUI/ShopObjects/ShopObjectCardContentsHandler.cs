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
    [Space]
    [SerializeField] private Transform statsContainer;
    [SerializeField] private Transform statUISample;
    [Space]
    [SerializeField] private TextMeshProUGUI objectDescriptionText;
    [Space]
    [SerializeField] private List<Image> borders;

    [Header("Settings")]
    [SerializeField] private Color commonColor;
    [SerializeField] private Color uncommonColor;
    [SerializeField] private Color rareColor;
    [SerializeField] private Color epicColor;
    [SerializeField] private Color legendaryColor;

    [Header("Debug")]
    [SerializeField] private bool debug;

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
        SetObjectDescriptionText(inventoryObjectSO);

        GenerateNumericStats(inventoryObjectSO);
    }

    private void SetObjectNameText(InventoryObjectSO inventoryObjectSO) => objectNameText.text = inventoryObjectSO.inventoryObjectName;
    private void SetObjectImage(InventoryObjectSO inventoryObjectSO) => objectImage.sprite = inventoryObjectSO.sprite;
    private void SetObjectClassificationText(InventoryObjectSO inventoryObjectSO) => objectClassificationText.text = GeneralUIUtilities.MapInventoryObjectRarityType(inventoryObjectSO);
    private void SetObjectDescriptionText(InventoryObjectSO inventoryObjectSO) => objectDescriptionText.text = inventoryObjectSO.description;

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

    private void GenerateNumericStats(InventoryObjectSO inventoryObjectSO)
    {
        ClearNumericStatsContainer();

        if (inventoryObjectSO.embeddedStats.Count <= 0)
        {
            statsContainer.gameObject.SetActive(false);
            return;
        }

        foreach (EmbeddedStat embeddedStat in inventoryObjectSO.embeddedStats)
        {
            CreateNumericStat(embeddedStat);
        }
    }

    private void CreateNumericStat(EmbeddedStat embeddedStat)
    {
        Transform numericStatUI = Instantiate(statUISample, statsContainer);

        ShopObjectCardStatUI shopObjectCardStatUI = numericStatUI.GetComponent<ShopObjectCardStatUI>();

        if (shopObjectCardStatUI == null)
        {
            if (debug) Debug.Log("Instantiated Numeric Stat UI does not contain a ShopObjectCardStatUI component. Set will be ignored.");
            return;
        }

        shopObjectCardStatUI.SetEmbeddedStat(embeddedStat);
        numericStatUI.gameObject.SetActive(true);
    }

    private void ClearNumericStatsContainer()
    {
        foreach (Transform child in statsContainer)
        {
            child.gameObject.SetActive(false);
        }
    }


    private void ShopObjectCardUI_OnInventoryObjectSet(object sender, ShopObjectCardUI.OnInventoryObjectEventArgs e)
    {
        CompleteSetUI(e.inventoryObjectSO);
    }

}
