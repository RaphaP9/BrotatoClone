using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ElementsInventoryManager : MonoBehaviour
{
    public static ElementsInventoryManager Instance { get; private set; }

    [Header("Lists")]
    [SerializeField] private List<ElementSO> elementsInventory;

    [Header("Debug")]
    [SerializeField] private bool debug;

    public List<ElementSO> ElementsInventory => elementsInventory;

    public static event EventHandler<OnElementEventArgs> OnElementAddedToInventory;
    public static event EventHandler<OnElementEventArgs> OnElementRemovedFromInventory;

    public class OnElementEventArgs : EventArgs
    {
        public ElementSO elementSO;
    }

    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }

    private void Awake()
    {
        SetSingleton();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one ElementsInventoryManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    private void AddElementToInventory(ElementSO elementSO)
    {
        if (elementSO == null)
        {
            if (debug) Debug.Log("ElementSO is null, addition will be ignored");
            return;
        }

        if (elementsInventory.Contains(elementSO))
        {
            if (debug) Debug.Log($"ElementSO with name {elementSO.inventoryObjectName} is already on inventory, addition will be ignored");
            return;
        }

        elementsInventory.Add(elementSO);

        OnElementAddedToInventory?.Invoke(this, new OnElementEventArgs { elementSO = elementSO });
    }

    private void RemoveElementFromInventory(ElementSO elementSO)
    {
        if (elementSO == null)
        {
            if (debug) Debug.Log("ElementSO is null, remotion will be ignored");
            return;
        }

        if (!elementsInventory.Contains(elementSO))
        {
            if (debug) Debug.Log($"ElementSO with name {elementSO.inventoryObjectName} is not on inventory, remotion will be ignored");
            return;
        }

        elementsInventory.Remove(elementSO);

        OnElementRemovedFromInventory?.Invoke(this, new OnElementEventArgs { elementSO = elementSO });
    }
}
