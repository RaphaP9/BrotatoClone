using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryObjectInventoryUI : MonoBehaviour
{
    [Header("Runtime Filled")]
    [SerializeField] private PrimitiveInventoryObject primitiveInventoryObject;

    public PrimitiveInventoryObject PrimitiveInventoryObject => primitiveInventoryObject;

    protected void SetPrimitiveInventoryObject(string GUID, InventoryObjectSO inventoryObjectSO)
    {
        primitiveInventoryObject = new PrimitiveInventoryObject { GUID = GUID, inventoryObjectSO = inventoryObjectSO };
    }

    protected void ClearPrimitiveInventoryObject() => primitiveInventoryObject = null;
}
