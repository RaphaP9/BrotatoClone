using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InventoryObjectSO : ScriptableObject
{
    [Header("InventoryObjectSO Settings")]
    public int id;
    public string inventoryObjectName;
    public InventoryObjectRarity objectRarity;
    [TextArea(3,10)] public string description;
    public Sprite sprite;
    [Space]
    [Range(0, 1000)] public int price;
    [Space]

    [Header("Embedded Stats")]
    public List<EmbeddedStat> embeddedStats;

    [Header("Required Elements")]
    public List<ElementSO> requiredElements;

    public abstract InventoryObjectType GetInventoryObjectType();
}
