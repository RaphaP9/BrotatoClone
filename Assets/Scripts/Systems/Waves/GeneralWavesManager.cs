using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GeneralWavesManager : MonoBehaviour
{
    public static GeneralWavesManager Instance { get; private set; }

    [Header("Lists")]
    [SerializeField] private List<WaveSO> waves;

    [Header("Settings")]
    [SerializeField, Range(0f, 5f)] private float waveStartingTime;
    [SerializeField, Range(0f, 5f)] private float waveEndingTime;

    [Header("States")]
    [SerializeField] private State waveState;

    [Header("Debug")]
    [SerializeField] private bool debug;

    [Header("Runtime Filled")]
    [SerializeField] private int currentWaveNumber;
    [SerializeField] private WaveSO currentWave;

    public enum State { NotOnWave, StartingWave, OnWave, EndingWave}

    public static event EventHandler<OnWaveEventArgs> OnWaveStarting;
    public static event EventHandler<OnWaveEventArgs> OnWaveStart;
    public static event EventHandler<OnWaveEventArgs> OnWaveCompleted;
    public static event EventHandler<OnWaveEventArgs> OnWaveEnd;

    public List<WaveSO> Waves => waves;
    public State state => waveState;

    private float timer = 0f;

    public class OnWaveEventArgs : EventArgs
    {
        public WaveSO waveSO;
    }

    private void OnEnable()
    {
        WaveSpawningSystemManager.OnWaveStart += WaveSpawningSystemManager_OnWaveStart;
        WaveSpawningSystemManager.OnWaveCompleted += WaveSpawningSystemManager_OnWaveCompleted;
    }

    private void OnDisable()
    {
        WaveSpawningSystemManager.OnWaveStart -= WaveSpawningSystemManager_OnWaveStart;
        WaveSpawningSystemManager.OnWaveCompleted -= WaveSpawningSystemManager_OnWaveCompleted;
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
        if (Input.GetKeyDown(KeyCode.K))
        {
            StartNextWave();
        }

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

        OnWaveStart?.Invoke(this, new OnWaveEventArgs { waveSO = currentWave });
        ResetTimer();
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

        EndCurrentWave();

        ResetTimer();
        SetWaveState(State.NotOnWave);
    }

    #endregion

    public void StartNextWave()
    {
        if(state != State.NotOnWave)
        {
            if (debug) Debug.Log("Another Wave is in progress. Can not start wave.");
            return;
        }

        if (!WaveWithWaveNumberExists(currentWaveNumber+1))
        {
            if (debug) Debug.Log($"Wave with WaveNumber {currentWaveNumber + 1} does not exist. Can not start next wave.");
            return;
        }

        IncreaseCurrentWaveNumber();
        StartWave(currentWaveNumber);
    }

    private void StartWave(int waveNumber)
    {
        WaveSO waveSO = FindWaveByWaveNumber(waveNumber);

        if(waveSO == null)
        {
            if (debug) Debug.Log("WaveSO is null. Start will be ignored");
            return;
        }

        SetCurrentWave(waveSO);
        OnWaveStarting?.Invoke(this, new OnWaveEventArgs { waveSO = waveSO });
        SetWaveState(State.StartingWave);

        ResetTimer();
    }

    private WaveSO FindWaveByWaveNumber(int waveNumber)
    {
        foreach(WaveSO waveSO in waves)
        {
            if (waveSO.waveNumber == waveNumber) return waveSO; 
        }

        if (debug) Debug.Log($"WaveSO with WaveNumber {waveNumber} could not be found. Proceding to return null.");
        return null;
    }

    private bool WaveWithWaveNumberExists(int waveNumber)
    {
        WaveSO waveSO = FindWaveByWaveNumber(waveNumber);
        return waveSO != null;
    }

    private void EndCurrentWave()
    {
        OnWaveEnd?.Invoke(this, new OnWaveEventArgs { waveSO = currentWave });
        ClearCurrentWave();
    }

    private void SetCurrentWave(WaveSO waveSO) => currentWave = waveSO;
    private void ClearCurrentWave() => currentWave = null;
    private void ResetCurrentWaveNumber() => currentWaveNumber = 0;
    private void SetCurrentWaveNumber(int number) => currentWaveNumber = number;
    private void IncreaseCurrentWaveNumber() => currentWaveNumber += 1;
    private void SetWaveState(State state) => waveState = state;
    private void ResetTimer() => timer = 0f;


    #region Subscriptions
    private void WaveSpawningSystemManager_OnWaveStart(object sender, WaveSpawningSystemManager.OnWaveEventArgs e)
    {
        SetWaveState(State.OnWave);
        ResetTimer();
    }

    private void WaveSpawningSystemManager_OnWaveCompleted(object sender, WaveSpawningSystemManager.OnWaveEventArgs e)
    {
        SetWaveState(State.EndingWave);
        OnWaveCompleted?.Invoke(this, new OnWaveEventArgs { waveSO = e.waveSO });
        ResetTimer();
    }
    #endregion
}
