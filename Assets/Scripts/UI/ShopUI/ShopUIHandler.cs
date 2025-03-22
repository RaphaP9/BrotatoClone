using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class ShopUIHandler : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private Transform shopInventoryObjectPrefab;
    [Space]
    [SerializeField] private Button rerollButton;
    [SerializeField] private Toggle lockShopToggle;

    public static event EventHandler OnRerollClick;
    public static event EventHandler<OnLockShopToggledEventArgs> OnLockShopToggled;

    public class OnLockShopToggledEventArgs : EventArgs
    {
        public bool isOn;
    }

    private void OnEnable()
    {
        ShopManager.OnShopItemsGenerated += ShopManager_OnNewShopItemsGenerated;
        ShopManager.OnRerollCostSet += ShopManager_OnRerollCostSet;
    }

    private void OnDisable()
    {
        ShopManager.OnShopItemsGenerated -= ShopManager_OnNewShopItemsGenerated;
        ShopManager.OnRerollCostSet -= ShopManager_OnRerollCostSet;
    }

    private void Awake()
    {
        InitializeLockShopToggle();
        InitializeListeners();
    }

    private void InitializeLockShopToggle()
    {
        lockShopToggle.isOn = false;
    }

    private void InitializeListeners()
    {
        rerollButton.onClick.AddListener(Reroll);
        lockShopToggle.onValueChanged.AddListener(ToggleShopLock);
    }

    private void Reroll()
    {
        OnRerollClick?.Invoke(this, EventArgs.Empty);
    }

    private void ToggleShopLock(bool isOn)
    {
        OnLockShopToggled?.Invoke(this, new OnLockShopToggledEventArgs{isOn = isOn});
    }

    #region Subscriptions

    private void ShopManager_OnRerollCostSet(object sender, ShopManager.OnRerollCostEventArgs e)
    {
        Debug.Log($"Reroll cost: {e.rerollCost}");
    }

    private void ShopManager_OnNewShopItemsGenerated(object sender, ShopManager.OnShopItemsEventArgs e)
    {
        foreach(InventoryObjectSO i in e.inventoryObjectSOs)
        {
            Debug.Log(i.inventoryObjectName);
        }
    }

    #endregion
}
