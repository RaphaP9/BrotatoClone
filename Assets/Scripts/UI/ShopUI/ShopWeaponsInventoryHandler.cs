using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopWeaponsInventoryHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform inventoryWeaponsContainer;
    [SerializeField] private Transform weaponUIPrefab;
    [SerializeField] private Transform emptySlotPrefab;

    [Header("Debugg")]
    [SerializeField] private bool debug;

    private void OnEnable()
    {
        WeaponsInventoryManager.OnWeaponsInventoryInitialized += ObjectsInventoryManager_OnObjectsInventoryInitialized;
        WeaponsInventoryManager.OnWeaponAddedToInventory += ObjectsInventoryManager_OnObjectAddedToInventory;
        WeaponsInventoryManager.OnWeaponRemovedFromInventory += ObjectsInventoryManager_OnObjectRemovedFromInventory;
    }

    private void OnDisable()
    {
        WeaponsInventoryManager.OnWeaponsInventoryInitialized -= ObjectsInventoryManager_OnObjectsInventoryInitialized;
        WeaponsInventoryManager.OnWeaponAddedToInventory -= ObjectsInventoryManager_OnObjectAddedToInventory;
        WeaponsInventoryManager.OnWeaponRemovedFromInventory -= ObjectsInventoryManager_OnObjectRemovedFromInventory;
    }

    private void UpdateUI()
    {
        ClearContainer();

        int emptySlots = WeaponsInventoryManager.Instance.GetWeaponsInventoryCapacity();

        foreach (WeaponInventoryIdentified weaponInventoryIdentified in WeaponsInventoryManager.Instance.WeaponsInventory)
        {
            CreateWeaponUI(weaponInventoryIdentified);
            emptySlots--;
        }

        for (int i = 0; i < emptySlots; i++)
        {
            CreateEmptySlot();
        }
    }


    private void ClearContainer()
    {
        foreach (Transform child in inventoryWeaponsContainer)
        {
            Destroy(child.gameObject);
        }
    }

    private void CreateWeaponUI(WeaponInventoryIdentified weaponInventoryIdentified)
    {
        Transform weaponUITransform = Instantiate(weaponUIPrefab, inventoryWeaponsContainer);

        WeaponInventoryUI weaponInventoryUI = weaponUITransform.GetComponent<WeaponInventoryUI>();

        if (weaponInventoryUI == null)
        {
            if (debug) Debug.Log("Instantiated prefab does not contain an WeaponInventoryUI component. Set will be ignored");
            return;
        }

        weaponInventoryUI.SetWeaponInventory(weaponInventoryIdentified);
    }

    private void CreateEmptySlot()
    {
        Transform emptySlotUI = Instantiate(emptySlotPrefab, inventoryWeaponsContainer);
    }


    private void ObjectsInventoryManager_OnObjectsInventoryInitialized(object sender, WeaponsInventoryManager.OnWeaponsEventArgs e)
    {
        UpdateUI();
    }

    private void ObjectsInventoryManager_OnObjectAddedToInventory(object sender, WeaponsInventoryManager.OnWeaponEventArgs e)
    {
        UpdateUI();
    }

    private void ObjectsInventoryManager_OnObjectRemovedFromInventory(object sender, WeaponsInventoryManager.OnWeaponEventArgs e)
    {
        UpdateUI();
    }
}
