using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static UnityEngine.EventSystems.EventTrigger;

public abstract class WaveSpawningSystemManager : MonoBehaviour
{
    public static WaveSpawningSystemManager Instance { get; private set; }

    public static event EventHandler<OnWaveEventArgs> OnWaveStart;
    public static event EventHandler<OnWaveEventArgs> OnWaveCompleted;

    [Header("Runtime Filled")]
    [SerializeField] protected float currentWaveDuration;
    [SerializeField] protected float currentWaveElapsedTime;
    [SerializeField] protected WaveSO currentWave;

    public float CurrentWaveDuration => currentWaveDuration;
    public float CurrentWaveElapsedTime => currentWaveElapsedTime;
    public WaveSO CurrentWave => currentWave;

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

        ClearCurrentWave();
        ResetCurrentWaveDuration();
        ResetCurrentWaveElapsedTime();
    }

    protected EnemySO GetRandomEnemyByWeight(WaveSO waveSO)
    {
        int totalWeight = GetTotalWaveWeight(waveSO);
        int randomValue = UnityEngine.Random.Range(0, totalWeight);

        int currentWeight = 0;

        foreach (WaveEnemy waveEnemy in waveSO.waveEnemies)
        {
            currentWeight += waveEnemy.weight;

            if (randomValue <= currentWeight) return waveEnemy.enemySO;
        }

        return waveSO.waveEnemies[0].enemySO; 
    }

    protected int GetTotalWaveWeight(WaveSO waveSO)
    {
        int totalWeight = 0;

        foreach(WaveEnemy waveEnemy in waveSO.waveEnemies)
        {
            totalWeight += waveEnemy.weight;
        }

        return totalWeight;
    }

    protected void SetCurrentWave(WaveSO waveSO) => currentWave = waveSO;
    protected void SetCurrentWaveDuration(float duration) => currentWaveDuration = duration;
    protected void SetCurrentWaveElapsedTime(float elapsedTime) => currentWaveElapsedTime = elapsedTime;

    protected void ClearCurrentWave() => currentWave = null;
    protected void ResetCurrentWaveDuration() => currentWaveDuration = 0;
    protected void ResetCurrentWaveElapsedTime() => currentWaveElapsedTime = 0;

    protected float GetNormalizedElapsedWaveTime() => currentWaveElapsedTime/currentWaveDuration;

    #region Subscriptions
    private void WaveStateManager_OnWaveStart(object sender, GeneralWavesManager.OnWaveEventArgs e)
    {
        SetCurrentWave(e.waveSO);
        SetCurrentWaveDuration(e.waveSO.duration);

        OnWaveStart?.Invoke(this, new OnWaveEventArgs { waveSO = e.waveSO });

        StartWave(e.waveSO);
    }
    #endregion
}
