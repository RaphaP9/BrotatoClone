using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashesStatUI : StatUI
{
    private void OnEnable()
    {
        DashesStatManager.OnDashesStatInitialized += DashesStatManager_OnDashesStatInitialized;
        DashesStatManager.OnDashesStatUpdated += DashesStatManager_OnDashesStatUpdated;
    }

    private void OnDisable()
    {
        DashesStatManager.OnDashesStatInitialized -= DashesStatManager_OnDashesStatInitialized;
        DashesStatManager.OnDashesStatUpdated -= DashesStatManager_OnDashesStatUpdated;
    }

    protected override StatType GetStatType() => StatType.Dashes;


    private void DashesStatManager_OnDashesStatInitialized(object sender, DashesStatManager.OnDashesStatEventArgs e)
    {
        UpdateUIByNewValue(e.dashesStat, playerIdentifier.CharacterSO.dashes);
    }

    private void DashesStatManager_OnDashesStatUpdated(object sender, DashesStatManager.OnDashesStatEventArgs e)
    {
        UpdateUIByNewValue(e.dashesStat, playerIdentifier.CharacterSO.dashes);
    }

}
