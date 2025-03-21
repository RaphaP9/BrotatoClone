using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static UnityEditor.Rendering.FilterWindow;

public class ElementsInventoryManager : MonoBehaviour
{
    public static ElementsInventoryManager Instance { get; private set; }

    [Header("Lists")]
    [SerializeField] private List<ElementInventoryIdentified> elementsInventory;

    [Header("Debug")]
    [SerializeField] private bool debug;

    public List<ElementInventoryIdentified> ElementsInventory => elementsInventory;

    public static event EventHandler<OnElementsEventArgs> OnElementsInventoryInitialized;
    public static event EventHandler<OnElementEventArgs> OnElementAddedToInventory;
    public static event EventHandler<OnElementEventArgs> OnElementRemovedFromInventory;

    public class OnElementsEventArgs : EventArgs
    {
        public List<ElementInventoryIdentified> elements;
    }

    public class OnElementEventArgs : EventArgs
    {
        public ElementInventoryIdentified element;
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

    private void Start()
    {
        SetElementsInventoryFromCharacter();
        InitializeElementsInventory();
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

    private void InitializeElementsInventory()
    {
        OnElementsInventoryInitialized?.Invoke(this, new OnElementsEventArgs { elements = elementsInventory });
    }

    private void AddElementToInventory(ElementSO elementSO)
    {
        if (elementSO == null)
        {
            if (debug) Debug.Log("ElementSO is null, addition will be ignored");
            return;
        }

        string elementGUID = GeneralDataUtilities.GenerateGUID();

        ElementInventoryIdentified elementToAdd = new ElementInventoryIdentified { GUID = elementGUID, elementSO = elementSO };

        elementsInventory.Add(elementToAdd);

        OnElementAddedToInventory?.Invoke(this, new OnElementEventArgs { element = elementToAdd });
    }

    private void RemoveElementFromInventoryByElementSO(ElementSO elementSO)
    {
        if (elementSO == null)
        {
            if (debug) Debug.Log("ElementSO is null, remotion will be ignored");
            return;
        }

        ElementInventoryIdentified elementIdentified = FindElementByElementSO(elementSO);

        if (elementIdentified == null)
        {
            if (debug) Debug.Log("Could not find weapon by ElementSO");
            return;
        }

        elementsInventory.Remove(elementIdentified);

        OnElementRemovedFromInventory?.Invoke(this, new OnElementEventArgs { element = elementIdentified });
    }

    private void RemoveWeaponFromInventoryByGUID(string GUID)
    {
        ElementInventoryIdentified elementIdentified = FindElementByGUID(GUID);

        if (elementIdentified == null)
        {
            if (debug) Debug.Log("Could not find element by GUID");
            return;
        }

        elementsInventory.Remove(elementIdentified);

        OnElementRemovedFromInventory?.Invoke(this, new OnElementEventArgs { element = elementIdentified });
    }

    private ElementInventoryIdentified FindElementByElementSO(ElementSO elementSO)
    {
        foreach (ElementInventoryIdentified element in elementsInventory)
        {
            if (element.elementSO == elementSO) return element;
        }

        if (debug) Debug.Log($"Element with ElementSO with ID {elementSO.id} could not be found. Proceding to return null");
        return null;
    }

    private ElementInventoryIdentified FindElementByGUID(string GUID)
    {
        foreach (ElementInventoryIdentified element in elementsInventory)
        {
            if (element.GUID == GUID) return element;
        }

        if (debug) Debug.Log($"Element with GUID {GUID} could not be found. Proceding to return null");
        return null;
    }

    private void SetElementsInventoryFromCharacter()
    {
        ClearElementsInventory();
        AddElementsToInventory(PlayerIdentifier.Instance.CharacterSO.startingElements);
    }

    private void AddElementsToInventory(List<ElementSO> elementSOs)
    {
        foreach (ElementSO elementSO in elementSOs)
        {
            AddElementToInventory(elementSO);
        }
    }

    private void ClearElementsInventory() => elementsInventory.Clear();

    public bool ElementsInventoryFull() => false;

    public bool ElementInInventoryByElementSO(ElementSO elementSO)
    {
        foreach (ElementInventoryIdentified element in elementsInventory)
        {
            if (element.elementSO == elementSO) return true;
        }

        return false;
    }

    public bool ElementsInInventoryByElementSO(List<ElementSO> elementSOs)
    {
        foreach (ElementSO elementSO in elementSOs)
        {
            if (!ElementInInventoryByElementSO(elementSO)) return false;
        }

        return true;
    }
}

[System.Serializable]
public class ElementInventoryIdentified
{
    public string GUID;
    public ElementSO elementSO;
}