using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InventoryObjectSO : ScriptableObject
{
    [Header("InventoryObjectSO Settings")]
    public int id;
    public string inventoryObjectName;
    public InventoryObjectRarityType rarityType;
    [TextArea(3,10)] public string description;
    public Sprite sprite;
    [Space]
    [Range(0, 1000)] public int price;


    public abstract InventoryObjectType GetInventoryObjectType();
}
