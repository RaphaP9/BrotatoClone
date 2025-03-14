using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MeleeWeaponHandler : AttackBasedWeaponHandler
{
    private MeleeWeaponSO MeleeWeaponSO => weaponSO as MeleeWeaponSO;

    protected void DealAreaDamage(Vector2 position, float damage)
    {

    }

}
