using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerSO", menuName = "ScriptableObjects/Entities/Player")]
public class PlayerSO : EntitySO
{
    [Header("Player Stats")]
    [Range(0, 10)] public int healthRegen;
    [Space]
    [Range(0, 3)] public int dashes;
    [Space]
    [Range(1f, 2f)] public float attackDamageMultiplier;
    [Range(0f, 1f)] public float attackCritChance;
    [Range(0.5f, 2f)] public float attackCritDamageMultiplier;
    [Space]
    [Range(0f, 0.5f)] public float abilityCooldownReduction;
    [Space]
    [Range(1f, 2f)] public float abilityEffectMultiplier;
    [Range(0f, 1f)] public float abilityCritChance;
    [Range(0.5f, 2f)] public float abilityCritEffectMultiplier;
}
