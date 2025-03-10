using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloomIntensityUIHandler : PostProcessingUIHandler
{
    private void OnEnable()
    {
        BloomIntensityManager.OnBloomIntensityManagerInitialized += BloomIntensityManager_OnBloomIntensityManagerInitialized;
        BloomIntensityManager.OnBloomIntensityChanged += BloomIntensityManager_OnBloomIntensityChanged;
    }

    private void OnDisable()
    {
        BloomIntensityManager.OnBloomIntensityManagerInitialized -= BloomIntensityManager_OnBloomIntensityManagerInitialized;
        BloomIntensityManager.OnBloomIntensityChanged -= BloomIntensityManager_OnBloomIntensityChanged;
    }

    protected override void SetPostProcessingManager() => postProcessingManager = BloomIntensityManager.Instance;

    private void BloomIntensityManager_OnBloomIntensityManagerInitialized(object sender, System.EventArgs e)
    {
        InitializeUI();
    }

    private void BloomIntensityManager_OnBloomIntensityChanged(object sender, BloomIntensityManager.OnIntensityChangedEventArgs e)
    {
        if (!postProcessingManager) return;
        UpdateVisual();
    }
}
