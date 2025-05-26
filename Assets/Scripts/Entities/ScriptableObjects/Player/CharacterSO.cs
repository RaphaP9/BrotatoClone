using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacterSO", menuName = "ScriptableObjects/Entities/Character")]
public class CharacterSO : EntitySO
{
    [Header("Character Stats Settings")]
    [Range(0, 10)] public int healthRegen;
    [Space]
    [Range(0, 3)] public int dashes;
    [Space]
    [Range(0.5f, 4f)] public float areaMultiplier;
    [Space]
    [Range(0.5f, 4f)] public float attackSpeedMultiplier;
    [Range(0.5f, 4f)] public float attackRangeMultiplier;
    [Space]
    [Range(1f, 2f)] public float attackDamageMultiplier;
    [Range(0f, 1f)] public float attackCritChance;
    [Range(0.5f, 3f)] public float attackCritDamageMultiplier;

    [Header("Weapons")]
    [Range(2, 12)] public int weaponSlots;
    public List<WeaponSO> startingWeapons;

    [Header("Objects")]
    public List<ObjectSO> startingObjects;

    [Header("Visual")]
    public Transform characterVisualTransform;
}
