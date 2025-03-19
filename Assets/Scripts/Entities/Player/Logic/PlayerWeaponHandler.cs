using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerWeaponHandler : MonoBehaviour
{
    public static PlayerWeaponHandler Instance { get; private set; }

    [Header("Lists")]
    [SerializeField] private List<PointWeaponSlot> pointWeaponSlots;

    [Header("Debug")]
    [SerializeField] private bool debug;

    public List<PointWeaponSlot> PointWeaponsSlots => pointWeaponSlots;

    [System.Serializable]
    public class PointWeaponSlot
    {
        public Transform point;
        public WeaponIdentifier weaponIdentifier;
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
            Debug.LogWarning("There is more than one PlayerWeaponHandler instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
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
        if(pointWeaponSlot.weaponIdentifier != null)
        {
            if (debug) Debug.Log("PointWeaponSlot already contains a weapon. Creation will be ignored");
            return;
        }

        Transform weaponTransform = Instantiate(weaponSO.weaponTransform,pointWeaponSlot.point);
        WeaponIdentifier weaponIdentifier = weaponTransform.GetComponent<WeaponIdentifier>();

        if(weaponIdentifier == null)
        {
            if (debug) Debug.Log("Instantiated WeaponTranform does not have a WeaponIdentifier component. Assignation will be ignored");
            return;
        }

        pointWeaponSlot.weaponIdentifier = weaponIdentifier;
    }

    private void RemoveWeapon(WeaponSO weaponSO)
    {
        foreach(PointWeaponSlot pointWeaponSlot in pointWeaponSlots)
        {
            if (pointWeaponSlot.weaponIdentifier == null) continue;
            if(pointWeaponSlot.weaponIdentifier.WeaponSO == weaponSO)
            {
                ClearPointWeaponSlot(pointWeaponSlot);
                break;
            }
        }

        if (debug) Debug.Log($"No pointWeaponSlot has WeaponIdentifier.WeaponSO with name {weaponSO.inventoryObjectName}, remotion will be ignored");
    }

    private PointWeaponSlot FindNextAvailablePointWeaponSlot()
    {
        foreach(PointWeaponSlot pointWeaponSlot in pointWeaponSlots)
        {
            if(pointWeaponSlot.weaponIdentifier == null) return pointWeaponSlot;
        }

        if (debug) Debug.Log("Could not find an empty pointWeaponSlot, proceding to return null.");

        return null;
    }

    private void ClearPointWeaponSlot(PointWeaponSlot pointWeaponSlot)
    {
        if(pointWeaponSlot.weaponIdentifier == null) return;
        
        Destroy(pointWeaponSlot.weaponIdentifier.gameObject);
        pointWeaponSlot.weaponIdentifier = null;
        
    }

    public int GetPointWeaponSlotsCount() => pointWeaponSlots.Count;

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
