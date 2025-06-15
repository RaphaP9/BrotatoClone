using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInventoryUI : InventoryObjectInventoryUI
{
    [Header("Runtime Filled")]
    [SerializeField] private ObjectInventoryIdentified objectInventoryIdentified;

    public event EventHandler<OnObjectInventorySetEventArgs> OnObjectInventorySet;
    public class OnObjectInventorySetEventArgs : EventArgs
    {
        public ObjectInventoryIdentified objectInventoryIdentified;
    }

    public void SetObjectInventory(ObjectInventoryIdentified objectInventoryIdentified)
    {
        this.objectInventoryIdentified = objectInventoryIdentified;
        OnObjectInventorySet?.Invoke(this, new OnObjectInventorySetEventArgs { objectInventoryIdentified = objectInventoryIdentified });

        SetPrimitiveInventoryObject(objectInventoryIdentified.GUID, objectInventoryIdentified.objectSO);
    }
}
