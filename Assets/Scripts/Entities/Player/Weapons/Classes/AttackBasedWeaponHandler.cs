using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackBasedWeaponHandler : WeaponHandler
{
    public AttackBasedWeaponSO AttackBasedWeaponSO => weaponSO as AttackBasedWeaponSO;

    protected float attackTimer = 0f;

    public class OnWeaponAttackEventArgs: EventArgs
    {
        public int id;
        public bool isCrit;
    }

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
        if (!GetAttackInput()) return;

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
    protected float GetWeaponModifiedAttackSpeed() => GeneralGameplayUtilities.GetModifiedAttackSpeed(AttackBasedWeaponSO.attackSpeed, AttackSpeedMultiplierStatManager.Instance.AttackSpeedMultiplierStat);
    public float GetAttackSpeedRatioToBaseSpeed() => GetWeaponModifiedAttackSpeed() / AttackBasedWeaponSO.attackSpeed;
}
