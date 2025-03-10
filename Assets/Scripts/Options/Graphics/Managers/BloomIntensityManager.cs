using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using System;

public class BloomIntensityManager : PostProcessingManager
{
    public static BloomIntensityManager Instance { get; private set; }

    private Bloom bloom;

    public static event EventHandler OnBloomIntensityManagerInitialized;
    public static event EventHandler<OnIntensityChangedEventArgs> OnBloomIntensityChanged;

    private const float MAX_INTENSITY = 1.5f;
    private const float MIN_INTENSITY = 0f;

    private const float DEFAULT_NORMALIZED_INTENSITY = 0.5f;

    private void Awake()
    {
        SetSingleton();
        InitializeSetting();
        SetDefaultNormalizedIntensity(DEFAULT_NORMALIZED_INTENSITY);
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            //Debug.LogWarning("There is more than one BloomIntensityManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    protected override void InitializeSetting()
    {
        if (!volumeProfile.TryGet(out bloom))
        {
            settingFound = false;
            Debug.Log("Bloom settings not found in the Post Process Volume");
        }
    }

    protected override void InitializeIntensity()
    {
        base.InitializeIntensity();
        OnBloomIntensityManagerInitialized?.Invoke(this, EventArgs.Empty);
    }

    public override void ChangeIntensity(float normalizedIntensity)
    {
        base.ChangeIntensity(normalizedIntensity);
        OnBloomIntensityChanged?.Invoke(this, new OnIntensityChangedEventArgs { newIntensity = normalizedIntensity });
    }

    protected override void SetIntensity(float intensity) => bloom.intensity.value = intensity;
    protected override float GetIntensity() => bloom.intensity.value;
    public override float GetMaxIntensity() => MAX_INTENSITY;
    public override float GetMinIntensity() => MIN_INTENSITY;
}
