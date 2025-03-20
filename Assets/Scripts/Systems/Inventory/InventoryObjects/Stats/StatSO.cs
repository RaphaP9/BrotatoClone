using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "NewStatSO", menuName = "ScriptableObjects/Inventory/Stat")]
public class StatSO : InventoryObjectSO
{
    [Header("Stat Settings")]
    public StatType statType;
    public StatModificationType statModificationType;
    public float value;

    public override InventoryObjectType GetInventoryObjectType() => InventoryObjectType.Stat;
}
