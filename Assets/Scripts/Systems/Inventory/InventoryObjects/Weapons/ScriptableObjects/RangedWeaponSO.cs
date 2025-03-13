using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRangedWeaponSO", menuName = "ScriptableObjects/Inventory/Weapons/RangedWeapon")]

public class RangedWeaponSO : WeaponSO
{
    [Header("Ranged Weapon Settings")]
    [Range(0f, 5f)] public float attackRange;
}
