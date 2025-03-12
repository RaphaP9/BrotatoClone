using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStatsSO", menuName = "ScriptableObjects/Player/Stats")]
public class StatsSO : ScriptableObject
{
    [Range(10,100)] public int maxHealth;
    [Range(1f, 10f)] public float moveSpeed;
}
