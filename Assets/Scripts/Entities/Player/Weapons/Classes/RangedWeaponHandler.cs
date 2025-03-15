using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RangedWeaponHandler : AttackBasedWeaponHandler
{
    [Header("RangedWeaponHandler Components")]
    [SerializeField] protected WeaponAim weaponAim;

    protected RangedWeaponSO RangedWeaponSO => weaponSO as RangedWeaponSO;

    public static event EventHandler<OnRangedWeaponAttackEventArgs> OnRangedFire;
    public event EventHandler<OnRangedWeaponAttackEventArgs> OnThisRangedFire;

    public class OnRangedWeaponAttackEventArgs : OnWeaponAttackEventArgs
    {
        public Transform firePoint;
    }

    protected void ShootProjectile(Transform projectilePrefab, Transform firePoint)
    {
        bool isCrit = GeneralGameplayUtilities.EvaluateCritAttack(GetWeaponModifiedCritChance());

        Vector2 shootDirection = weaponAim.AimDirection;
        Vector2 shootPosition = GeneralUtilities.TransformPositionVector2(firePoint);

        CreateProjectile(projectilePrefab, shootPosition, shootDirection, isCrit);

        OnRangedFire?.Invoke(this, new OnRangedWeaponAttackEventArgs { id = RangedWeaponSO.id,  isCrit = isCrit, firePoint = firePoint, });
        OnThisRangedFire?.Invoke(this, new OnRangedWeaponAttackEventArgs { id = RangedWeaponSO.id, isCrit = isCrit, firePoint = firePoint, });
    }

    protected void CreateProjectile(Transform projectile, Vector2 position, Vector2 shootDirection, bool isCrit)
    {
        Vector3 vector3Position = GeneralUtilities.Vector2ToVector3(position);
        Transform instantiatedProjectile = Instantiate(projectile, vector3Position, Quaternion.identity);

        ProjectileHandler projectileHandler = instantiatedProjectile.GetComponent<ProjectileHandler>();

        if(projectileHandler == null)
        {
            if (debug) Debug.Log("Instantiated projectile does not contain a ProjectileHandler component. Set will be ignored.");
            return;
        }

        Vector2 processedShootDirection = GeneralGameplayUtilities.DeviateShootDirection(shootDirection, RangedWeaponSO.dispersionAngle);

        projectileHandler.SetProjectile(RangedWeaponSO.projectileSpeed, RangedWeaponSO.projectileRange, RangedWeaponSO.regularDamage,
            RangedWeaponSO.bleedDamage, RangedWeaponSO.bleedDuration, RangedWeaponSO.bleedTickTime, RangedWeaponSO.critChance,RangedWeaponSO.critDamageMultiplier, RangedWeaponSO.projectileDamageType,
            RangedWeaponSO.projectileArea, RangedWeaponSO, processedShootDirection, isCrit);
    }
}
