using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicsVolumeManager : VolumeManager
{
    public static CinematicsVolumeManager Instance { get; private set; }

    private const string CINEMATICS_VOLUME = "CinematicsVolume";

    public static event EventHandler OnCinematicsVolumeManagerInitialized;
    public static event EventHandler<OnVolumeChangedEventArgs> OnCinematicsVolumeChanged;

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
            //Debug.LogWarning("There is more than one CinematicsVolumeManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }
    protected override void InitializeVolume()
    {
        base.InitializeVolume();
        OnCinematicsVolumeManagerInitialized?.Invoke(this, EventArgs.Empty);
    }

    public override void ChangeVolume(float volume)
    {
        volume = volume < GetMinVolume() ? GetMinVolume() : volume;
        volume = volume > GetMaxVolume() ? GetMaxVolume() : volume;

        masterAudioMixer.SetFloat(CINEMATICS_VOLUME, Mathf.Log10(volume) * 20);
        SaveVolumePlayerPrefs(volume);

        OnCinematicsVolumeChanged?.Invoke(this, new OnVolumeChangedEventArgs { newVolume = volume });
    }

    public override float GetLogarithmicVolume()
    {
        masterAudioMixer.GetFloat(CINEMATICS_VOLUME, out float logarithmicVolume);
        return logarithmicVolume;
    }
}
