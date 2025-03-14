using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FistsHandler : MeleeWeaponHandler
{
    [Header("Fists Components")]
    [SerializeField] private Transform attackPoint;

    protected override void Attack()
    {
        MeleeAttack(attackPoint);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmosColor;

        Gizmos.DrawWireSphere(attackPoint.position, GetWeaponModifiedArea());
    }
}
