using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaMultiplierStatUI : StatUI
{
    private void OnEnable()
    {
        AreaMultiplierStatManager.OnAreaMultiplierStatInitialized += AreaMultiplierStatManager_OnAreaMultiplierStatInitialized;
        AreaMultiplierStatManager.OnAreaMultiplierStatUpdated += AreaMultiplierStatManager_OnAreaMultiplierStatUpdated;
    }

    private void OnDisable()
    {
        AreaMultiplierStatManager.OnAreaMultiplierStatInitialized -= AreaMultiplierStatManager_OnAreaMultiplierStatInitialized;
        AreaMultiplierStatManager.OnAreaMultiplierStatUpdated -= AreaMultiplierStatManager_OnAreaMultiplierStatUpdated;
    }

    protected override string ProcessCurrentValue(float currentValue) => GeneralGameplayUtilities.ProcessCurrentValueToExcessPercentage(currentValue, 2);

    private void AreaMultiplierStatManager_OnAreaMultiplierStatInitialized(object sender, AreaMultiplierStatManager.OnAreaMultiplierStatEventArgs e)
    {
        UpdateUIByNewValue(e.areaMultiplierStat, playerIdentifier.CharacterSO.areaMultiplier);
    }

    private void AreaMultiplierStatManager_OnAreaMultiplierStatUpdated(object sender, AreaMultiplierStatManager.OnAreaMultiplierStatEventArgs e)
    {
        UpdateUIByNewValue(e.areaMultiplierStat, playerIdentifier.CharacterSO.areaMultiplier);
    }
}
