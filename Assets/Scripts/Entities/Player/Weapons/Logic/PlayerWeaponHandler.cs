using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerWeaponHandler : MonoBehaviour
{
    [Header("Lists")]
    [SerializeField] private List<PointWeaponSlot> pointWeaponSlots;

    [Header("Debug")]
    [SerializeField] private bool debug;

    public List<PointWeaponSlot> PointWeaponsSlots => pointWeaponSlots;

    [System.Serializable]
    public class PointWeaponSlot
    {
        public Transform point;
        public WeaponHandler weaponHandler;
    }

    private void OnEnable()
    {
        WeaponsInventoryManager.OnWeaponsInventoryInitialized += WeaponsInventoryManager_OnWeaponsInventoryInitialized;
        WeaponsInventoryManager.OnWeaponAddedToInventory += WeaponsInventoryManager_OnWeaponAddedToInventory;
        WeaponsInventoryManager.OnWeaponRemovedFromInventory += WeaponsInventoryManager_OnWeaponRemovedFromInventory;
    }

    private void OnDisable()
    {
        WeaponsInventoryManager.OnWeaponsInventoryInitialized -= WeaponsInventoryManager_OnWeaponsInventoryInitialized;
        WeaponsInventoryManager.OnWeaponAddedToInventory -= WeaponsInventoryManager_OnWeaponAddedToInventory;
        WeaponsInventoryManager.OnWeaponRemovedFromInventory -= WeaponsInventoryManager_OnWeaponRemovedFromInventory;
    }

    private void CreateWeapons(List<WeaponSO> weaponSOs)
    {
        foreach(WeaponSO weaponSO in weaponSOs)
        {
            CreateWeapon(weaponSO);
        }
    }

    private void CreateWeapon(WeaponSO weaponSO)
    {
        PointWeaponSlot pointWeaponSlot = FindNextAvailablePointWeaponSlot();

        if (pointWeaponSlot == null)
        {
            if (debug) Debug.Log("PointWeaponSlot is null, creation will be ignored.");
            return;
        }

        CreateWeaponAtPoint(pointWeaponSlot, weaponSO);
    }

    private void CreateWeaponAtPoint(PointWeaponSlot pointWeaponSlot, WeaponSO weaponSO)
    {
        if(pointWeaponSlot.weaponHandler != null)
        {
            if (debug) Debug.Log("PointWeaponSlot already contains a weapon. Creation will be ignored");
            return;
        }

        Transform weaponTransform = Instantiate(weaponSO.weaponTransform,pointWeaponSlot.point);
        WeaponHandler weaponHandler = weaponTransform.GetComponent<WeaponHandler>();

        if(weaponHandler == null)
        {
            if (debug) Debug.Log("Instantiated WeaponTranform does not have a WeaponHandler component. Assignation will be ignored");
            return;
        }

        pointWeaponSlot.weaponHandler = weaponHandler;
    }

    private void RemoveWeapon(WeaponSO weaponSO)
    {
        foreach(PointWeaponSlot pointWeaponSlot in pointWeaponSlots)
        {
            if (pointWeaponSlot.weaponHandler == null) continue;
            if(pointWeaponSlot.weaponHandler.WeaponSO == weaponSO)
            {
                ClearPointWeaponSlot(pointWeaponSlot);
                break;
            }
        }

        if (debug) Debug.Log($"No pointWeaponSlot has weaponSO with name {weaponSO.inventoryObjectName}, remotion will be ignored");
    }

    private PointWeaponSlot FindNextAvailablePointWeaponSlot()
    {
        foreach(PointWeaponSlot pointWeaponSlot in pointWeaponSlots)
        {
            if(pointWeaponSlot.weaponHandler == null) return pointWeaponSlot;
        }

        if (debug) Debug.Log("Could not find an empty pointWeaponSlot, proceding to return null.");

        return null;
    }

    private void ClearPointWeaponSlot(PointWeaponSlot pointWeaponSlot)
    {
        if(pointWeaponSlot.weaponHandler == null) return;
        
        Destroy(pointWeaponSlot.weaponHandler.gameObject);
        pointWeaponSlot.weaponHandler = null;
        
    }

    #region Subscriptions
    private void WeaponsInventoryManager_OnWeaponsInventoryInitialized(object sender, WeaponsInventoryManager.OnWeaponsEventArgs e)
    {
        CreateWeapons(e.weaponSOs);
    }

    private void WeaponsInventoryManager_OnWeaponAddedToInventory(object sender, WeaponsInventoryManager.OnWeaponEventArgs e)
    {
        CreateWeapon(e.weaponSO);
    }

    private void WeaponsInventoryManager_OnWeaponRemovedFromInventory(object sender, WeaponsInventoryManager.OnWeaponEventArgs e)
    {
        
    }
    #endregion
}
