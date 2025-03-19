using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class WaveSpawningSystemManager : MonoBehaviour
{
    public static WaveSpawningSystemManager Instance { get; private set; }

    public static event EventHandler<OnWaveEventArgs> OnWaveStart;
    public static event EventHandler<OnWaveEventArgs> OnWaveCompleted;

    public class OnWaveEventArgs : EventArgs
    {
        public WaveSO waveSO;
    }

    private void OnEnable()
    {
        GeneralWavesManager.OnWaveStart += WaveStateManager_OnWaveStart;
    }

    private void OnDisable()
    {
        GeneralWavesManager.OnWaveStart -= WaveStateManager_OnWaveStart;
    }

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
            Debug.LogWarning("There is more than one WaveManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }
    
    protected abstract void StartWave(WaveSO waveSO);

    protected virtual void CompleteWave(WaveSO waveSO)
    {
        OnWaveCompleted?.Invoke(this, new OnWaveEventArgs { waveSO = waveSO });
    }

    #region Subscriptions
    private void WaveStateManager_OnWaveStart(object sender, GeneralWavesManager.OnWaveEventArgs e)
    {
        OnWaveStart?.Invoke(this, new OnWaveEventArgs { waveSO = e.waveSO });
        StartWave(e.waveSO);
    }
    #endregion
}
