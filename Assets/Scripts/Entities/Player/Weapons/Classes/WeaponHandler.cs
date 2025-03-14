using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] protected WeaponSO weaponSO;

    public WeaponSO WeaponSO => weaponSO;

    protected bool InputAttack => AttackInput.Instance.GetAttackDown();
}
