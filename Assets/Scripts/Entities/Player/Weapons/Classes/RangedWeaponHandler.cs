using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RangedWeaponHandler : AttackBasedWeaponHandler
{
    protected RangedWeaponSO RangedWeaponSO => weaponSO as RangedWeaponSO;

    protected void InstantiateProyectile()
    {

    }
}
