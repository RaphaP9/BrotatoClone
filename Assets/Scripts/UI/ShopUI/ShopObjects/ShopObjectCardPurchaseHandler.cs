using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopObjectCardPurchaseHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private ShopObjectCardUI shopObjectCardUI;
    [SerializeField] private Button purchaseButton;

    public static event EventHandler<OnShopObjectPurchaseEventArgs> OnAnyShopObjectPurchase;
    public static event EventHandler<OnShopObjectPurchaseEventArgs> OnAnyShopObjectPurchaseDeniedByGold;
    public static event EventHandler<OnShopObjectPurchaseEventArgs> OnAnyShopObjectPurchaseDeniedByObjectsInventory;
    public static event EventHandler<OnShopObjectPurchaseEventArgs> OnAnyShopObjectPurchaseDeniedByWeaponsInventory;

    public event EventHandler<OnShopObjectPurchaseEventArgs> OnShopObjectPurchase;
    public event EventHandler<OnShopObjectPurchaseEventArgs> OnShopObjectPurchaseDeniedByGold;
    public event EventHandler<OnShopObjectPurchaseEventArgs> OnShopObjectPurchaseDeniedByObjectsInventory;
    public event EventHandler<OnShopObjectPurchaseEventArgs> OnShopObjectPurchaseDeniedByWeaponsInventory;

    public class OnShopObjectPurchaseEventArgs : EventArgs
    {
        public InventoryObjectSO inventoryObjectSO;
    }

    private void OnEnable()
    {
        shopObjectCardUI.OnInventoryObjectSet += ShopObjectCardUI_OnInventoryObjectSet;
    }

    private void OnDisable()
    {
        shopObjectCardUI.OnInventoryObjectSet -= ShopObjectCardUI_OnInventoryObjectSet;
    }

    private void HandlePurchase(InventoryObjectSO inventoryObjectSO)
    {
        if (!GoldManager.Instance.CanSpendGold(inventoryObjectSO.price))
        {
            DenyPurchaseByGold(inventoryObjectSO);
            return;
        }

        if(inventoryObjectSO.GetInventoryObjectType() == InventoryObjectType.Object)
        {
            if (ObjectsInventoryManager.Instance.ObjectsInventoryFull())
            {
                DenyPurchaseByObjectsInventory(inventoryObjectSO);
                return;
            }
        }

        if (inventoryObjectSO.GetInventoryObjectType() == InventoryObjectType.Weapon)
        {
            if (WeaponsInventoryManager.Instance.WeaponsInventoryFull())
            {
                DenyPurchaseByWeaponsInventory(inventoryObjectSO);
                return;
            }
        }

        Purchase(inventoryObjectSO);
    }

    private void Purchase(InventoryObjectSO inventoryObjectSO)
    {
        GoldManager.Instance.SpendGold(inventoryObjectSO.price);

        OnAnyShopObjectPurchase?.Invoke(this, new OnShopObjectPurchaseEventArgs { inventoryObjectSO = inventoryObjectSO });
        OnShopObjectPurchase?.Invoke(this, new OnShopObjectPurchaseEventArgs { inventoryObjectSO = inventoryObjectSO });
    }

    private void DenyPurchaseByGold(InventoryObjectSO inventoryObjectSO)
    {
        OnAnyShopObjectPurchaseDeniedByGold?.Invoke(this, new OnShopObjectPurchaseEventArgs { inventoryObjectSO = inventoryObjectSO });
        OnShopObjectPurchaseDeniedByGold?.Invoke(this, new OnShopObjectPurchaseEventArgs { inventoryObjectSO = inventoryObjectSO });
    }

    private void DenyPurchaseByObjectsInventory(InventoryObjectSO inventoryObjectSO)
    {
        OnAnyShopObjectPurchaseDeniedByObjectsInventory?.Invoke(this, new OnShopObjectPurchaseEventArgs { inventoryObjectSO = inventoryObjectSO });
        OnShopObjectPurchaseDeniedByObjectsInventory?.Invoke(this, new OnShopObjectPurchaseEventArgs { inventoryObjectSO = inventoryObjectSO });
    }

    private void DenyPurchaseByWeaponsInventory(InventoryObjectSO inventoryObjectSO)
    {
        OnAnyShopObjectPurchaseDeniedByWeaponsInventory?.Invoke(this, new OnShopObjectPurchaseEventArgs { inventoryObjectSO = inventoryObjectSO });
        OnShopObjectPurchaseDeniedByWeaponsInventory?.Invoke(this, new OnShopObjectPurchaseEventArgs { inventoryObjectSO = inventoryObjectSO });
    }

    private void UpdateButtonListener(InventoryObjectSO inventoryObjectSO)
    {
        purchaseButton.onClick.RemoveAllListeners();
        purchaseButton.onClick.AddListener(() => HandlePurchase(inventoryObjectSO));
    }

    private void ShopObjectCardUI_OnInventoryObjectSet(object sender, ShopObjectCardUI.OnInventoryObjectEventArgs e)
    {
        UpdateButtonListener(e.inventoryObjectSO);
    }
}
