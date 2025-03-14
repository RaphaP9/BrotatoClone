using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PistolHandler : RangedWeaponHandler
{
    [Header("Pistol Components")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private Transform projectilePrefab;

    protected override void Attack()
    {
        ShootProjectile(projectilePrefab, firePoint);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmosColor;

        Gizmos.DrawLine(firePoint.position, firePoint.position + GeneralUtilities.Vector2ToVector3(weaponAim.AimDirection)*RangedWeaponSO.projectileRange);
    }
}