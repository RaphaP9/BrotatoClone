using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStatSO", menuName = "ScriptableObjects/Inventory/Stat")]
public class StatSO : InventoryObjectSO
{
    [Header("Stat Settings")]
    [Range(0,1000)] public int price;
    [Space]
    public StatType statType;
    public StatModificationType statModificationType;
    public float value;
}
