using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

public class MusicVolumeManager : VolumeManager
{
    public static MusicVolumeManager Instance { get; private set; }

    private const string MUSIC_VOLUME = "MusicVolume";

    public static event EventHandler OnMusicVolumeManagerInitialized;
    public static event EventHandler<OnVolumeChangedEventArgs> OnMusicVolumeChanged;

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
            //Debug.LogWarning("There is more than one MusicVolumeManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }
    protected override void InitializeVolume()
    {
        base.InitializeVolume();
        OnMusicVolumeManagerInitialized?.Invoke(this, EventArgs.Empty);
    }

    public override void ChangeVolume(float volume)
    {
        volume = volume < GetMinVolume() ? GetMinVolume() : volume;
        volume = volume > GetMaxVolume() ? GetMaxVolume() : volume;

        masterAudioMixer.SetFloat(MUSIC_VOLUME, Mathf.Log10(volume) * 20);
        SaveVolumePlayerPrefs(volume);

        OnMusicVolumeChanged?.Invoke(this, new OnVolumeChangedEventArgs { newVolume = volume });
    }

    public override float GetLogarithmicVolume()
    {
        masterAudioMixer.GetFloat(MUSIC_VOLUME, out float logarithmicVolume);
        return logarithmicVolume;
    }
}
