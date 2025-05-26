using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponSO : InventoryObjectSO, IDamageDealer
{
    [Header("WeaponSO Settings")]
    public WeaponType weaponType;
    public FireType fireType;
    [Range(0, 10)] public int regularDamage;
    [Space]
    [Range(0f, 10)] public int bleedDamage;
    [Range(2f, 10f)] public float bleedDuration;
    [Range(0.25f, 2f)] public float bleedTickTime;
    [Space]
    [Range(0f, 1f)] public float critChance;
    [Range(0f,1f)] public float critDamageMultiplier;
    [Space]
    [ColorUsage(true, true)] public Color damageColor;
    [Space]
    public Transform weaponTransform;


    public override InventoryObjectType GetInventoryObjectType() => InventoryObjectType.Weapon;


    public string GetName() => inventoryObjectName;
    public Color GetDamageColor() => damageColor;
    public string GetDescription() => description;
    public Sprite GetSprite() => sprite;
    public DamageDealerClassification GetDamageDealerClassification() => DamageDealerClassification.Weapon;
}
