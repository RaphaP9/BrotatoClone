using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WeaponsInventoryManager : MonoBehaviour
{
    public static WeaponsInventoryManager Instance { get; private set; }

    [Header("Lists")]
    [SerializeField] private List<WeaponSO> weaponsInventory;

    [Header("Debug")]
    [SerializeField] private bool debug;

    public List<WeaponSO> WeaponsInventory => weaponsInventory;

    public static event EventHandler<OnWeaponsEventArgs> OnWeaponsInventoryInitialized;
    public static event EventHandler<OnWeaponEventArgs> OnWeaponAddedToInventory;
    public static event EventHandler<OnWeaponEventArgs> OnWeaponRemovedFromInventory;

    public class OnWeaponsEventArgs : EventArgs
    {
        public List<WeaponSO> weaponSOs;
    }

    public class OnWeaponEventArgs : EventArgs
    {
        public WeaponSO weaponSO;
    }

    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }

    private void Start()
    {
        InitializeWeaponsInventory();
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
            Debug.LogWarning("There is more than one WeaponsInventoryManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    private void InitializeWeaponsInventory()
    {
        OnWeaponsInventoryInitialized?.Invoke(this , new OnWeaponsEventArgs { weaponSOs = weaponsInventory });
    }

    private void AddWeaponToInventory(WeaponSO weaponSO)
    {
        if (weaponSO == null)
        {
            if (debug) Debug.Log("WeaponSO is null, addition will be ignored");
            return;
        }

        if (weaponsInventory.Contains(weaponSO))
        {
            if (debug) Debug.Log($"WeaponSO with name {weaponSO.inventoryObjectName} is already on inventory, addition will be ignored");
            return;
        }

        weaponsInventory.Add(weaponSO);

        OnWeaponAddedToInventory?.Invoke(this, new OnWeaponEventArgs { weaponSO = weaponSO });
    }

    private void RemoveWeaponFromInventory(WeaponSO weaponSO)
    {
        if (weaponSO == null)
        {
            if (debug) Debug.Log("WeaponSO is null, remotion will be ignored");
            return;
        }

        if (!weaponsInventory.Contains(weaponSO))
        {
            if (debug) Debug.Log($"WeaponSO with name {weaponSO.inventoryObjectName} is not on inventory, remotion will be ignored");
            return;
        }

        weaponsInventory.Remove(weaponSO);

        OnWeaponRemovedFromInventory?.Invoke(this, new OnWeaponEventArgs { weaponSO = weaponSO });
    }
}