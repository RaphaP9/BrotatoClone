using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRangedWeaponSO", menuName = "ScriptableObjects/Inventory/Weapons/RangedWeapon")]

public class RangedWeaponSO : AttackBasedWeaponSO
{
    [Header("Ranged Weapon Settings")]
    [Range(0f, 5f)] public float attackRange;
    [Range(5f, 15f)] public float projectileSpeed;
    [Range(0f, 0.1f)] public float dispersionPercentage;

    protected void FireProjectile()
    {

    }
}
