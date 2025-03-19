using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSpeedStatUI : StatUI
{
    private void OnEnable()
    {
        MoveSpeedStatManager.OnMoveSpeedStatInitialized += MoveSpeedStatManager_OnMoveSpeedStatInitialized;
        MoveSpeedStatManager.OnMoveSpeedStatUpdated += MoveSpeedStatManager_OnMoveSpeedStatUpdated;
    }
    private void OnDisable()
    {
        MoveSpeedStatManager.OnMoveSpeedStatInitialized -= MoveSpeedStatManager_OnMoveSpeedStatInitialized;
        MoveSpeedStatManager.OnMoveSpeedStatUpdated -= MoveSpeedStatManager_OnMoveSpeedStatUpdated;
    }

    protected override string ProcessCurrentValue(float currentValue) => GeneralGameplayUtilities.ProcessCurrentValueToSimpleFloat(currentValue, 2);

    private void MoveSpeedStatManager_OnMoveSpeedStatInitialized(object sender, MoveSpeedStatManager.OnMoveSpeedStatEventArgs e)
    {
        UpdateUIByNewValue(e.moveSpeedStat, playerIdentifier.CharacterSO.moveSpeed);
    }

    private void MoveSpeedStatManager_OnMoveSpeedStatUpdated(object sender, MoveSpeedStatManager.OnMoveSpeedStatEventArgs e)
    {
        UpdateUIByNewValue(e.moveSpeedStat, playerIdentifier.CharacterSO.moveSpeed);
    }

}
