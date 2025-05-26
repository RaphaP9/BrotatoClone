using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShopGenerator : MonoBehaviour
{
    public static ShopGenerator Instance {  get; private set; }

    [Header("Components")]
    [SerializeField] private ShopSettingsSO shopSettingsSO;

    [Header("Debug")]
    [SerializeField] private bool debug; 

    private const InventoryObjectRarity DEFAULT_OBJECT_RARITY = InventoryObjectRarity.Common;
    private const InventoryObjectType DEFAULT_OBJECT_TYPE = InventoryObjectType.Object;

    private const int MAX_OBJECT_GENERATION_ITERATIONS = 20;

    private void Awake()
    {
        SetSingleton();
    }
    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one ShopGenerator instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    #region GetAvailableInventoryObjects
    private List<WeaponSO> GetShopAvailableWeaponsFromCompleteWeaponsList(ShopSettingsSO shopSettingsSO)
    {
        List<WeaponSO> validWeapons = new List<WeaponSO>();

        foreach (WeaponSO weapon in shopSettingsSO.weaponsPool)
        {
            validWeapons.Add(weapon);
        }

        return validWeapons;
    }

    private List<ObjectSO> GetShopAvailableObjectsFromCompleteObjectsList(ShopSettingsSO shopSettingsSO)
    {
        List<ObjectSO> validObjects = new List<ObjectSO>();

        foreach (ObjectSO @object in shopSettingsSO.objectsPool)
        {
            validObjects.Add(@object);
        }

        return validObjects;
    }

    private List<InventoryObjectSO> GetShopAvailableInventoryObjectsList(ShopSettingsSO shopSettingsSO)
    {
        List<InventoryObjectSO> validObjectsAsInventoryObjects = GetShopAvailableObjectsFromCompleteObjectsList(shopSettingsSO).Select(x => x as InventoryObjectSO).ToList();
        List<InventoryObjectSO> validWeaponsAsInventoryObjects = GetShopAvailableWeaponsFromCompleteWeaponsList(shopSettingsSO).Select(x => x as InventoryObjectSO).ToList();

        List<List<InventoryObjectSO>> unnappendedAvailableInventoryObjectsList = new List<List<InventoryObjectSO>> { validObjectsAsInventoryObjects, validWeaponsAsInventoryObjects};
        List<InventoryObjectSO> availableInventoryObjects = GeneralUtilities.AppendListsOfLists(unnappendedAvailableInventoryObjectsList);

        return availableInventoryObjects;
    }
    #endregion

    #region GenerateObjectsList
    public List<InventoryObjectSO> GenerateShopObjectsList()
    {
        List<InventoryObjectSO> availableInventoryObjectsList = GetShopAvailableInventoryObjectsList(shopSettingsSO);

        List<InventoryObjectSO> generatedList = new List<InventoryObjectSO>();

        for (int i = 0; i < shopSettingsSO.shopSize; i++)
        {
            InventoryObjectSO shopObject = GenerateShopObject(shopSettingsSO, availableInventoryObjectsList, generatedList);

            if(shopObject == null)
            {
                if (debug) Debug.Log("Shop object is null and will not be added to generated list. List will be short sized.");
                continue;
            }

            generatedList.Add(shopObject);
        }

        return generatedList;
    }

    private InventoryObjectSO GenerateShopObject(ShopSettingsSO shopSettingsSO, List<InventoryObjectSO> availableInventoryObjectsList, List<InventoryObjectSO> currentGeneratedList)
    {
        bool validObject = false;
        int iterations = 0;

        InventoryObjectSO selectedInventoryObject = null;

        while(!validObject && iterations< MAX_OBJECT_GENERATION_ITERATIONS)
        {
            iterations++;

            InventoryObjectType targetObjectType = GenerateInventoryObjectType(shopSettingsSO);
            InventoryObjectRarity targetObjectRarity = GenerateInventoryObjectRarity(shopSettingsSO);

            if (HasReachedTypeCap(shopSettingsSO, targetObjectType, currentGeneratedList)) continue;
            if (HasReachedRarityCap(shopSettingsSO, targetObjectRarity, currentGeneratedList)) continue;

            InventoryObjectSO foundInventoryObject = GetRandomInventoryObjectFromListByTypeAndRarity(availableInventoryObjectsList, targetObjectType, targetObjectRarity);

            if(foundInventoryObject != null)
            {
                selectedInventoryObject = foundInventoryObject;
                validObject = true;
            }
        }

        if(selectedInventoryObject == null) //In case all iterations failed to find a valid inventory object (respecting the caps limit), find a random unrestricted object
        {
            selectedInventoryObject = GetRandomInventoryObjectFromList(shopSettingsSO.randomBreakerInventoryObjectList);
        }

        if(selectedInventoryObject == null)
        {
            if (debug) Debug.Log("Could not find an inventory object.");
        }

        return selectedInventoryObject;
    }
    #endregion

    #region Generate Rarity&Type

    private InventoryObjectRarity GenerateInventoryObjectRarity(ShopSettingsSO shopSettingsSO)
    {
        int totalWeight = shopSettingsSO.inventoryObjectRaritySettings.Sum(x => x.weight);

        if (totalWeight <= 0) return DEFAULT_OBJECT_RARITY;

        System.Random random = new System.Random();
        int randomValue = random.Next(0, totalWeight) + 1;

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

        System.Random random = new System.Random();
        int randomValue = random.Next(0, totalWeight) +1;

        int currentWeight = 0;

        foreach (InventoryObjectTypeSetting typeSetting in shopSettingsSO.inventoryObjectTypeSettings)
        {
            currentWeight += typeSetting.weight;

            if (randomValue <= currentWeight) return typeSetting.objectType;
        }

        return shopSettingsSO.inventoryObjectTypeSettings[0].objectType;
    }
    #endregion

    #region CapReached
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

    #endregion

    #region GetInventoryObject From List with Type&Rarity
    private InventoryObjectSO GetRandomInventoryObjectFromListByTypeAndRarity(List<InventoryObjectSO> inventoryObjectList, InventoryObjectType targetObjectType, InventoryObjectRarity targetObjectRarity)
    {
        List<InventoryObjectSO> filteredInventoryObjectList = new List<InventoryObjectSO>();

        foreach (InventoryObjectSO inventoryObject in inventoryObjectList) //First on list that matches conditions(Rarity,Type) is returned
        {
            if (!IsInventoryObjectOfRarity(inventoryObject, targetObjectRarity)) continue;
            if (!IsInventoryObjectOfType(inventoryObject, targetObjectType)) continue;

            filteredInventoryObjectList.Add(inventoryObject);
        }

        if (filteredInventoryObjectList.Count <= 0)
        {
            //if (debug) Debug.Log($"No matching inventory objects found for the given Type: {targetObjectType} and Rarity: {targetObjectRarity}. Proceding to return null.");
            return null;
        }

        return GetRandomInventoryObjectFromList(filteredInventoryObjectList);
    }

    private InventoryObjectSO GetRandomInventoryObjectFromList(List<InventoryObjectSO> inventoryObjectList)
    {
        if(inventoryObjectList.Count <= 0)
        {
            if (debug) Debug.Log("List does not contain any elements. Proceding to return null");
            return null;    
        }

        System.Random random = new System.Random();

        InventoryObjectSO randomInventoryObject = inventoryObjectList[random.Next(inventoryObjectList.Count)];
        return randomInventoryObject;
    }
    #endregion

    #region Get Setting By Type&Rarity
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
    #endregion

    #region Check Type&Rarity
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
    #endregion
}
