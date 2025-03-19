using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WaveStateManager : MonoBehaviour
{
    public static WaveStateManager Instance { get; private set; }

    [Header("Lists")]
    [SerializeField] private List<Wave> waves;

    [Header("Settings")]
    [SerializeField, Range(0f, 5f)] private float waveStartingTime;
    [SerializeField, Range(0f, 5f)] private float waveEndingTime;

    [Header("States")]
    [SerializeField] private State waveState;

    [Header("Debug")]
    [SerializeField] private bool debug;

    [Header("Runtime Filled")]
    [SerializeField] private int currentWaveNumber;
    [SerializeField] private Wave currentWave;

    public enum State { NotOnWave, StartingWave, OnWave, EndingWave}

    public static event EventHandler<OnWaveEventArgs> OnWaveStarting;
    public static event EventHandler<OnWaveEventArgs> OnWaveStart;
    public static event EventHandler<OnWaveEventArgs> OnWaveCompleted;
    public static event EventHandler<OnWaveEventArgs> OnWaveEnd;

    public List<Wave> Waves => waves;
    public State state => waveState;

    private float timer = 0f;

    public class OnWaveEventArgs : EventArgs
    {
        public Wave wave;
    }

    [Serializable]
    public class Wave
    {
        public int waveNumber;
    }

    private void Awake()
    {
        SetSingleton();
    }

    private void Start()
    {
        SetWaveState(State.NotOnWave);
        ResetCurrentWaveNumber();
        ClearCurrentWave();
    }

    private void Update()
    {
        HandleWavesLogic();
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

    private void HandleWavesLogic()
    {
        switch (state)
        {
            case State.NotOnWave:
                NotOnWaveLogic();
                break;
            case State.StartingWave:
                StartingWaveLogic();
                break;
            case State.OnWave:
                OnWaveLogic();
                break;
            case State.EndingWave:
                EndingWaveLogic();
                break;
        }
    }

    #region Logics
    private void NotOnWaveLogic()
    {
        ResetTimer();
    }

    private void StartingWaveLogic()
    {
        if(timer < waveStartingTime)
        {
            timer += Time.deltaTime;
            return;
        }

        OnWaveStart?.Invoke(this, new OnWaveEventArgs { wave = currentWave });
        ResetTimer();

        SetWaveState(State.OnWave);
    }

    private void OnWaveLogic()
    {
        //Derived to WaveSpawningSystemManager
    }

    private void EndingWaveLogic()
    {
        if (timer < waveStartingTime)
        {
            timer += Time.deltaTime;
            return;
        }

        OnWaveEnd?.Invoke(this, new OnWaveEventArgs { wave = currentWave });
        ResetTimer();

        ClearCurrentWave();
        SetWaveState(State.NotOnWave);
    }

    #endregion

    public void StartNextWave()
    {
        IncreaseCurrentWaveNumber();
        StartWave(currentWaveNumber);
    }

    private void StartWave(int waveNumber)
    {
        Wave wave = FindWaveByWaveNumber(waveNumber);

        if(wave == null)
        {
            if (debug) Debug.Log("Wave is null. Start will be ignored");
            return;
        }

        OnWaveStart?.Invoke(this, new OnWaveEventArgs { wave = wave });
    }

    private Wave FindWaveByWaveNumber(int waveNumber)
    {
        foreach(Wave wave in waves)
        {
            if (wave.waveNumber == waveNumber) return wave; 
        }

        if (debug) Debug.Log($"Wave with waveNumber {waveNumber} could not be found. Proceding to return null.");
        return null;
    }

    private void SetCurrentWave(Wave wave) => currentWave = wave;
    private void ClearCurrentWave() => currentWave = null;
    private void ResetCurrentWaveNumber() => currentWaveNumber = 0;
    private void SetCurrentWaveNumber(int number) => currentWaveNumber = number;
    private void IncreaseCurrentWaveNumber() => currentWaveNumber += 1;
    private void SetWaveState(State state) => waveState = state;
    private void ResetTimer() => timer = 0f;
}
