using System.Collections;
using System.Collections.Generic;
using UnityEditor.Playables;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private ShopSettingsSO shopSettingsSO;

    [Header("Lists")]
    [SerializeField] private List<WeaponSO> completeWeaponsList;
    [SerializeField] private List<ObjectSO> completeObjectsList;
    [SerializeField] private List<AbilitySO> completeAbilitiesList;
    [SerializeField] private List<ElementSO> completeElementsList;


    private List<WeaponSO> GetShopAvailableWeaponsFromCompleteWeaponsList()
    {
        List<WeaponSO> validWeapons = new List<WeaponSO>();

        foreach (WeaponSO weapon in completeWeaponsList)
        {
            if (ElementsInventoryManager.Instance.ElementsInInventoryByElementSO(weapon.requiredElements)) validWeapons.Add(weapon);
        }

        return validWeapons;
    }

    private List<ObjectSO> GetShopAvailableObjectsFromCompleteObjectsList()
    {
        List<ObjectSO> validObjects = new List<ObjectSO>();

        foreach (ObjectSO @object in completeObjectsList)
        {
            if (ElementsInventoryManager.Instance.ElementsInInventoryByElementSO(@object.requiredElements)) validObjects.Add(@object);
        }

        return validObjects;
    }

    private List<AbilitySO> GetShopAvailableAbilitiesFromCompleteAbilitiesList()
    {
        List<AbilitySO> validAbilities = new List<AbilitySO>();

        foreach (AbilitySO ability in completeAbilitiesList)
        {
            if (AbilitiesInventoryManager.Instance.AbilityInInventoryByAbilitySO(ability)) continue; //Unique Abilities
            if (ElementsInventoryManager.Instance.ElementsInInventoryByElementSO(ability.requiredElements)) validAbilities.Add(ability);
        }

        return validAbilities;
    }

    private List<ElementSO> GetShopAvailableElementsFromCompleteElementsList()
    {
        List<ElementSO> validElements = new List<ElementSO>();

        foreach (ElementSO element in completeElementsList)
        {
            if (ElementsInventoryManager.Instance.ElementInInventoryByElementSO(element)) continue; //Unique Elements
            if (ElementsInventoryManager.Instance.ElementsInInventoryByElementSO(element.requiredElements)) validElements.Add(element);
        }

        return validElements;
    }
}
