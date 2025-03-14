using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RangedWeaponHandler : AttackBasedWeaponHandler
{
    [Header("RangedWeaponHandler Components")]
    [SerializeField] protected WeaponAim weaponAim;

    protected RangedWeaponSO RangedWeaponSO => weaponSO as RangedWeaponSO;

    protected void ShootProjectile(Transform projectile, Vector2 position, Vector2 shootDirection)
    {
        Vector3 vector3Position = GeneralUtilities.Vector2ToVector3(position);
        Transform instantiatedProjectile = Instantiate(projectile, vector3Position, Quaternion.identity);

        ProjectileHandler projectileHandler = instantiatedProjectile.GetComponent<ProjectileHandler>();

        if(projectileHandler == null)
        {
            if (debug) Debug.Log("Instantiated projectile does not contain a ProjectileHandler component. Set will be ignored.");
            return;
        }

        projectileHandler.SetProjectile(RangedWeaponSO.projectileSpeed, RangedWeaponSO.projectileRange, RangedWeaponSO.regularDamage,
            RangedWeaponSO.bleedDamage, RangedWeaponSO.bleedDuration, RangedWeaponSO.bleedTickTime, RangedWeaponSO.projectileDamageType,
            RangedWeaponSO.projectileArea, RangedWeaponSO, shootDirection);
    }
}
