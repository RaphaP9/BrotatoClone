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

    protected override string ProcessCurrentValue(float currentValue) => GeneralGameplayUtilities.ProcessCurrentValueToPercentage(currentValue, 2);


    private void AttackCritChanceStatManager_OnAttackCritChanceStatUpdated(object sender, AttackCritChanceStatManager.OnAttackCritChanceStatEventArgs e)
    {
        UpdateUIByNewValue(e.attackCritChanceStat, playerIdentifier.PlayerSO.attackCritChance);
    }

    private void AttackCritChanceStatManager_OnAttackCritChanceStatInitialized(object sender, AttackCritChanceStatManager.OnAttackCritChanceStatEventArgs e)
    {
        UpdateUIByNewValue(e.attackCritChanceStat, playerIdentifier.PlayerSO.attackCritChance);
    }
}
