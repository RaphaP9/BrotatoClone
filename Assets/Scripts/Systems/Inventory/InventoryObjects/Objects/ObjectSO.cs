using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewObjectSO", menuName = "ScriptableObjects/Inventory/Object")]
public class ObjectSO : InventoryObjectSO
{
    //[Header("ObjectSO Settings")]

    public override InventoryObjectType GetInventoryObjectType() => InventoryObjectType.Object;
}
