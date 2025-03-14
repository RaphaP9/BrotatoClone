using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackBasedWeaponSO : WeaponSO
{
    [Header("AttackBasedWeaponSO Settings")]
    [Range(0.25f, 4f)] public float attackSpeed;
}
