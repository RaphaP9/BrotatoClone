using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopObjectCardUI : MonoBehaviour
{
    [Header("Inventory Object")]
    [SerializeField] private InventoryObjectSO inventoryObjectSO;

    [Header("Components")]
    [SerializeField] private Button purchaseButton;

    public event EventHandler<OnInventoryObjectEventArgs> OnInventoryObjectSet;

    public class OnInventoryObjectEventArgs: EventArgs
    {
        public InventoryObjectSO inventoryObjectSO;
    }

    public void SetInventoryObject(InventoryObjectSO inventoryObjectSO)
    {
        this.inventoryObjectSO = inventoryObjectSO;
        OnInventoryObjectSet?.Invoke(this, new OnInventoryObjectEventArgs { inventoryObjectSO = inventoryObjectSO });
    }
}
