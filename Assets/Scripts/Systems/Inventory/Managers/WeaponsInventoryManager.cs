using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WeaponsInventoryManager : MonoBehaviour
{
    public static WeaponsInventoryManager Instance { get; private set; }

    [Header("Lists")]
    [SerializeField] private List<WeaponInventoryIdentified> weaponsInventory;

    [Header("Debug")]
    [SerializeField] private bool debug;

    public List<WeaponInventoryIdentified> WeaponsInventory => weaponsInventory;

    public static event EventHandler<OnWeaponSlotsEventArgs> OnWeaponSlotsInitialized;

    public static event EventHandler<OnWeaponsEventArgs> OnWeaponsInventoryInitialized;
    public static event EventHandler<OnWeaponEventArgs> OnWeaponAddedToInventory;
    public static event EventHandler<OnWeaponEventArgs> OnWeaponRemovedFromInventory;

    public class OnWeaponSlotsEventArgs : EventArgs
    {
        public int weaponSlots;
    }

    public class OnWeaponEventArgs : EventArgs
    {
        public WeaponInventoryIdentified weapon;
    }

    public class OnWeaponsEventArgs : EventArgs
    {
        public List<WeaponInventoryIdentified> weapons;
        public int capacity;
    }


    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }
    private void Awake()
    {
        SetSingleton();
    }

    private void Start()
    {
        SetWeaponSlotsFromCharacter();
        SetWeaponsInventoryFromCharacter();
        InitializeWeaponsInventory();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one WeaponsInventoryManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    private void InitializeWeaponsInventory()
    {
        OnWeaponsInventoryInitialized?.Invoke(this , new OnWeaponsEventArgs { weapons = weaponsInventory, capacity = GetWeaponsInventoryCapacity()});
    }

    public void AddWeaponToInventory(WeaponSO weaponSO)
    {
        if (WeaponsInventoryFull())
        {
            if(debug) Debug.Log("Weapon Inventory is full, addition will be ignored");
            return;
        }

        if (weaponSO == null)
        {
            if (debug) Debug.Log("WeaponSO is null, addition will be ignored");
            return;
        }

        string weaponGUID = GeneralDataUtilities.GenerateGUID();

        WeaponInventoryIdentified weaponToAdd = new WeaponInventoryIdentified { GUID = weaponGUID, weaponSO = weaponSO };

        weaponsInventory.Add(weaponToAdd);

        OnWeaponAddedToInventory?.Invoke(this, new OnWeaponEventArgs { weapon = weaponToAdd });
    }

    private void RemoveWeaponFromInventoryByWeaponSO(WeaponSO weaponSO)
    {
        if (weaponSO == null)
        {
            if (debug) Debug.Log("WeaponSO is null, remotion will be ignored");
            return;
        }

        WeaponInventoryIdentified weaponIdentified = FindWeaponByWeaponSO(weaponSO);

        if(weaponIdentified == null)
        {
            if (debug) Debug.Log("Could not find weapon by WeaponSO");
            return;
        }

        weaponsInventory.Remove(weaponIdentified);

        OnWeaponRemovedFromInventory?.Invoke(this, new OnWeaponEventArgs { weapon = weaponIdentified });
    }

    public void RemoveWeaponFromInventoryByGUID(string GUID)
    {
        WeaponInventoryIdentified weaponIdentified = FindWeaponByGUID(GUID);

        if (weaponIdentified == null)
        {
            if (debug) Debug.Log("Could not find weapon by GUID");
            return;
        }

        weaponsInventory.Remove(weaponIdentified);

        OnWeaponRemovedFromInventory?.Invoke(this, new OnWeaponEventArgs { weapon = weaponIdentified });
    }

    private WeaponInventoryIdentified FindWeaponByWeaponSO(WeaponSO weaponSO)
    {
        foreach(WeaponInventoryIdentified weapon in weaponsInventory)
        {
            if (weapon.weaponSO == weaponSO) return weapon;
        }

        if (debug) Debug.Log($"Weapon with WeaponSO with ID {weaponSO.id} could not be found. Proceding to return null");
        return null;
    }

    private WeaponInventoryIdentified FindWeaponByGUID(string GUID)
    {
        foreach (WeaponInventoryIdentified weapon in weaponsInventory)
        {
            if (weapon.GUID == GUID) return weapon;
        }

        if (debug) Debug.Log($"Weapon with GUID {GUID} could not be found. Proceding to return null");
        return null;
    }


    private void SetWeaponsInventoryFromCharacter()
    {
        ClearWeaponsInventory();
        AddWeaponsToInventory(PlayerIdentifier.Instance.CharacterSO.startingWeapons);
    }

    private void AddWeaponsToInventory(List<WeaponSO> weaponSOs)
    {
        foreach (WeaponSO weaponSO in weaponSOs)
        {
            AddWeaponToInventory(weaponSO);
        }
    }

    private void ClearWeaponsInventory() => weaponsInventory.Clear();

    public bool WeaponsInventoryFull() => weaponsInventory.Count >= PlayerIdentifier.Instance.CharacterSO.weaponSlots;
    public int GetWeaponsInventoryCapacity() => PlayerIdentifier.Instance.CharacterSO.weaponSlots;
    public int GetWeaponsInventoryCount() => weaponsInventory.Count;


    public bool HasOneOrLessWeapons() => weaponsInventory.Count <= 1;

    public bool WeaponInInventoryByWeaponSO(WeaponSO weaponSO)
    {
        foreach(WeaponInventoryIdentified weapon in weaponsInventory)
        {
            if(weapon.weaponSO == weaponSO) return true;    
        }

        return false;
    }

    private void SetWeaponSlotsFromCharacter()
    {
        OnWeaponSlotsInitialized?.Invoke(this, new OnWeaponSlotsEventArgs { weaponSlots = PlayerIdentifier.Instance.CharacterSO.weaponSlots });
    }
}

[System.Serializable]
public class WeaponInventoryIdentified
{
    public string GUID;
    public WeaponSO weaponSO;
}