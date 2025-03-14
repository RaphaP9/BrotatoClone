using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InventoryObjectSO : ScriptableObject
{
    public int id;
    public string inventoryObjectName;
    public InventoryObjectType type;
    public InventoryObjectRarityType rarityType;
    [TextArea(3,10)] public string description;
    public Sprite sprite;
}
