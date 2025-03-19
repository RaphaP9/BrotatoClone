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

    public float CurrentWaveDuration => currentWaveDuration;
    public float CurrentWaveElapsedTime => currentWaveElapsedTime;

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

    #region Subscriptions
    private void WaveStateManager_OnWaveStart(object sender, GeneralWavesManager.OnWaveEventArgs e)
    {
        OnWaveStart?.Invoke(this, new OnWaveEventArgs { waveSO = e.waveSO });
        StartWave(e.waveSO);
    }
    #endregion
}
