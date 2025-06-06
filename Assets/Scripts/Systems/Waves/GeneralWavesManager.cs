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

    [Header("Debug")]
    [SerializeField] private bool debug;

    [Header("Runtime Filled")]
    [SerializeField] private int currentWaveNumber;
    [SerializeField] private WaveSO currentWave;

    public static event EventHandler<OnWaveEventArgs> OnWaveStart;
    public static event EventHandler<OnWaveEventArgs> OnWaveCompleted;

    public static event EventHandler<OnWaveEventArgs> OnSuddenCompleteWave;

    public List<WaveSO> Waves => waves;

    private bool waveCompleted = false;

    public class OnWaveEventArgs : EventArgs
    {
        public WaveSO waveSO;
        public int waveNumber;
    }

    private void OnEnable()
    {
        WaveSpawningSystemManager.OnWaveCompleted += WaveSpawningSystemManager_OnWaveCompleted;
    }

    private void OnDisable()
    {
        WaveSpawningSystemManager.OnWaveCompleted -= WaveSpawningSystemManager_OnWaveCompleted;
    }

    private void Awake()
    {
        SetSingleton();

        ResetCurrentWaveNumber();
        ClearCurrentWave();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one WaveManager instance, proceding to destroy duplicate.");
            Destroy(gameObject);
        }
    }

    public void StartNextWave()
    {
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

        StartCoroutine(StartWave(nextWave));
    }

    private IEnumerator StartWave(WaveSO waveSO)
    {
        SetCurrentWave(waveSO);
        OnWaveStart?.Invoke(this, new OnWaveEventArgs { waveSO = waveSO, waveNumber = currentWaveNumber });

        waveCompleted = false;
        WaveSpawningSystemManager.Instance.StartWave(waveSO);
        yield return new WaitUntil(() => waveCompleted);
        waveCompleted = false;

        OnWaveCompleted?.Invoke(this, new OnWaveEventArgs { waveSO = waveSO, waveNumber = currentWaveNumber });

        ClearCurrentWave();
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

    private void SetCurrentWave(WaveSO waveSO) => currentWave = waveSO;
    private void ClearCurrentWave() => currentWave = null;
    private void ResetCurrentWaveNumber() => currentWaveNumber = 0;
    private void IncreaseCurrentWaveNumber() => currentWaveNumber += 1;

    public void SuddenCompleteWave()
    {
        if(currentWave == null) return;

        OnSuddenCompleteWave?.Invoke(this, new OnWaveEventArgs { waveSO = currentWave, waveNumber = currentWaveNumber });
    }

    #region Subscriptions
    private void WaveSpawningSystemManager_OnWaveCompleted(object sender, WaveSpawningSystemManager.OnWaveEventArgs e)
    {
        waveCompleted = true;
    }
    #endregion
}
