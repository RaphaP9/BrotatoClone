using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EspinasDeAyahuascaHandler : RangedWeaponHandler
{
    [Header("Components")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private Transform projectilePrefab;
    [SerializeField, Range(10f,30f)] private float deviationAngle;

    public event EventHandler OnEspinasDeAyahuascaAttack;

    protected override void Attack()
    {
        //3 attacks
        ShootProjectile(projectilePrefab, firePoint, weaponAim.AimDirection);
        ShootProjectile(projectilePrefab, firePoint, GeneralUtilities.RotateVector2ByAngleDegrees(weaponAim.AimDirection, deviationAngle));
        ShootProjectile(projectilePrefab, firePoint, GeneralUtilities.RotateVector2ByAngleDegrees(weaponAim.AimDirection, -deviationAngle));

        OnEspinasDeAyahuascaAttack?.Invoke(this, EventArgs.Empty);
    }

    private void OnDrawGizmos()
    {
        if(!debug) return;  

        Gizmos.color = gizmosColor;

        Gizmos.DrawLine(firePoint.position, firePoint.position + GeneralUtilities.Vector2ToVector3(weaponAim.AimDirection) * RangedWeaponSO.projectileRange);
    }
}