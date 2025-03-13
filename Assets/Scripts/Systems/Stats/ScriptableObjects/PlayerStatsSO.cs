using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerStatsSO", menuName = "ScriptableObjects/Player/PlayerStats")]
public class PlayerStatsSO : ScriptableObject
{
    [Range(10,100)] public int maxHealth;
    [Range(0, 10)] public int healthRegen;
    [Range(0f, 1f)] public float armorPercentage;
    [Range(0f, 1f)] public float dodgeChance;
    [Space]
    [Range(1f, 10f)] public float moveSpeed;
    [Range(0, 3)] public int dashes;
    [Space]
    [Range(0.5f, 2f)] public float attackSpeed;
    [Range(1f, 3f)] public float attackRange;
    [Space]
    [Range(1f, 2f)] public float attackDamageMultiplier;
    [Range(0f, 1f)] public float attackCritChance;
    [Range(0.5f, 2f)] public float attackCritDamageMultiplier;
    [Space]
    [Range(1f, 2f)] public float abilityEffectMultiplier;
    [Range(0f, 1f)] public float abilityCritChance;
    [Range(0.5f, 2f)] public float abilityCritEffectMultiplier;
    [Space]
    [Range(0f, 1f)] public float lifesteal;
}
