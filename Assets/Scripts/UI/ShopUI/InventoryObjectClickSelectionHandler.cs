using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryObjectClickSelectionHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private InventoryObjectInventoryUI inventoryObjectInventoryUI;
    [Space]
    [SerializeField] private Button inventoryObjectButton;
    [SerializeField] private Transform selectionUITransform;

    private bool isSelected = false;

    public static event EventHandler<OnInventoryObjectEventArgs> OnInventoryObjectSelectionCommand;
    public static event EventHandler<OnInventoryObjectEventArgs> OnInventoryObjectDeselectionCommand;

    public class OnInventoryObjectEventArgs : EventArgs
    {
        public PrimitiveInventoryObject primitiveInventoryObject;
    }

    private void OnEnable()
    {
        ShopSellHandler.OnInventoryObjectSelected += ShopSellHandler_OnInventoryObjectSelected;
        ShopSellHandler.OnInventoryObjectDeselected += ShopSellHandler_OnInventoryObjectDeselected;

        ShopOpeningManager.OnShopOpen += ShopOpeningManager_OnShopOpen;
    }

    private void OnDisable()
    {
        ShopSellHandler.OnInventoryObjectSelected -= ShopSellHandler_OnInventoryObjectSelected;
        ShopSellHandler.OnInventoryObjectDeselected -= ShopSellHandler_OnInventoryObjectDeselected;

        ShopOpeningManager.OnShopOpen -= ShopOpeningManager_OnShopOpen;
    }

    private void Awake()
    {
        InitializeButtonsListeners();
    }

    private void Start()
    {
        Initialize();
    }

    private void InitializeButtonsListeners()
    {
        inventoryObjectButton.onClick.AddListener(HandleInventoryObjectSelection);
    }

    private void Initialize()
    {
        DeselectCommand();
        DisableSelectionUI();
        isSelected = false;
    }

    private void HandleInventoryObjectSelection()
    {
        if (!isSelected)
        {
            SelectCommand();
        }
        else
        {
            DeselectCommand();
        }
    }

    private void SelectCommand() => OnInventoryObjectSelectionCommand?.Invoke(this, new OnInventoryObjectEventArgs { primitiveInventoryObject = inventoryObjectInventoryUI.PrimitiveInventoryObject });
    private void DeselectCommand() => OnInventoryObjectDeselectionCommand?.Invoke(this, new OnInventoryObjectEventArgs { primitiveInventoryObject = inventoryObjectInventoryUI.PrimitiveInventoryObject });

    private void EnableSelectionUI() => selectionUITransform.gameObject.SetActive(true);
    private void DisableSelectionUI() => selectionUITransform.gameObject.SetActive(false);

    #region Subscriptions
    private void ShopOpeningManager_OnShopOpen(object sender, EventArgs e)
    {
        DeselectCommand();
    }

    private void ShopSellHandler_OnInventoryObjectSelected(object sender, ShopSellHandler.OnInventoryObjectSelectedEventArgs e)
    {
        if (e.inventoryObject == inventoryObjectInventoryUI.PrimitiveInventoryObject) //If another InventoryObject is selected, deselect this one
        {
            EnableSelectionUI();
            isSelected = true;
        }
        
    }

    private void ShopSellHandler_OnInventoryObjectDeselected(object sender, ShopSellHandler.OnInventoryObjectSelectedEventArgs e)
    {
        if(e.inventoryObject == inventoryObjectInventoryUI.PrimitiveInventoryObject)
        {
            DisableSelectionUI();
            isSelected = false;
        }
    }
    #endregion
}
