using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewElementSO", menuName = "ScriptableObjects/Inventory/Element")]
public class ElementSO : InventoryObjectSO
{
    //[Header("ElementSO Settings")]


    public override InventoryObjectType GetInventoryObjectType() => InventoryObjectType.Element;

}
