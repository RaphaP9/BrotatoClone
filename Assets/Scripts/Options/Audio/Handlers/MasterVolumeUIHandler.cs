using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterVolumeUIHandler : VolumeUIHandler
{
    private void OnEnable()
    {
        MasterVolumeManager.OnMasterVolumeManagerInitialized += MasterVolumeManager_OnMasterVolumeManagerInitialized;
        MasterVolumeManager.OnMasterVolumeChanged += MasterVolumeManager_OnMasterVolumeChanged;
    }

    private void OnDisable()
    {
        MasterVolumeManager.OnMasterVolumeManagerInitialized -= MasterVolumeManager_OnMasterVolumeManagerInitialized;
        MasterVolumeManager.OnMasterVolumeChanged -= MasterVolumeManager_OnMasterVolumeChanged;
    }

    protected override void SetVolumeManager() => volumeManager = MasterVolumeManager.Instance;
    private void MasterVolumeManager_OnMasterVolumeManagerInitialized(object sender, System.EventArgs e)
    {
        InitializeUI();
    }
    private void MasterVolumeManager_OnMasterVolumeChanged(object sender, VolumeManager.OnVolumeChangedEventArgs e)
    {
        if (!volumeManager) return;
        UpdateVisual();
    }
}
