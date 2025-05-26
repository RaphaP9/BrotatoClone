using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GeneralWavesManager : MonoBehaviour
{
    public static GeneralWavesManager Instance { get; private set; }

    [Header("Lists")]
    [SerializeField] private List<WaveSO> waves;
    [Space]
    [SerializeField] private List<WaveSO> randomEndgameWaves;

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

    public static event EventHandler<OnWaveEventArgs> OnSuddenCompleteWave;

    public List<WaveSO> Waves => waves;
    public State state => waveState;

    private float timer = 0f;

    public class OnWaveEventArgs : EventArgs
    {
        public WaveSO waveSO;
        public int waveNumber;
    }

    private void OnEnable()
    {
        GameManager.OnStateChanged += GameManager_OnStateChanged;

        WaveSpawningSystemManager.OnWaveStart += WaveSpawningSystemManager_OnWaveStart;
        WaveSpawningSystemManager.OnWaveCompleted += WaveSpawningSystemManager_OnWaveCompleted;
    }

    private void OnDisable()
    {
        GameManager.OnStateChanged -= GameManager_OnStateChanged;

        WaveSpawningSystemManager.OnWaveStart -= WaveSpawningSystemManager_OnWaveStart;
        WaveSpawningSystemManager.OnWaveCompleted -= WaveSpawningSystemManager_OnWaveCompleted;
    }

    private void Awake()
    {
        SetSingleton();

        SetWaveState(State.NotOnWave);
        ResetCurrentWaveNumber();
        ClearCurrentWave();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            SuddenCompleteWave();
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

        OnWaveStart?.Invoke(this, new OnWaveEventArgs { waveSO = currentWave, waveNumber = currentWaveNumber });
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

        IncreaseCurrentWaveNumber();

        WaveSO nextWave;

        if (WaveWithWaveNumberExists(currentWaveNumber))
        {
            nextWave = FindWaveByWaveNumber(currentWaveNumber);
        }
        else
        {
            if (debug) Debug.Log($"Wave with WaveNumber {currentWaveNumber} does not exist. Choosing random wave from endgame list.");
            nextWave = GetRamdomWaveFromList(randomEndgameWaves);
        }

        StartWave(nextWave);
    }

    private void StartWave(WaveSO waveSO)
    {
        if (waveSO == null)
        {
            if (debug) Debug.Log("WaveSO is null. Start will be ignored");
            return;
        }

        SetCurrentWave(waveSO);
        OnWaveStarting?.Invoke(this, new OnWaveEventArgs { waveSO = waveSO, waveNumber = currentWaveNumber });
        SetWaveState(State.StartingWave);

        ResetTimer();
    }

    private WaveSO FindWaveByWaveNumber(int waveNumber)
    {
        if (waves.Count < waveNumber)
        {
            if (debug) Debug.Log($"WaveSO with WaveNumber {waveNumber} could not be found on list. Proceding to return null.");
            return null;
        }

        return waves[waveNumber-1];
    }

    private bool WaveWithWaveNumberExists(int waveNumber)
    {
        if(waves.Count < waveNumber) return false;
        return true;
    }

    private WaveSO GetRamdomWaveFromList(List<WaveSO> wavesList)
    {
        if (wavesList.Count <= 0)
        {
            if (debug) Debug.Log("List does not contain any elements. Proceding to return null");
            return null;
        }

        WaveSO waveSO = GeneralUtilities.ChooseRandomElementFromList(wavesList);
        return waveSO;
    }

    private void EndCurrentWave()
    {
        OnWaveEnd?.Invoke(this, new OnWaveEventArgs{ waveSO = currentWave, waveNumber = currentWaveNumber });
        ClearCurrentWave();
    }

    private void SetCurrentWave(WaveSO waveSO) => currentWave = waveSO;
    private void ClearCurrentWave() => currentWave = null;
    private void ResetCurrentWaveNumber() => currentWaveNumber = 0;
    private void SetCurrentWaveNumber(int number) => currentWaveNumber = number;
    private void IncreaseCurrentWaveNumber() => currentWaveNumber += 1;
    private void SetWaveState(State state) => waveState = state;
    private void ResetTimer() => timer = 0f;

    public void SuddenCompleteWave()
    {
        if(currentWave == null) return;
        if (state != State.OnWave) return;

        OnSuddenCompleteWave?.Invoke(this, new OnWaveEventArgs { waveSO = currentWave, waveNumber = currentWaveNumber });
    }

    #region Subscriptions

    private void GameManager_OnStateChanged(object sender, GameManager.OnStateEventArgs e)
    {
        if (e.newState != GameManager.State.OnWave) return;
        StartNextWave();
    }

    private void WaveSpawningSystemManager_OnWaveStart(object sender, WaveSpawningSystemManager.OnWaveEventArgs e)
    {
        SetWaveState(State.OnWave);
        ResetTimer();
    }

    private void WaveSpawningSystemManager_OnWaveCompleted(object sender, WaveSpawningSystemManager.OnWaveEventArgs e)
    {
        SetWaveState(State.EndingWave);
        OnWaveCompleted?.Invoke(this, new OnWaveEventArgs { waveSO = e.waveSO, waveNumber = currentWaveNumber });
        ResetTimer();
    }
    #endregion
}
