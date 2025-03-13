using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponSO : InventoryObjectSO, IDamageDealer
{
    [Header("WeaponSO Settings")]
    public WeaponType weaponType;
    [Range(1, 10)] public int damage;
    [Space]
    [Range(1, 10)] public int bleedDamage;
    [Range(2f, 10f)] public float bleedDuration;
    [Range(0.25f, 2f)] public float bleedTickTime;
    [Space]
    [Range(0.25f, 4f)] public float attackSpeed;
    [Space]
    [Range(2f, 10f)] public Color damageColor;

    public string GetName() => inventoryObjectName;
    public Color GetDamageColor() => damageColor;
    public string GetDescription() => description;
    public Sprite GetSprite() => sprite;
}
