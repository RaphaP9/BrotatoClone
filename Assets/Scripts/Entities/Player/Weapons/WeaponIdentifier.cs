using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponIdentifier : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private WeaponSO weaponSO;

    public WeaponSO WeaponSO => weaponSO;
}
