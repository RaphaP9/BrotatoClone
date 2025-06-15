using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryObjectSelectionHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Button inventoryObjectButton;
    [SerializeField] private Transform selectionUITransform;

    private bool isSelected = false;

    private void OnEnable()
    {
        ShopOpeningManager.OnShopOpen += ShopOpeningManager_OnShopOpen;
    }

    private void OnDisable()
    {
        ShopOpeningManager.OnShopOpen -= ShopOpeningManager_OnShopOpen;
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

        }
        else
        {

        }
    }

    private void SelectInventoryObject()
    {
        EnableSelectionUI();

        isSelected = true;
    }

    private void DeselectInventoryObject()
    {
        DisableSelectionUI();

        isSelected = false;
    }

    private void EnableSelectionUI() => selectionUITransform.gameObject.SetActive(true);
    private void DisableSelectionUI() => selectionUITransform.gameObject.SetActive(false);

    private void ShopOpeningManager_OnShopOpen(object sender, EventArgs e)
    {
        DeselectInventoryObject();
    }
}
