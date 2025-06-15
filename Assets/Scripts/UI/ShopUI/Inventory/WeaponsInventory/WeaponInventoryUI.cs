using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInventoryUI : InventoryObjectInventoryUI
{
    [Header("Runtime Filled")]
    [SerializeField] private WeaponInventoryIdentified weaponInventoryIdentified;

    public event EventHandler<OnWeaponInventorySetEventArgs> OnWeaponInventorySet;
    public class OnWeaponInventorySetEventArgs : EventArgs
    {
        public WeaponInventoryIdentified weaponInventoryIdentified;
    }

    public void SetWeaponInventory(WeaponInventoryIdentified weaponInventoryIdentified)
    {
        this.weaponInventoryIdentified = weaponInventoryIdentified;
        OnWeaponInventorySet?.Invoke(this, new OnWeaponInventorySetEventArgs { weaponInventoryIdentified = weaponInventoryIdentified });

        SetPrimitiveInventoryObject(weaponInventoryIdentified.GUID, weaponInventoryIdentified.weaponSO);
    }
}
