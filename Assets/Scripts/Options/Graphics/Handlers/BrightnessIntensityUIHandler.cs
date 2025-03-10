using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrightnessIntensityUIHandler : PostProcessingUIHandler
{
    private void OnEnable()
    {
        BrightnessIntensityManager.OnBrightnessIntensityManagerInitialized += BrightnessIntensityManager_OnBrightnessIntensityManagerInitialized;
        BrightnessIntensityManager.OnBrightnessIntensityChanged += BrightnessIntensityManager_OnBrightnessIntensityChanged;
    }

    private void OnDisable()
    {
        BrightnessIntensityManager.OnBrightnessIntensityManagerInitialized -= BrightnessIntensityManager_OnBrightnessIntensityManagerInitialized;
        BrightnessIntensityManager.OnBrightnessIntensityChanged -= BrightnessIntensityManager_OnBrightnessIntensityChanged;
    }

    protected override void SetPostProcessingManager() => postProcessingManager = BrightnessIntensityManager.Instance;

    private void BrightnessIntensityManager_OnBrightnessIntensityManagerInitialized(object sender, System.EventArgs e)
    {
        InitializeUI();
    }

    private void BrightnessIntensityManager_OnBrightnessIntensityChanged(object sender, BrightnessIntensityManager.OnIntensityChangedEventArgs e)
    {
        if (!postProcessingManager) return;
        UpdateVisual();
    }
}
