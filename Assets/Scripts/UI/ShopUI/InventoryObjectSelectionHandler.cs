using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryObjectSelectionHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private InventoryObjectInventoryUI inventoryObjectInventoryUI;
    [Space]
    [SerializeField] private Button inventoryObjectButton;
    [SerializeField] private Transform selectionUITransform;

    private bool isSelected = false;

    public static event EventHandler<OnInventoryObjectEventArgs> OnInventoryObjectSelection;
    public static event EventHandler<OnInventoryObjectEventArgs> OnInventoryObjectDeselection;

    public class OnInventoryObjectEventArgs : EventArgs
    {
        public PrimitiveInventoryObject primitiveInventoryObject;
    }


    private void OnEnable()
    {
        ShopOpeningManager.OnShopOpen += ShopOpeningManager_OnShopOpen;
        OnInventoryObjectSelection += InventoryObjectSelectionHandler_OnInventoryObjectSelection;
    }

    private void OnDisable()
    {
        ShopOpeningManager.OnShopOpen -= ShopOpeningManager_OnShopOpen;
        OnInventoryObjectSelection -= InventoryObjectSelectionHandler_OnInventoryObjectSelection;
    }

    private void Awake()
    {
        InitializeButtonsListeners();
    }

    private void Start()
    {
        DeselectInventoryObject();
    }

    private void InitializeButtonsListeners()
    {
        inventoryObjectButton.onClick.AddListener(HandleInventoryObjectSelection);
    }

    private void HandleInventoryObjectSelection()
    {
        if (!isSelected)
        {
            SelectInventoryObject();
        }
        else
        {
            DeselectInventoryObject();
        }
    }

    private void SelectInventoryObject()
    {
        EnableSelectionUI();
        OnInventoryObjectSelection?.Invoke(this, new OnInventoryObjectEventArgs { primitiveInventoryObject = inventoryObjectInventoryUI.PrimitiveInventoryObject });
        
        isSelected = true;
    }

    private void DeselectInventoryObject()
    {
        DisableSelectionUI();
        OnInventoryObjectDeselection?.Invoke(this, new OnInventoryObjectEventArgs { primitiveInventoryObject = inventoryObjectInventoryUI.PrimitiveInventoryObject });
        
        isSelected = false;
    }

    private void EnableSelectionUI() => selectionUITransform.gameObject.SetActive(true);
    private void DisableSelectionUI() => selectionUITransform.gameObject.SetActive(false);

    #region Subscriptions
    private void ShopOpeningManager_OnShopOpen(object sender, EventArgs e)
    {
        DeselectInventoryObject();
    }

    private void InventoryObjectSelectionHandler_OnInventoryObjectSelection(object sender, OnInventoryObjectEventArgs e)
    {
        if(e.primitiveInventoryObject != inventoryObjectInventoryUI.PrimitiveInventoryObject) //If another InventoryObject is selected, deselect this one
        {
            DeselectInventoryObject();
        }
    }
    #endregion
}
