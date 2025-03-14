using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackBasedWeaponHandler : WeaponHandler
{
    public AttackBasedWeaponSO AttackBasedWeaponSO => weaponSO as AttackBasedWeaponSO;

    protected float attackTimer = 0f;

    protected virtual void Start()
    {
        InitializeVariables();
    }

    protected virtual void Update()
    {
        HandleAttack();
        HandleAttackCooldown();
    }

    protected virtual void InitializeVariables()
    {
        ResetAttackTimer();
    }

    private void HandleAttack()
    {
        if (AttackOnCooldown()) return;
        if (!InputAttack) return;

        Attack();
        ResetTimer();
    }

    private void HandleAttackCooldown()
    {
        if (attackTimer < 0) return;

        attackTimer -= Time.deltaTime;
    }

    protected abstract void Attack();

    private void ResetAttackTimer() => attackTimer = 0f;
    private bool AttackOnCooldown() => attackTimer > 0f;
    private void ResetTimer() => attackTimer = 1f / GetWeaponModifiedAttackSpeed();
    private float GetWeaponModifiedAttackSpeed() => GeneralGameplayUtilities.GetModifiedAttackSpeed(AttackBasedWeaponSO.attackSpeed, AttackSpeedMultiplierStatManager.Instance.AttackSpeedMultiplierStat);
}
