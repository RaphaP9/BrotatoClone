using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MeleeWeaponHandler : AttackBasedWeaponHandler
{
    protected MeleeWeaponSO MeleeWeaponSO => weaponSO as MeleeWeaponSO;

    protected float GetWeaponModifiedArea() => GeneralGameplayUtilities.GetWeaponModifiedArea(MeleeWeaponSO.attackArea, AreaMultiplierStatManager.Instance.AreaMultiplierStat);
}
