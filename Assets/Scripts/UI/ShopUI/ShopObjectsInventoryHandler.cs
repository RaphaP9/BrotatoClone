using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;

public class ShopObjectsInventoryHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform inventoryObjectsContainer;
    [SerializeField] private Transform objectUIPrefab;
    [SerializeField] private Transform emptySlotPrefab;

    [Header("Debugg")]
    [SerializeField] private bool debug;

    private void OnEnable()
    {
        ObjectsInventoryManager.OnObjectsInventoryInitialized += ObjectsInventoryManager_OnObjectsInventoryInitialized;
        ObjectsInventoryManager.OnObjectAddedToInventory += ObjectsInventoryManager_OnObjectAddedToInventory;
        ObjectsInventoryManager.OnObjectRemovedFromInventory += ObjectsInventoryManager_OnObjectRemovedFromInventory;
    }

    private void OnDisable()
    {
        ObjectsInventoryManager.OnObjectsInventoryInitialized -= ObjectsInventoryManager_OnObjectsInventoryInitialized;
        ObjectsInventoryManager.OnObjectAddedToInventory -= ObjectsInventoryManager_OnObjectAddedToInventory;
        ObjectsInventoryManager.OnObjectRemovedFromInventory -= ObjectsInventoryManager_OnObjectRemovedFromInventory;
    }

    private void UpdateUI()
    {
        ClearContainer();

        int emptySlots = ObjectsInventoryManager.Instance.GetObjectsInventoryCapacity();

        foreach (ObjectInventoryIdentified objectInventoryIdentified in ObjectsInventoryManager.Instance.ObjectsInventory)
        {
            CreateObjectUI(objectInventoryIdentified);
            emptySlots--;
        }

        for (int i = 0; i < emptySlots; i++)
        {
            CreateEmptySlot();
        }
    }


    private void ClearContainer()
    {
        foreach(Transform child in inventoryObjectsContainer)
        {
            Destroy(child.gameObject);
        }
    }

    private void CreateObjectUI(ObjectInventoryIdentified objectInventoryIdentified)
    {
        Transform objectUITransform = Instantiate(objectUIPrefab, inventoryObjectsContainer);

        ObjectInventoryUI objectInventoryUI = objectUITransform.GetComponent<ObjectInventoryUI>();

        if(objectInventoryUI == null)
        {
            if (debug) Debug.Log("Instantiated prefab does not contain an ObjectInventoryUI component. Set will be ignored");
            return;
        }

        objectInventoryUI.SetObjectInventory(objectInventoryIdentified);
    }

    private void CreateEmptySlot()
    {
        Transform emptySlotUI = Instantiate(emptySlotPrefab, inventoryObjectsContainer);
    }

    private void ObjectsInventoryManager_OnObjectsInventoryInitialized(object sender, ObjectsInventoryManager.OnObjectsEventArgs e)
    {
        UpdateUI();
    }

    private void ObjectsInventoryManager_OnObjectAddedToInventory(object sender, ObjectsInventoryManager.OnObjectEventArgs e)
    {
        UpdateUI();
    }

    private void ObjectsInventoryManager_OnObjectRemovedFromInventory(object sender, ObjectsInventoryManager.OnObjectEventArgs e)
    {
        UpdateUI();
    }
}
