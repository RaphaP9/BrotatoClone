using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WeaponsInventoryManager : MonoBehaviour
{
    public static WeaponsInventoryManager Instance { get; private set; }

    [Header("Lists")]
    [SerializeField] private List<WeaponIdentified> weaponsInventory;

    [Header("Debug")]
    [SerializeField] private bool debug;

    public List<WeaponIdentified> WeaponsInventory => weaponsInventory;

    public static event EventHandler<OnWeaponsEventArgs> OnWeaponsInventoryInitialized;
    public static event EventHandler<OnWeaponEventArgs> OnWeaponAddedToInventory;
    public static event EventHandler<OnWeaponEventArgs> OnWeaponRemovedFromInventory;

    public class OnWeaponsEventArgs : EventArgs
    {
        public List<WeaponIdentified> weapons;
    }

    public class OnWeaponEventArgs : EventArgs
    {
        public WeaponIdentified weapon;
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
        SetWeaponInventoryFromCharacter();
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
        OnWeaponsInventoryInitialized?.Invoke(this , new OnWeaponsEventArgs { weapons = weaponsInventory });
    }

    private void AddWeaponToInventory(WeaponSO weaponSO)
    {
        if (weaponSO == null)
        {
            if (debug) Debug.Log("WeaponSO is null, addition will be ignored");
            return;
        }

        string weaponGUID = GeneralDataUtilities.GenerateGUID();

        WeaponIdentified weaponToAdd = new WeaponIdentified { GUID = weaponGUID, weaponSO = weaponSO };

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

        WeaponIdentified weaponIdentified = FindWeaponByWeaponSO(weaponSO);

        if(weaponIdentified == null)
        {
            if (debug) Debug.Log("Could not find weapon by weaponSO");
            return;
        }

        weaponsInventory.Remove(weaponIdentified);

        OnWeaponRemovedFromInventory?.Invoke(this, new OnWeaponEventArgs { weapon = weaponIdentified });
    }

    private void RemoveWeaponFromInventoryByGUID(string GUID)
    {
        WeaponIdentified weaponIdentified = FindWeaponByGUID(GUID);

        if (weaponIdentified == null)
        {
            if (debug) Debug.Log("Could not find weapon by GUID");
            return;
        }

        weaponsInventory.Remove(weaponIdentified);

        OnWeaponRemovedFromInventory?.Invoke(this, new OnWeaponEventArgs { weapon = weaponIdentified });
    }

    private WeaponIdentified FindWeaponByWeaponSO(WeaponSO weaponSO)
    {
        foreach(WeaponIdentified weapon in weaponsInventory)
        {
            if (weapon.weaponSO == weaponSO) return weapon;
        }

        if (debug) Debug.Log($"Weapon with WeaponSO with ID {weaponSO.id} could not be found. Proceding to return null");
        return null;
    }

    private WeaponIdentified FindWeaponByGUID(string GUID)
    {
        foreach (WeaponIdentified weapon in weaponsInventory)
        {
            if (weapon.GUID == GUID) return weapon;
        }

        if (debug) Debug.Log($"Weapon with GUID {GUID} could not be found. Proceding to return null");
        return null;
    }


    private void SetWeaponInventoryFromCharacter()
    {
        ClearWeaponInventory();
        AddWeaponsToInventory(PlayerIdentifier.Instance.CharacterSO.startingWeapons);
    }

    private void AddWeaponsToInventory(List<WeaponSO> weaponSOs)
    {
        foreach (WeaponSO weaponSO in weaponSOs)
        {
            AddWeaponToInventory(weaponSO);
        }
    }

    private void ClearWeaponInventory() => weaponsInventory.Clear();

    public bool WeaponInventoryFull() => weaponsInventory.Count >= PlayerWeaponHandler.Instance.GetPointWeaponSlotsCount();
}

[System.Serializable]
public class WeaponIdentified
{
    public string GUID;
    public WeaponSO weaponSO;
}