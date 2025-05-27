using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeChanceStatUI : StatUI
{
    private void OnEnable()
    {
        DodgeChanceStatManager.OnDodgeChanceStatInitialized += DodgeChanceStatManager_OnDodgeChanceStatInitialized;
        DodgeChanceStatManager.OnDodgeChanceStatUpdated += DodgeChanceStatManager_OnDodgeChanceStatUpdated;
    }
    private void OnDisable()
    {
        DodgeChanceStatManager.OnDodgeChanceStatInitialized -= DodgeChanceStatManager_OnDodgeChanceStatInitialized;
        DodgeChanceStatManager.OnDodgeChanceStatUpdated -= DodgeChanceStatManager_OnDodgeChanceStatUpdated;
    }

    protected override StatType GetStatType() => StatType.DodgeChance;

    private void DodgeChanceStatManager_OnDodgeChanceStatInitialized(object sender, DodgeChanceStatManager.OnDodgeChanceStatEventArgs e)
    {
        UpdateUIByNewValue(e.dodgeChanceStat, playerIdentifier.CharacterSO.dodgeChance);

    }

    private void DodgeChanceStatManager_OnDodgeChanceStatUpdated(object sender, DodgeChanceStatManager.OnDodgeChanceStatEventArgs e)
    {
        UpdateUIByNewValue(e.dodgeChanceStat, playerIdentifier.CharacterSO.dodgeChance);
    }
}
