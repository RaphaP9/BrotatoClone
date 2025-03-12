using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InventoryObjectSO : ScriptableObject
{
    public string id;
    public string inventoryObjectName;
    public InventoryObjectType type;
    public InventoryObjectRarityType rarityType;
    public Sprite sprite;
}
