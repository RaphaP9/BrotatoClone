using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PlayerWeaponActivator : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private WeaponSO weaponSO;

    public event EventHandler<OnPlayerWeaponEventArgs> OnPlayerWeaponActivated;
    public event EventHandler<OnPlayerWeaponEventArgs> OnPlayerWeaponDeactivated;

    public class OnPlayerWeaponEventArgs : EventArgs
    {
        public WeaponSO weaponSO;
    }

    private void OnEnable()
    {
        WeaponsInventoryManager.OnWeaponAddedToInventory += WeaponsInventoryManager_OnWeaponAddedToInventory;
        WeaponsInventoryManager.OnWeaponRemovedFromInventory += WeaponsInventoryManager_OnWeaponRemovedFromInventory;
    }

    private void OnDisable()
    {
        WeaponsInventoryManager.OnWeaponAddedToInventory -= WeaponsInventoryManager_OnWeaponAddedToInventory;
        WeaponsInventoryManager.OnWeaponRemovedFromInventory -= WeaponsInventoryManager_OnWeaponRemovedFromInventory;
    }

    #region Subscriptions
    private void WeaponsInventoryManager_OnWeaponAddedToInventory(object sender, WeaponsInventoryManager.OnWeaponEventArgs e)
    {
        if (weaponSO != e.weaponSO) return;

        OnPlayerWeaponActivated?.Invoke(this, new OnPlayerWeaponEventArgs { weaponSO = weaponSO });
    }

    private void WeaponsInventoryManager_OnWeaponRemovedFromInventory(object sender, WeaponsInventoryManager.OnWeaponEventArgs e)
    {
        if (weaponSO != e.weaponSO) return;

        OnPlayerWeaponDeactivated?.Invoke(this, new OnPlayerWeaponEventArgs{ weaponSO = weaponSO});
    }
    #endregion
}
