using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FistsHandler : MeleeWeaponHandler
{
    [Header("Fists Components")]
    [SerializeField] private List<Transform> attackPoints;

    protected override void Attack()
    {
        MeleeAttack(attackPoints);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmosColor;

        foreach (Transform attackPoint in attackPoints)
        {
            Gizmos.DrawWireSphere(attackPoint.position, GetWeaponModifiedArea());
        }
    }
}
