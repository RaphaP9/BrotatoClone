using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {  get; private set; }

    [Header("States")]
    [SerializeField] private State state;
    [SerializeField] private State previousState;

    [Header("Settings")]
    [SerializeField, Range (2f,5f)] private float startingGameTimer;
    [SerializeField, Range(2f, 5f)] private float startingWaveTimer;
    [SerializeField, Range(2f, 5f)] private float endingWaveTimer;

    public enum State {StartingGame, StartingWave, Wave, EndingWave, Shop, Lose}

    public State GameState => state;

    public static event EventHandler<OnStateEventArgs> OnStateChanged;

    #region Flags
    private bool firstUpdateLogicCompleted = false;
    private bool waveCompleted = false;
    private bool shopClosed = false;
    #endregion

    public class OnStateEventArgs : EventArgs
    {
        public State newState;
    }

    private void OnEnable()
    {
        GeneralWavesManager.OnWaveCompleted += GeneralWavesManager_OnWaveCompleted;
        ShopOpeningManager.OnShopClose += ShopOpeningManager_OnShopClose;
    }

    private void OnDisable()
    {
        GeneralWavesManager.OnWaveCompleted -= GeneralWavesManager_OnWaveCompleted;
        ShopOpeningManager.OnShopClose -= ShopOpeningManager_OnShopClose;
    }

    private void Awake()
    {
        SetSingleton();
    }

    private void Update()
    {
        FirstUpdateLogic();
    }

    private void FirstUpdateLogic()
    {
        if (firstUpdateLogicCompleted) return;

        StartCoroutine(GameCoroutine());
        firstUpdateLogicCompleted = true;
    }

    private void SetSingleton()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one GameManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    #region States
    private void SetGameState(State state)
    {
        SetPreviousState(this.state);
        this.state = state;
    }

    private void SetPreviousState(State state)
    {
        previousState = state;
    }

    private void ChangeState(State state)
    {
        SetGameState(state);
        OnStateChanged?.Invoke(this, new OnStateEventArgs { newState = state });
    }
    #endregion

    #region Logic
    private IEnumerator GameCoroutine()
    {
        ChangeState(State.StartingGame);
        yield return new WaitForSeconds(startingGameTimer);

        bool gameEnded = false;

        while (!gameEnded)
        {
            ChangeState(State.StartingWave);
            yield return new WaitForSeconds(startingWaveTimer);

            ChangeState(State.Wave);
            waveCompleted = false;
            GeneralWavesManager.Instance.StartNextWave();
            yield return new WaitUntil(() => waveCompleted);
            waveCompleted = false;

            ChangeState(State.EndingWave);
            yield return new WaitForSeconds(endingWaveTimer);

            GeneralWavesManager.Instance.IncreaseCurrentWaveNumber();

            ChangeState(State.Shop);
            shopClosed = false;
            ShopOpeningManager.Instance.OpenShop();
            yield return new WaitUntil(() => shopClosed);
            shopClosed = false;
        }
    }

    #endregion

    #region GeneralWavesManager Subscriptions
    private void GeneralWavesManager_OnWaveCompleted(object sender, GeneralWavesManager.OnWaveEventArgs e)
    {
        waveCompleted = true;
    }
    #endregion

    #region ShopOpeningManager Subscriptions
    private void ShopOpeningManager_OnShopClose(object sender, EventArgs e)
    {
        shopClosed = true;
    }
    #endregion
}
