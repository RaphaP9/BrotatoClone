using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMeleeWeaponSO", menuName = "ScriptableObjects/Inventory/Weapons/MeleeWeapon")]

public class MeleeWeaponSO : WeaponSO
{
    [Header("Melee Weapon Settings")]
    [Range(0f, 5f)] public float attackArea;
}
