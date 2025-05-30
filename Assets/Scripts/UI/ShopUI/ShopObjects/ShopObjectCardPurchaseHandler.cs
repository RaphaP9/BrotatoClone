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
    public static event EventHandler<OnShopObjectPurchaseEventArgs> OnAnyShopObjectPurchaseDenied;

    public event EventHandler<OnShopObjectPurchaseEventArgs> OnShopObjectPurchase;
    public event EventHandler<OnShopObjectPurchaseEventArgs> OnShopObjectPurchaseDenied;

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
            DenyPurchase(inventoryObjectSO);
            return;
        }

        Purchase(inventoryObjectSO);
    }

    private void Purchase(InventoryObjectSO inventoryObjectSO)
    {
        GoldManager.Instance.SpendGold(inventoryObjectSO.price);

        OnAnyShopObjectPurchase?.Invoke(this, new OnShopObjectPurchaseEventArgs { inventoryObjectSO = inventoryObjectSO });
        OnShopObjectPurchase?.Invoke(this, new OnShopObjectPurchaseEventArgs { inventoryObjectSO = inventoryObjectSO });
    }

    private void DenyPurchase(InventoryObjectSO inventoryObjectSO)
    {
        OnAnyShopObjectPurchaseDenied?.Invoke(this, new OnShopObjectPurchaseEventArgs { inventoryObjectSO = inventoryObjectSO });
        OnShopObjectPurchaseDenied?.Invoke(this, new OnShopObjectPurchaseEventArgs { inventoryObjectSO = inventoryObjectSO });
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
