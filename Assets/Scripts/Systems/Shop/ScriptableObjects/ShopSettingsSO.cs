using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewShopSettingsSO", menuName = "ScriptableObjects/Shop/ShopSettings")]

public class ShopSettingsSO : ScriptableObject
{
    [Header("Shop Size")]
    [Range(3, 7)]public int shopSize;

    [Header("Type Settings")]
    public List<InventoryObjectTypeSetting> inventoryObjectTypeSettings;

    [Header("Rarity Settings")]
    public List<InventoryObjectRaritySetting> inventoryObjectRaritySettings;

    [Header("Rerolls")]
    [Range(1, 100)] public int rerollBaseCost;
    [Range(1, 10)] public int rerollCostIncreasePerReroll;

    [Header("Pools")]
    public List<WeaponSO> weaponsPool;
    public List<ObjectSO> objectsPool;
    public List<AbilitySO> abilitiesPool;
    public List<ElementSO> elementsPool;
}


////////////////////////////////////////////////////////////////////////////////////////////////////////////

[System.Serializable]
public class InventoryObjectTypeSetting
{
    public InventoryObjectType objectType;
    [Range(0, 100)] public int weight;
    [Range(0, 5)] public int cap;
}

[System.Serializable]
public class InventoryObjectRaritySetting
{
    public InventoryObjectRarity objectRarity;
    [Range(0, 100)] public int weight;
    [Range(0, 5)] public int cap;
}




