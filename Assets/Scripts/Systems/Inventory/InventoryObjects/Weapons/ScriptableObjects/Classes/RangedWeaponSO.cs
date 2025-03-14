using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRangedWeaponSO", menuName = "ScriptableObjects/Inventory/Weapons/RangedWeapon")]

public class RangedWeaponSO : AttackBasedWeaponSO, IProjectileSpawner
{
    [Header("Ranged Weapon Settings")]
    [Range(3f, 20f)] public float projectileRange;
    [Range(5f, 15f)] public float projectileSpeed;
    [Range(0f, 20f)] public float dispersionAngle;
    [Space]
    public ProjectileDamageType projectileDamageType;
    [Range(0f,3f)] public float projectileArea;

    public ProjectileDamageType GetProjectileDamageType() => projectileDamageType;
    public float GetProjectileArea() => projectileArea;
}
