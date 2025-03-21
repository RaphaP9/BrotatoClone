using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityCritChanceStatUI : StatUI
{
    private void OnEnable()
    {
        AbilityCritChanceStatManager.OnAbilityCritChanceStatInitialized += AbilityCritChanceStatManager_OnAbilityCritChanceStatInitialized;
        AbilityCritChanceStatManager.OnAbilityCritChanceStatUpdated += AbilityCritChanceStatManager_OnAbilityCritChanceStatUpdated;
    }

    private void OnDisable()
    {
        AbilityCritChanceStatManager.OnAbilityCritChanceStatInitialized -= AbilityCritChanceStatManager_OnAbilityCritChanceStatInitialized;
        AbilityCritChanceStatManager.OnAbilityCritChanceStatUpdated -= AbilityCritChanceStatManager_OnAbilityCritChanceStatUpdated;
    }

    protected override string ProcessCurrentValue(float currentValue) => GeneralGameplayUtilities.ProcessCurrentValueToPercentage(currentValue, 2);


    private void AbilityCritChanceStatManager_OnAbilityCritChanceStatInitialized(object sender, AbilityCritChanceStatManager.OnAbilityCritChanceStatEventArgs e)
    {
        UpdateUIByNewValue(e.abilityCritChanceStat, playerIdentifier.CharacterSO.abilityCritChance);
    }

    private void AbilityCritChanceStatManager_OnAbilityCritChanceStatUpdated(object sender, AbilityCritChanceStatManager.OnAbilityCritChanceStatEventArgs e)
    {
        UpdateUIByNewValue(e.abilityCritChanceStat, playerIdentifier.CharacterSO.abilityCritChance);
    }

}
