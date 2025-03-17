using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthRegenStatUI : StatUI
{
    private void OnEnable()
    {
        HealthRegenStatManager.OnHealthRegenStatInitialized += HealthRegenStatManager_OnHealthRegenStatInitialized;
        HealthRegenStatManager.OnHealthRegenStatUpdated += HealthRegenStatManager_OnHealthRegenStatUpdated;
    }
    private void OnDisable()
    {
        HealthRegenStatManager.OnHealthRegenStatInitialized -= HealthRegenStatManager_OnHealthRegenStatInitialized;
        HealthRegenStatManager.OnHealthRegenStatUpdated -= HealthRegenStatManager_OnHealthRegenStatUpdated;
    }

    protected override string ProcessCurrentValue(float currentValue) => GeneralGameplayUtilities.ProcessCurrentValueToSimpleString(currentValue);

    private void HealthRegenStatManager_OnHealthRegenStatInitialized(object sender, HealthRegenStatManager.OnHealthRegenStatEventArgs e)
    {
        UpdateUIByNewValue(e.healthRegenStat, playerIdentifier.PlayerSO.healthRegen);
    }
    private void HealthRegenStatManager_OnHealthRegenStatUpdated(object sender, HealthRegenStatManager.OnHealthRegenStatEventArgs e)
    {
        UpdateUIByNewValue(e.healthRegenStat, playerIdentifier.PlayerSO.healthRegen);
    }

}
