using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [Header("Lists")]
    [SerializeField] private List<WeaponSO> completeWeaponsList;
    [SerializeField] private List<ObjectSO> completeObjectsList;
    [SerializeField] private List<AbilitySO> completeAbilitiesList;
    [SerializeField] private List<ElementSO> completeElementsList;


    private List<WeaponSO> GetValidWeaponsFromCompleteWeaponsList()
    {
        List<WeaponSO> validWeapons = new List<WeaponSO>();

        foreach (WeaponSO weapon in completeWeaponsList)
        {
            if (ElementsInventoryManager.Instance.ElementsInInventoryByElementSO(weapon.requiredElements)) validWeapons.Add(weapon);
        }

        return validWeapons;
    }

    private List<ObjectSO> GetValidObjectsFromCompleteObjectsList()
    {
        List<ObjectSO> validObjects = new List<ObjectSO>();

        foreach (ObjectSO @object in completeObjectsList)
        {
            if (ElementsInventoryManager.Instance.ElementsInInventoryByElementSO(@object.requiredElements)) validObjects.Add(@object);
        }

        return validObjects;
    }

    private List<AbilitySO> GetValidAbilitiesFromCompleteAbilitiesList()
    {
        List<AbilitySO> validAbilities = new List<AbilitySO>();

        foreach (AbilitySO ability in completeAbilitiesList)
        {
            if (ElementsInventoryManager.Instance.ElementsInInventoryByElementSO(ability.requiredElements)) validAbilities.Add(ability);
        }

        return validAbilities;
    }

    private List<ElementSO> GetValidElementsFromCompleteElementsList()
    {
        List<ElementSO> validElements = new List<ElementSO>();

        foreach (ElementSO element in completeElementsList)
        {
            if (ElementsInventoryManager.Instance.ElementsInInventoryByElementSO(element.requiredElements)) validElements.Add(element);
        }

        return validElements;
    }
}
