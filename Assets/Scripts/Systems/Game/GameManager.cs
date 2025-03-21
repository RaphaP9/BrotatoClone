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

    public enum State { OnWave, OnShop, OnAugment, OnLose }

    public State GameState => state;

    public static event EventHandler<OnStateEventArgs> OnStateChanged;

    public class OnStateEventArgs : EventArgs
    {
        public State newState;
    }

    private void OnEnable()
    {
        GeneralWavesManager.OnWaveStart += GeneralWavesManager_OnWaveStart;
        GeneralWavesManager.OnWaveEnd += GeneralWavesManager_OnWaveEnd;

        ShopOpeningManager.OnShopOpen += ShopOpeningManager_OnShopOpen;
        ShopOpeningManager.OnShopClose += ShopOpeningManager_OnShopClose;
    }

    private void OnDisable()
    {
        GeneralWavesManager.OnWaveStart -= GeneralWavesManager_OnWaveStart;
        GeneralWavesManager.OnWaveEnd -= GeneralWavesManager_OnWaveEnd;

        ShopOpeningManager.OnShopOpen -= ShopOpeningManager_OnShopOpen;
        ShopOpeningManager.OnShopClose -= ShopOpeningManager_OnShopClose;
    }

    private void Awake()
    {
        SetSingleton();
    }

    private void Start()
    {
        ChangeState(State.OnWave);
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

    #region GeneralWavesManager Subscriptions
    private void GeneralWavesManager_OnWaveStart(object sender, GeneralWavesManager.OnWaveEventArgs e)
    {
        //ChangeState(State.OnWave);
    }

    private void GeneralWavesManager_OnWaveEnd(object sender, GeneralWavesManager.OnWaveEventArgs e)
    {
        //HandleLogicFor ElementAugmentAppeareance or Shop
        ChangeState(State.OnShop);
    }
    #endregion

    #region ShopOpeningManager Subscriptions
    private void ShopOpeningManager_OnShopOpen(object sender, EventArgs e)
    {
        //ChangeState(State.OnShop);
    }

    private void ShopOpeningManager_OnShopClose(object sender, EventArgs e)
    {
        ChangeState(State.OnWave);
    }
    #endregion
}
