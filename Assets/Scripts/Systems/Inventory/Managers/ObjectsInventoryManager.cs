using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static WeaponsInventoryManager;

public class ObjectsInventoryManager : MonoBehaviour
{
    public static ObjectsInventoryManager Instance { get; private set; }

    [Header("Lists")]
    [SerializeField] private List<ObjectIdentified> objectsInventory;

    [Header("Debug")]
    [SerializeField] private bool debug;
    
    public List<ObjectIdentified> ObjectsInventory => objectsInventory;

    public static event EventHandler<OnObjectsEventArgs> OnObjectsInventoryInitialized;
    public static event EventHandler<OnObjectEventArgs> OnObjectAddedToInventory;
    public static event EventHandler<OnObjectEventArgs> OnObjectRemovedFromInventory;

    public class OnObjectEventArgs : EventArgs
    {
        public ObjectIdentified @object;
    }
    public class OnObjectsEventArgs : EventArgs
    {
        public List<ObjectIdentified> objects;
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
        SetObjectsInventoryFromCharacter();
        InitializeObjectsInventory();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one ObjectsInventoryManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    private void InitializeObjectsInventory()
    {
        OnObjectsInventoryInitialized?.Invoke(this, new OnObjectsEventArgs { objects = objectsInventory });
    }

    private void AddObjectToInventory(ObjectSO objectSO)
    {
        if (objectSO == null)
        {
            if (debug) Debug.Log("ObjectSO is null, addition will be ignored");
            return;
        }

        string objectGUID = GeneralDataUtilities.GenerateGUID();

        ObjectIdentified objectToAdd = new ObjectIdentified { GUID = objectGUID, objectSO = objectSO };

        objectsInventory.Add(objectToAdd);

        OnObjectAddedToInventory?.Invoke(this, new OnObjectEventArgs { @object = objectToAdd });
    }

    private void RemoveObjectFromInventoryByObjectSO(ObjectSO objectSO)
    {
        if (objectSO == null)
        {
            if (debug) Debug.Log("ObjectSO is null, remotion will be ignored");
            return;
        }

        ObjectIdentified objectIdentified = FindObjectByObjectSO(objectSO);

        if (objectIdentified == null)
        {
            if (debug) Debug.Log("Could not find object by ObjectSO");
            return;
        }

        objectsInventory.Remove(objectIdentified);

        OnObjectRemovedFromInventory?.Invoke(this, new OnObjectEventArgs { @object = objectIdentified });
    }

    private void RemoveObjectFromInventoryByGUID(string GUID)
    {
        ObjectIdentified objectIdentified = FindObjectByGUID(GUID);

        if (objectIdentified == null)
        {
            if (debug) Debug.Log("Could not find object by GUID");
            return;
        }

        objectsInventory.Remove(objectIdentified);

        OnObjectRemovedFromInventory?.Invoke(this, new OnObjectEventArgs { @object = objectIdentified });
    }

    private ObjectIdentified FindObjectByObjectSO(ObjectSO objectSO)
    {
        foreach (ObjectIdentified @object in objectsInventory)
        {
            if (@object.objectSO == objectSO) return @object;
        }

        if (debug) Debug.Log($"Object with ObjectSO with ID {objectSO.id} could not be found. Proceding to return null");
        return null;
    }

    private ObjectIdentified FindObjectByGUID(string GUID)
    {
        foreach (ObjectIdentified @object in objectsInventory)
        {
            if (@object.GUID == GUID) return @object;
        }

        if (debug) Debug.Log($"Object with GUID {GUID} could not be found. Proceding to return null");
        return null;
    }


    private void SetObjectsInventoryFromCharacter()
    {
        ClearObjectsInventory();
        AddObjectsToInventory(PlayerIdentifier.Instance.CharacterSO.startingObjects);
    }

    private void AddObjectsToInventory(List<ObjectSO> objectSOs)
    {
        foreach (ObjectSO objectSO in objectSOs)
        {
            AddObjectToInventory(objectSO);
        }
    }

    private void ClearObjectsInventory() => objectsInventory.Clear();

    public bool ObjectsInventoryFull() => false;
}

[System.Serializable]
public class ObjectIdentified
{
    public string GUID;
    public ObjectSO objectSO;
}