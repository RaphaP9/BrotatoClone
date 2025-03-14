using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MeleeWeaponHandler : AttackBasedWeaponHandler
{
    [Header("Settings")]
    [SerializeField] protected LayerMask enemyLayerMask;
    protected MeleeWeaponSO MeleeWeaponSO => weaponSO as MeleeWeaponSO;

    protected float GetWeaponModifiedArea() => GeneralGameplayUtilities.GetWeaponModifiedArea(MeleeWeaponSO.attackArea, AreaMultiplierStatManager.Instance.AreaMultiplierStat);


}
