using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {  get; private set; }
    public static int LastWave;

    [Header("States")]
    [SerializeField] private State state;
    [SerializeField] private State previousState;

    [Header("Settings")]
    [SerializeField, Range (0.5f,5f)] private float startingGameTimer;
    [SerializeField, Range(1f, 5f)] private float startingWaveTimer;
    [SerializeField, Range(1f, 5f)] private float endingWaveTimer;
    [SerializeField, Range(0.5f, 5f)] private float dialogueInterval;
    [Space]
    [SerializeField] private bool infiniteWaves;
    [SerializeField] private bool ignoreGameFlow;
    [Space]
    [SerializeField] private bool allowActionsWhileStartingGame;

    [Header("Lose")]
    [SerializeField,Range(1f, 3f)] private float timeToEndAfterLose;
    [SerializeField] private string loseScene;

    [Header("Win")]
    [SerializeField, Range(1f, 3f)] private float timeToEndAfterWin;
    [SerializeField] private string winScene;

    public enum State {StartingGame, StartingWave, Wave, EndingWave, Shop, Lose, Dialogue, Win}

    public State GameState => state;
    public bool AllowActionsWhileStartingGame => allowActionsWhileStartingGame;

    public static event EventHandler<OnStateEventArgs> OnStateChanged;
    public static event EventHandler OnGameLost;
    public static event EventHandler OnGameWon;

    #region Flags
    private bool firstUpdateLogicCompleted = false;
    private bool waveCompleted = false;
    private bool shopClosed = false;
    private bool dialogueConcluded = false;
    #endregion

    public class OnStateEventArgs : EventArgs
    {
        public State newState;
    }

    private void OnEnable()
    {
        GeneralWavesManager.OnWaveCompleted += GeneralWavesManager_OnWaveCompleted;
        ShopOpeningManager.OnShopClose += ShopOpeningManager_OnShopClose;

        DialogueManager.OnGeneralDialogueConcluded += DialogueManager_OnGeneralDialogueConcluded;

        PlayerHealth.OnPlayerDeath += PlayerHealth_OnPlayerDeath;
    }

    private void OnDisable()
    {
        GeneralWavesManager.OnWaveCompleted -= GeneralWavesManager_OnWaveCompleted;
        ShopOpeningManager.OnShopClose -= ShopOpeningManager_OnShopClose;

        DialogueManager.OnGeneralDialogueConcluded -= DialogueManager_OnGeneralDialogueConcluded;

        PlayerHealth.OnPlayerDeath -= PlayerHealth_OnPlayerDeath;
    }

    private void Awake()
    {
        SetSingleton();
        ResetWavesSurvived();
    }

    private void Update()
    {
        FirstUpdateLogic();
    }

    private void SetLastWaveSurvived(int wave) => LastWave = wave;
    private void ResetWavesSurvived() => LastWave = 1;

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
        if (ignoreGameFlow)
        {
            ChangeState(State.Wave);
            yield break;
        }

        ChangeState(State.StartingGame);
        yield return new WaitForSeconds(startingGameTimer);

        bool gameEnded = false;

        CharacterSO characterSO = PlayerIdentifier.Instance.CharacterSO;
        int waveNumber = GeneralWavesManager.Instance.CurrentWaveNumber;

        while (!gameEnded)
        {
            #region PreCombat Dialogue Logic
            if (DialogueTriggerHandler.Instance.ExistDialogueWithConditions(characterSO, waveNumber, DialogueChronology.PreCombat))
            {
                ChangeState(State.Dialogue);

                yield return new WaitForSeconds(dialogueInterval);
                dialogueConcluded = false;
                DialogueTriggerHandler.Instance.PlayDialogueWithConditions(characterSO, waveNumber, DialogueChronology.PreCombat);
                yield return new WaitUntil(() => dialogueConcluded);
                dialogueConcluded = false;
                yield return new WaitForSeconds(dialogueInterval);
            }
            #endregion

            SetLastWaveSurvived(waveNumber);

            #region Wave Logic
            ChangeState(State.StartingWave);
            yield return new WaitForSeconds(startingWaveTimer);

            ChangeState(State.Wave);
            waveCompleted = false;
            GeneralWavesManager.Instance.StartNextWave();
            yield return new WaitUntil(() => waveCompleted);
            waveCompleted = false;

            ChangeState(State.EndingWave);
            yield return new WaitForSeconds(endingWaveTimer);
            #endregion

            #region PostCombat Dialogue Logic
            if (DialogueTriggerHandler.Instance.ExistDialogueWithConditions(characterSO, waveNumber, DialogueChronology.PostCombat))
            {
                ChangeState(State.Dialogue);

                yield return new WaitForSeconds(dialogueInterval);
                dialogueConcluded = false;
                DialogueTriggerHandler.Instance.PlayDialogueWithConditions(characterSO, waveNumber, DialogueChronology.PostCombat);
                yield return new WaitUntil(() => dialogueConcluded);
                dialogueConcluded = false;
                yield return new WaitForSeconds(dialogueInterval);
            }
            #endregion

            GeneralWavesManager.Instance.IncreaseCurrentWaveNumber();
            waveNumber = GeneralWavesManager.Instance.CurrentWaveNumber;

            #region Win Logic
            if (!infiniteWaves)
            {
                if (!GeneralWavesManager.Instance.WaveWithWaveNumberExists(waveNumber))
                {
                    gameEnded = true;
                    break;
                }
            }
            #endregion

            #region Shop Logic
            ChangeState(State.Shop);
            shopClosed = false;
            ShopOpeningManager.Instance.OpenShop();
            yield return new WaitUntil(() => shopClosed);
            shopClosed = false;
            #endregion
        }

        SetGameState(State.Win);
        OnGameWon?.Invoke(this, EventArgs.Empty);

        yield return WinGameCoroutine();
    }

    #endregion

    private void LoseGame()
    {
        StopAllCoroutines();
        SetGameState(State.Lose);
        OnGameLost?.Invoke(this, EventArgs.Empty);
        StartCoroutine(LoseGameCoroutine());
    }

    private IEnumerator LoseGameCoroutine()
    {
        yield return new WaitForSeconds(timeToEndAfterLose);
        ScenesManager.Instance.FadeLoadTargetScene(loseScene);
    }

    private IEnumerator WinGameCoroutine()
    {
        yield return new WaitForSeconds(timeToEndAfterWin);
        ScenesManager.Instance.FadeLoadTargetScene(winScene);
    }

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

    #region Dialogue Subscriptions
    private void DialogueManager_OnGeneralDialogueConcluded(object sender, EventArgs e)
    {
        dialogueConcluded = true;
    }

    private void PlayerHealth_OnPlayerDeath(object sender, EntityHealth.OnEntityDeathEventArgs e)
    {
        LoseGame();
    }
    #endregion
}
