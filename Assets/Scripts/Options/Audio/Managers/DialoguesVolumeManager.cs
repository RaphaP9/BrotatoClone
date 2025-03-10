using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static VolumeManager;

public class DialoguesVolumeManager : VolumeManager
{
    public static DialoguesVolumeManager Instance { get; private set; }

    private const string DIALOGUES_VOLUME = "DialoguesVolume";

    public static event EventHandler OnDialoguesVolumeManagerInitialized;
    public static event EventHandler<OnVolumeChangedEventArgs> OnDialoguesVolumeChanged;

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
            //Debug.LogWarning("There is more than one DialoguesVolumeManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }
    protected override void InitializeVolume()
    {
        base.InitializeVolume();
        OnDialoguesVolumeManagerInitialized?.Invoke(this, EventArgs.Empty);
    }

    public override void ChangeVolume(float volume)
    {
        volume = volume < GetMinVolume() ? GetMinVolume() : volume;
        volume = volume > GetMaxVolume() ? GetMaxVolume() : volume;

        masterAudioMixer.SetFloat(DIALOGUES_VOLUME, Mathf.Log10(volume) * 20);
        SaveVolumePlayerPrefs(volume);

        OnDialoguesVolumeChanged?.Invoke(this, new OnVolumeChangedEventArgs { newVolume = volume });
    }

    public override float GetLogarithmicVolume()
    {
        masterAudioMixer.GetFloat(DIALOGUES_VOLUME, out float logarithmicVolume);
        return logarithmicVolume;
    }
}
