using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopSellHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Button sellButton;
    [Space]
    [SerializeField] private Transform nonSelectedGroup;
    [SerializeField] private Transform selectedGroup;
    [Space]
    [SerializeField] private TextMeshProUGUI priceText;

    [Header("Runtime Filled")]
    [SerializeField] private PrimitiveInventoryObject selectedInventoryObject;

    public static event EventHandler<OnInventoryObjectSelectedEventArgs> OnInventoryObjectSelected;
    public static event EventHandler<OnInventoryObjectSelectedEventArgs> OnInventoryObjectDeselected;

    public static event EventHandler OnWeaponSellDeniedByOnlyOne;

    public static event EventHandler<OnObjectSoldEventArgs> OnObjectSold;
    public static event EventHandler<OnWeaponSoldEventArgs> OnWeaponSold;

    public class OnInventoryObjectSelectedEventArgs : EventArgs
    {
        public PrimitiveInventoryObject inventoryObject;
    }

    public class OnObjectSoldEventArgs : EventArgs
    {
        public ObjectSO objectSO;
    }

    public class OnWeaponSoldEventArgs : EventArgs
    {
        public WeaponSO weaponSO;
    }

    private void OnEnable()
    {
        InventoryObjectClickSelectionHandler.OnInventoryObjectSelectionCommand += InventoryObjectClickSelectionHandler_OnInventoryObjectSelectionCommand;
        InventoryObjectClickSelectionHandler.OnInventoryObjectDeselectionCommand += InventoryObjectClickSelectionHandler_OnInventoryObjectDeselectionCommand;
    }

    private void OnDisable()
    {
        InventoryObjectClickSelectionHandler.OnInventoryObjectSelectionCommand -= InventoryObjectClickSelectionHandler_OnInventoryObjectSelectionCommand;
        InventoryObjectClickSelectionHandler.OnInventoryObjectDeselectionCommand -= InventoryObjectClickSelectionHandler_OnInventoryObjectDeselectionCommand;
    }

    private void Awake()
    {
        InitializeButtonsListeners();
    }

    private void Start()
    {
        RemoveInventoryObjectSelection();
    }

    private void InitializeButtonsListeners()
    {
        sellButton.onClick.AddListener(HandleSell);
    }

    private void HandleSell()
    {
        if (selectedInventoryObject == null) return;
        if (selectedInventoryObject.inventoryObjectSO == null) return;
        if (selectedInventoryObject.GUID == "") return;

        switch (selectedInventoryObject.inventoryObjectSO.GetInventoryObjectType())
        {
            case InventoryObjectType.Object:
                HandleObjectSell(selectedInventoryObject);
                break;
            case InventoryObjectType.Weapon:
                HandleWeaponSell(selectedInventoryObject);
                break;
        }
    }

    private void HandleObjectSell(PrimitiveInventoryObject selectedInventoryObject)
    {
        SwitchToNonSelectedMode();
        ObjectsInventoryManager.Instance.RemoveObjectFromInventoryByGUID(selectedInventoryObject.GUID);
        GoldManager.Instance.AddGold(selectedInventoryObject.inventoryObjectSO.sellPrice);

        OnObjectSold?.Invoke(this, new OnObjectSoldEventArgs { objectSO = selectedInventoryObject.inventoryObjectSO as ObjectSO });

    }

    private void HandleWeaponSell(PrimitiveInventoryObject selectedInventoryObject)
    {
        if (WeaponsInventoryManager.Instance.HasOneOrLessWeapons())
        {
            DenyWeaponSellByOnlyOne();
            return;
        }

        SwitchToNonSelectedMode();
        WeaponsInventoryManager.Instance.RemoveWeaponFromInventoryByGUID(selectedInventoryObject.GUID);
        GoldManager.Instance.AddGold(selectedInventoryObject.inventoryObjectSO.sellPrice);

        OnWeaponSold?.Invoke(this, new OnWeaponSoldEventArgs { weaponSO = selectedInventoryObject.inventoryObjectSO as WeaponSO });
    }

    private void DenyWeaponSellByOnlyOne()
    {
        OnWeaponSellDeniedByOnlyOne?.Invoke(this, EventArgs.Empty);
    }


    private void SwitchToNonSelectedMode()
    {
        sellButton.enabled = false;

        selectedGroup.gameObject.SetActive(false);
        nonSelectedGroup.gameObject.SetActive(true);
    }

    private void SwitchToSelectedMode()
    {
        sellButton.enabled = true;

        nonSelectedGroup.gameObject.SetActive(false);
        selectedGroup.gameObject.SetActive(true);
    }

    private void SetInventoryObjectSelection(PrimitiveInventoryObject primitiveInventoryObject)
    {
        SetSelectedInventoryObject(primitiveInventoryObject);
        SetPriceText(primitiveInventoryObject.inventoryObjectSO.sellPrice);

        SwitchToSelectedMode();
    }

    private void RemoveInventoryObjectSelection()
    {
        ClearSelectedInventoryObject();
        SwitchToNonSelectedMode();
    }

    private void SetPriceText(int price) => priceText.text = price.ToString();

    private void SetSelectedInventoryObject(PrimitiveInventoryObject primitiveInventoryObject)
    {
        OnInventoryObjectDeselected?.Invoke(this, new OnInventoryObjectSelectedEventArgs { inventoryObject = selectedInventoryObject }); //Deselection of the previous one
        selectedInventoryObject = primitiveInventoryObject;

        OnInventoryObjectSelected?.Invoke(this, new OnInventoryObjectSelectedEventArgs { inventoryObject = selectedInventoryObject }); //Selection of the previous one
    }

    private void ClearSelectedInventoryObject()
    {
        OnInventoryObjectDeselected?.Invoke(this, new OnInventoryObjectSelectedEventArgs { inventoryObject = selectedInventoryObject }); //Deselection of the previous one
        selectedInventoryObject = null;
    }

    #region Subscriptions
    private void InventoryObjectClickSelectionHandler_OnInventoryObjectSelectionCommand(object sender, InventoryObjectClickSelectionHandler.OnInventoryObjectEventArgs e)
    {
        if (selectedInventoryObject == e.primitiveInventoryObject) return; //AlreadySelected

        SetInventoryObjectSelection(e.primitiveInventoryObject);
    }

    private void InventoryObjectClickSelectionHandler_OnInventoryObjectDeselectionCommand(object sender, InventoryObjectClickSelectionHandler.OnInventoryObjectEventArgs e)
    {
        if (selectedInventoryObject != e.primitiveInventoryObject) return; //Other was selected

        RemoveInventoryObjectSelection();    
    }
    #endregion
}
