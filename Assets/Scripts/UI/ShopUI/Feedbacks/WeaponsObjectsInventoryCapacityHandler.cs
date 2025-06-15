using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeaponsObjectsInventoryCapacityHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;
    [Space]
    [SerializeField] private TextMeshProUGUI currentText;
    [SerializeField] private TextMeshProUGUI capacityText;

    private const string DENY_ANIMATION_NAME = "Deny";

    private void OnEnable()
    {
        WeaponsInventoryManager.OnWeaponsInventoryInitialized += WeaponsInventoryManager_OnWeaponsInventoryInitialized;
        WeaponsInventoryManager.OnWeaponAddedToInventory += WeaponsInventoryManager_OnWeaponAddedToInventory;
        WeaponsInventoryManager.OnWeaponRemovedFromInventory += WeaponsInventoryManager_OnWeaponRemovedFromInventory;

        ShopObjectCardPurchaseHandler.OnAnyShopObjectPurchaseDeniedByWeaponsInventory += ShopObjectCardPurchaseHandler_OnAnyShopObjectPurchaseDeniedByWeaponsInventory;
        ShopSellHandler.OnWeaponSellDeniedByOnlyOne += ShopSellHandler_OnWeaponSellDeniedByOnlyOne;
    }

    private void OnDisable()
    {
        WeaponsInventoryManager.OnWeaponsInventoryInitialized -= WeaponsInventoryManager_OnWeaponsInventoryInitialized;
        WeaponsInventoryManager.OnWeaponAddedToInventory -= WeaponsInventoryManager_OnWeaponAddedToInventory;
        WeaponsInventoryManager.OnWeaponRemovedFromInventory -= WeaponsInventoryManager_OnWeaponRemovedFromInventory;

        ShopObjectCardPurchaseHandler.OnAnyShopObjectPurchaseDeniedByObjectsInventory -= ShopObjectCardPurchaseHandler_OnAnyShopObjectPurchaseDeniedByWeaponsInventory;
        ShopSellHandler.OnWeaponSellDeniedByOnlyOne -= ShopSellHandler_OnWeaponSellDeniedByOnlyOne;
    }

    private void SetCurrentText(int currentObjects) => currentText.text = currentObjects.ToString();
    private void SetCapacityText(int capacity) => capacityText.text = capacity.ToString();


    #region Subscriptions
    private void WeaponsInventoryManager_OnWeaponsInventoryInitialized(object sender, WeaponsInventoryManager.OnWeaponsEventArgs e)
    {
        SetCurrentText(WeaponsInventoryManager.Instance.GetWeaponsInventoryCount());
        SetCapacityText(WeaponsInventoryManager.Instance.GetWeaponsInventoryCapacity());
    }

    private void WeaponsInventoryManager_OnWeaponAddedToInventory(object sender, WeaponsInventoryManager.OnWeaponEventArgs e)
    {
        SetCurrentText(WeaponsInventoryManager.Instance.GetWeaponsInventoryCount());
    }

    private void WeaponsInventoryManager_OnWeaponRemovedFromInventory(object sender, WeaponsInventoryManager.OnWeaponEventArgs e)
    {
        SetCurrentText(WeaponsInventoryManager.Instance.GetWeaponsInventoryCount());
    }

    private void ShopObjectCardPurchaseHandler_OnAnyShopObjectPurchaseDeniedByWeaponsInventory(object sender, ShopObjectCardPurchaseHandler.OnShopObjectPurchaseEventArgs e)
    {
        animator.Play(DENY_ANIMATION_NAME);
    }

    private void ShopSellHandler_OnWeaponSellDeniedByOnlyOne(object sender, System.EventArgs e)
    {
        animator.Play(DENY_ANIMATION_NAME);
    }
    #endregion
}
