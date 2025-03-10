using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

public class MasterVolumeManager : VolumeManager
{
    public static MasterVolumeManager Instance { get; private set; }

    private const string MASTER_VOLUME = "MasterVolume";

    public static event EventHandler OnMasterVolumeManagerInitialized;
    public static event EventHandler<OnVolumeChangedEventArgs> OnMasterVolumeChanged;

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
            //Debug.LogWarning("There is more than one MasterVolumeManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }
    protected override void InitializeVolume()
    {
        base.InitializeVolume();
        OnMasterVolumeManagerInitialized?.Invoke(this, EventArgs.Empty);
    }

    public override void ChangeVolume(float volume) 
    {
        volume = volume < GetMinVolume() ? GetMinVolume() : volume;
        volume = volume > GetMaxVolume() ? GetMaxVolume() : volume;

        masterAudioMixer.SetFloat(MASTER_VOLUME, Mathf.Log10(volume) * 20);
        SaveVolumePlayerPrefs(volume);

        OnMasterVolumeChanged?.Invoke(this, new OnVolumeChangedEventArgs { newVolume = volume });
    }

    public override float GetLogarithmicVolume()
    {
        masterAudioMixer.GetFloat(MASTER_VOLUME, out float logarithmicVolume);
        return logarithmicVolume;
    }
}
