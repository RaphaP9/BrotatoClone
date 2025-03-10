using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

public class SFXVolumeManager : VolumeManager
{
    public static SFXVolumeManager Instance { get; private set; }

    private const string SFX_VOLUME = "SFXVolume";

    public static event EventHandler OnSFXVolumeManagerInitialized;
    public static event EventHandler<OnVolumeChangedEventArgs> OnSFXVolumeChanged;

    private void Awake()
    {
        SetSingleton();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            //Debug.LogWarning("There is more than one SFXVolumeManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    protected override void InitializeVolume()
    {
        base.InitializeVolume();
        OnSFXVolumeManagerInitialized?.Invoke(this, EventArgs.Empty);
    }

    public override void ChangeVolume(float volume)
    {
        volume = volume < GetMinVolume() ? GetMinVolume() : volume;
        volume = volume > GetMaxVolume() ? GetMaxVolume() : volume;

        masterAudioMixer.SetFloat(SFX_VOLUME, Mathf.Log10(volume) * 20);
        SaveVolumePlayerPrefs(volume);

        OnSFXVolumeChanged?.Invoke(this, new OnVolumeChangedEventArgs { newVolume = volume });
    }

    public override float GetLogarithmicVolume()
    {
        masterAudioMixer.GetFloat(SFX_VOLUME, out float logarithmicVolume);
        return logarithmicVolume;
    }
}
