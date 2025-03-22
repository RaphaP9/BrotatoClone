using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Playables;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private ShopSettingsSO shopSettingsSO;

    [Header("Debug")]
    [SerializeField] private bool debug; 

    private const InventoryObjectRarity DEFAULT_OBJECT_RARITY = InventoryObjectRarity.Common;
    private const InventoryObjectType DEFAULT_OBJECT_TYPE = InventoryObjectType.Object;

    private List<WeaponSO> GetShopAvailableWeaponsFromCompleteWeaponsList(ShopSettingsSO shopSettingsSO)
    {
        List<WeaponSO> validWeapons = new List<WeaponSO>();

        foreach (WeaponSO weapon in shopSettingsSO.weaponsPool)
        {
            if (ElementsInventoryManager.Instance.ElementsInInventoryByElementSO(weapon.requiredElements)) validWeapons.Add(weapon);
        }

        return validWeapons;
    }

    private List<ObjectSO> GetShopAvailableObjectsFromCompleteObjectsList(ShopSettingsSO shopSettingsSO)
    {
        List<ObjectSO> validObjects = new List<ObjectSO>();

        foreach (ObjectSO @object in shopSettingsSO.objectsPool)
        {
            if (ElementsInventoryManager.Instance.ElementsInInventoryByElementSO(@object.requiredElements)) validObjects.Add(@object);
        }

        return validObjects;
    }

    private List<AbilitySO> GetShopAvailableAbilitiesFromCompleteAbilitiesList(ShopSettingsSO shopSettingsSO)
    {
        List<AbilitySO> validAbilities = new List<AbilitySO>();

        foreach (AbilitySO ability in shopSettingsSO.abilitiesPool)
        {
            if (AbilitiesInventoryManager.Instance.AbilityInInventoryByAbilitySO(ability)) continue; //Unique Abilities
            if (ElementsInventoryManager.Instance.ElementsInInventoryByElementSO(ability.requiredElements)) validAbilities.Add(ability);
        }

        return validAbilities;
    }

    private List<ElementSO> GetShopAvailableElementsFromCompleteElementsList(ShopSettingsSO shopSettingsSO)
    {
        List<ElementSO> validElements = new List<ElementSO>();

        foreach (ElementSO element in shopSettingsSO.elementsPool)
        {
            if (ElementsInventoryManager.Instance.ElementInInventoryByElementSO(element)) continue; //Unique Elements
            if (ElementsInventoryManager.Instance.ElementsInInventoryByElementSO(element.requiredElements)) validElements.Add(element);
        }

        return validElements;
    }

    private List<InventoryObjectSO> GenerateShopObjectsList(ShopSettingsSO shopSettingsSO)
    {
        List<InventoryObjectSO> generatedList = new List<InventoryObjectSO>();

        for (int i = 0; i < shopSettingsSO.shopSize; i++)
        {
            InventoryObjectSO shopObject = GenerateShopObject(shopSettingsSO, generatedList);
            generatedList.Add(shopObject);
        }

        return generatedList;
    }

    private InventoryObjectSO GenerateShopObject(ShopSettingsSO shopSettingsSO, List<InventoryObjectSO> currentGeneratedList)
    {
        return null;
    }

    private InventoryObjectRarity GenerateInventoryObjectRarity(ShopSettingsSO shopSettingsSO)
    {
        int totalWeight = shopSettingsSO.inventoryObjectRaritySettings.Sum(x => x.weight);

        if (totalWeight <= 0) return DEFAULT_OBJECT_RARITY;

        int randomValue = Random.Range(0, totalWeight);

        int currentWeight = 0;

        foreach (InventoryObjectRaritySetting raritySetting in shopSettingsSO.inventoryObjectRaritySettings)
        {
            currentWeight += raritySetting.weight;

            if (randomValue <= currentWeight) return raritySetting.objectRarity;
        }

        return shopSettingsSO.inventoryObjectRaritySettings[0].objectRarity;
    }

    private InventoryObjectType GenerateInventoryObjectType(ShopSettingsSO shopSettingsSO)
    {
        int totalWeight = shopSettingsSO.inventoryObjectTypeSettings.Sum(x => x.weight);

        if(totalWeight <= 0) return DEFAULT_OBJECT_TYPE;

        int randomValue = Random.Range(0, totalWeight);

        int currentWeight = 0;

        foreach (InventoryObjectTypeSetting typeSetting in shopSettingsSO.inventoryObjectTypeSettings)
        {
            currentWeight += typeSetting.weight;

            if (randomValue <= currentWeight) return typeSetting.objectType;
        }

        return shopSettingsSO.inventoryObjectTypeSettings[0].objectType;
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    private bool HasReachedTypeCap(ShopSettingsSO shopSettingsSO, InventoryObjectType inventoryObjectType, List<InventoryObjectSO> inventoryObjectList)
    {
        InventoryObjectTypeSetting inventoryObjectTypeSetting = GetInventoryObjectTypeSettingByObjectType(shopSettingsSO, inventoryObjectType);

        if(inventoryObjectTypeSetting == null)
        {
            if (debug) Debug.Log("InventoryObjectTypeSetting is null. Can not define if type cap reached.");
            return false;
        }

        int accumulator = 0;

        foreach(InventoryObjectSO inventoryObjectSO in inventoryObjectList)
        {
            if (IsInventoryObjectOfType(inventoryObjectSO, inventoryObjectType)) accumulator += 1;
        }

        if (accumulator >= inventoryObjectTypeSetting.cap) return true;
        return false;
    }

    private bool HasReachedRarityCap(ShopSettingsSO shopSettingsSO, InventoryObjectRarity inventoryObjectRarity, List<InventoryObjectSO> inventoryObjectList)
    {
        InventoryObjectRaritySetting inventoryObjectRaritySetting = GetInventoryObjectRaritySettingByObjectRarity(shopSettingsSO, inventoryObjectRarity);

        if (inventoryObjectRaritySetting == null)
        {
            if (debug) Debug.Log("InventoryObjectRaritySetting is null. Can not define if rarity cap reached.");
            return false;
        }

        int accumulator = 0;

        foreach (InventoryObjectSO inventoryObjectSO in inventoryObjectList)
        {
            if (IsInventoryObjectOfRarity(inventoryObjectSO, inventoryObjectRarity)) accumulator += 1;
        }

        if (accumulator >= inventoryObjectRaritySetting.cap) return true;
        return false;
    }

    private InventoryObjectTypeSetting GetInventoryObjectTypeSettingByObjectType(ShopSettingsSO shopSettingsSO, InventoryObjectType inventoryObjectType)
    {
        foreach(InventoryObjectTypeSetting typeSetting in shopSettingsSO.inventoryObjectTypeSettings)
        {
            if(typeSetting.objectType == inventoryObjectType) return typeSetting;
        }

        if (debug) Debug.Log($"InventoryObjectTypeSetting with InventoryObjectType {inventoryObjectType} was not found. Proceding to return null.");
        return null;
    }

    private InventoryObjectRaritySetting GetInventoryObjectRaritySettingByObjectRarity(ShopSettingsSO shopSettingsSO, InventoryObjectRarity inventoryObjectRarity)
    {
        foreach (InventoryObjectRaritySetting raritySetting in shopSettingsSO.inventoryObjectRaritySettings)
        {
            if (raritySetting.objectRarity == inventoryObjectRarity) return raritySetting;
        }

        if (debug) Debug.Log($"InventoryObjectRaritySetting with InventoryObjectRarity {inventoryObjectRarity} was not found. Proceding to return null.");
        return null;
    }

    private bool IsInventoryObjectOfType(InventoryObjectSO inventoryObjectSO, InventoryObjectType inventoryObjectType)
    {
        if(inventoryObjectSO.GetInventoryObjectType() == inventoryObjectType) return true;
        return false;
    }

    private bool IsInventoryObjectOfRarity(InventoryObjectSO inventoryObjectSO, InventoryObjectRarity inventoryObjectRarity)
    {
        if (inventoryObjectSO.objectRarity == inventoryObjectRarity) return true;
        return false;
    }
}
