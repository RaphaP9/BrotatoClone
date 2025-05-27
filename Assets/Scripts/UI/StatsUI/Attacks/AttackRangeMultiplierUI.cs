using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRangeMultiplierUI : StatUI
{
    private void OnEnable()
    {
        AttackRangeMultiplierStatManager.OnAttackRangeMultiplierStatInitialized += AttackRangeMultiplierStatManager_OnAttackRangeMultiplierStatInitialized;
        AttackRangeMultiplierStatManager.OnAttackRangeMultiplierStatUpdated += AttackRangeMultiplierStatManager_OnAttackRangeMultiplierStatUpdated; 
    }

    private void OnDisable()
    {
        AttackRangeMultiplierStatManager.OnAttackRangeMultiplierStatInitialized -= AttackRangeMultiplierStatManager_OnAttackRangeMultiplierStatInitialized;
        AttackRangeMultiplierStatManager.OnAttackRangeMultiplierStatUpdated -= AttackRangeMultiplierStatManager_OnAttackRangeMultiplierStatUpdated;
    }

    protected override StatType GetStatType() => StatType.AttackRangeMultiplier;



    private void AttackRangeMultiplierStatManager_OnAttackRangeMultiplierStatInitialized(object sender, AttackRangeMultiplierStatManager.OnAttackRangeMultiplierStatEventArgs e)
    {
        UpdateUIByNewValue(e.attackRangeMultiplierStat, playerIdentifier.CharacterSO.attackRangeMultiplier);
    }

    private void AttackRangeMultiplierStatManager_OnAttackRangeMultiplierStatUpdated(object sender, AttackRangeMultiplierStatManager.OnAttackRangeMultiplierStatEventArgs e)
    {
        UpdateUIByNewValue(e.attackRangeMultiplierStat, playerIdentifier.CharacterSO.attackRangeMultiplier);
    }
}
