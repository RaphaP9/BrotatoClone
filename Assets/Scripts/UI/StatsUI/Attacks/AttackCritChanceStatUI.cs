using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCritChanceStatUI : StatUI
{
    private void OnEnable()
    {
        AttackCritChanceStatManager.OnAttackCritChanceStatInitialized += AttackCritChanceStatManager_OnAttackCritChanceStatInitialized;
        AttackCritChanceStatManager.OnAttackCritChanceStatUpdated += AttackCritChanceStatManager_OnAttackCritChanceStatUpdated;
    }

    private void OnDisable()
    {
        AttackCritChanceStatManager.OnAttackCritChanceStatInitialized -= AttackCritChanceStatManager_OnAttackCritChanceStatInitialized;
        AttackCritChanceStatManager.OnAttackCritChanceStatUpdated -= AttackCritChanceStatManager_OnAttackCritChanceStatUpdated;
    }

    protected override StatType GetStatType() => StatType.AttackCritChance;



    private void AttackCritChanceStatManager_OnAttackCritChanceStatUpdated(object sender, AttackCritChanceStatManager.OnAttackCritChanceStatEventArgs e)
    {
        UpdateUIByNewValue(e.attackCritChanceStat, playerIdentifier.CharacterSO.attackCritChance);
    }

    private void AttackCritChanceStatManager_OnAttackCritChanceStatInitialized(object sender, AttackCritChanceStatManager.OnAttackCritChanceStatEventArgs e)
    {
        UpdateUIByNewValue(e.attackCritChanceStat, playerIdentifier.CharacterSO.attackCritChance);
    }
}
