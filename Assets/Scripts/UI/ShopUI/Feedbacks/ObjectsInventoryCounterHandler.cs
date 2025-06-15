using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectsInventoryCounterHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;
    [Space]
    [SerializeField] private TextMeshProUGUI currentText;
    [SerializeField] private TextMeshProUGUI capacityText;

    private const string DENY_ANIMATION_NAME = "Deny";

    private void OnEnable()
    {
        ObjectsInventoryManager.OnObjectsInventoryInitialized += ObjectsInventoryManager_OnObjectsInventoryInitialized;
        ObjectsInventoryManager.OnObjectAddedToInventory += ObjectsInventoryManager_OnObjectAddedToInventory;
        ObjectsInventoryManager.OnObjectRemovedFromInventory += ObjectsInventoryManager_OnObjectRemovedFromInventory;

        ShopObjectCardPurchaseHandler.OnAnyShopObjectPurchaseDeniedByObjectsInventory += ShopObjectCardPurchaseHandler_OnAnyShopObjectPurchaseDeniedByObjectsInventory;
    }

    private void OnDisable()
    {
        ObjectsInventoryManager.OnObjectsInventoryInitialized -= ObjectsInventoryManager_OnObjectsInventoryInitialized;
        ObjectsInventoryManager.OnObjectAddedToInventory -= ObjectsInventoryManager_OnObjectAddedToInventory;
        ObjectsInventoryManager.OnObjectRemovedFromInventory -= ObjectsInventoryManager_OnObjectRemovedFromInventory;

        ShopObjectCardPurchaseHandler.OnAnyShopObjectPurchaseDeniedByObjectsInventory -= ShopObjectCardPurchaseHandler_OnAnyShopObjectPurchaseDeniedByObjectsInventory;
    }

    private void SetCurrentText(int currentObjects) => currentText.text = currentObjects.ToString();
    private void SetCapacityText(int capacity) => capacityText.text = capacity.ToString();


    #region Subscriptions
    private void ObjectsInventoryManager_OnObjectsInventoryInitialized(object sender, ObjectsInventoryManager.OnObjectsEventArgs e)
    {
        SetCurrentText(ObjectsInventoryManager.Instance.GetObjectsInventoryCount());
        SetCapacityText(ObjectsInventoryManager.Instance.GetObjectsInventoryCapacity());
    }

    private void ObjectsInventoryManager_OnObjectAddedToInventory(object sender, ObjectsInventoryManager.OnObjectEventArgs e)
    {
        SetCurrentText(ObjectsInventoryManager.Instance.GetObjectsInventoryCount());
    }

    private void ObjectsInventoryManager_OnObjectRemovedFromInventory(object sender, ObjectsInventoryManager.OnObjectEventArgs e)
    {
        SetCurrentText(ObjectsInventoryManager.Instance.GetObjectsInventoryCount());
    }

    private void ShopObjectCardPurchaseHandler_OnAnyShopObjectPurchaseDeniedByObjectsInventory(object sender, ShopObjectCardPurchaseHandler.OnShopObjectPurchaseEventArgs e)
    {
        animator.Play(DENY_ANIMATION_NAME);
    }
    #endregion
}
