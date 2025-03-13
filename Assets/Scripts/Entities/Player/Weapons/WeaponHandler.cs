using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] protected WeaponSO weaponSO;
    [SerializeField] protected EnemyDetector enemyDetector;

    public WeaponSO WeaponSO => weaponSO;

    private float timer = 0f;
}
