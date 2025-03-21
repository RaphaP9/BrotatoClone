using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using System.Linq;

public class PlayerWeaponsHandler : MonoBehaviour
{
    public static PlayerWeaponsHandler Instance { get; private set; }

    [Header("Lists - RuntimeFilled")]
    [SerializeField] private List<PointWeaponSlot> pointWeaponSlots;

    [Header("Components")]
    [SerializeField] private Transform weaponSlotTransformPrefab;

    [Header("Settings")]
    [SerializeField, Range(0.75f,1.5f)] private float weaponSlotsDistanceToPlayer = 1f;

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
        WeaponsInventoryManager.OnWeaponSlotsInitialized += WeaponsInventoryManager_OnWeaponSlotsInitialized;

        WeaponsInventoryManager.OnWeaponsInventoryInitialized += WeaponsInventoryManager_OnWeaponsInventoryInitialized;
        WeaponsInventoryManager.OnWeaponAddedToInventory += WeaponsInventoryManager_OnWeaponAddedToInventory;
        WeaponsInventoryManager.OnWeaponRemovedFromInventory += WeaponsInventoryManager_OnWeaponRemovedFromInventory;
    }

    private void OnDisable()
    {
        WeaponsInventoryManager.OnWeaponSlotsInitialized -= WeaponsInventoryManager_OnWeaponSlotsInitialized;

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

            if (pointWeaponSlot.weaponIdentifier.WeaponSO == weaponSO)
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

    private void CreateWeaponSlots(int slots)
    {
        float angleDifference = 360f / slots;

        for(int i = 0; i < slots; i++)
        {
            float slotAngle = i* angleDifference;
            CreateWeaponSlot(slotAngle);    
        }
    }

    private void CreateWeaponSlot(float angle)
    {
        Vector2 normalizedVectorPosition = GeneralUtilities.GetAngleDegreesVector2(angle);
        Vector2 localPosition = normalizedVectorPosition * weaponSlotsDistanceToPlayer;

        Transform weaponSlotTransform = Instantiate(weaponSlotTransformPrefab, transform);
        weaponSlotTransform.localPosition = localPosition;

        PointWeaponSlot pointWeaponSlot = new PointWeaponSlot { point = weaponSlotTransform, weaponIdentifier = null };
        pointWeaponSlots.Add(pointWeaponSlot);
    }


    public int GetPointWeaponSlotsCount() => pointWeaponSlots.Count;

    #region Subscriptions

    private void WeaponsInventoryManager_OnWeaponSlotsInitialized(object sender, WeaponsInventoryManager.OnWeaponSlotsEventArgs e)
    {
        CreateWeaponSlots(e.weaponSlots);
    }
    private void WeaponsInventoryManager_OnWeaponsInventoryInitialized(object sender, WeaponsInventoryManager.OnWeaponsEventArgs e)
    {
        
    }

    private void WeaponsInventoryManager_OnWeaponAddedToInventory(object sender, WeaponsInventoryManager.OnWeaponEventArgs e)
    {
        CreateWeapon(e.weapon.weaponSO);
    }

    private void WeaponsInventoryManager_OnWeaponRemovedFromInventory(object sender, WeaponsInventoryManager.OnWeaponEventArgs e)
    {
        RemoveWeapon(e.weapon.weaponSO);
    }
    #endregion
}
