using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAblitySO", menuName = "ScriptableObjects/Inventory/Ability")]
public class AbilitySO : InventoryObjectSO
{
    [Header("AbilitySO Settings")]
    [SerializeField, Range(10f,30f)] private float cooldown;

    public override InventoryObjectType GetInventoryObjectType() => InventoryObjectType.Ability;
}
