using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerDash : MonoBehaviour
{
    public static PlayerDash Instance { get; private set; }

    [Header("Enabler")]
    [SerializeField] private bool dashEnabled;

    [Header("Components")]
    [SerializeField] private MovementInput movementInput;
    [SerializeField] private MouseDirectionHandler mouseDirectionHandler;
    [SerializeField] private MovementDirectionHandler movementDirectionHandler;
    [SerializeField] private PlayerHealth playerHealth;

    [Header("Settings")]
    [SerializeField, Range(0, 3)] private int dashLimit;
    [Space]
    [SerializeField] private DirectionMode directionMode;
    [Space]
    [SerializeField, Range(1f, 12f)] private float dashDistance;
    [SerializeField, Range(0.1f, 1f)] private float dashTime;
    [SerializeField, Range(0f, 50f)] private float dashResistance;
    [Space]
    [SerializeField, Range(0f, 10f)] private float betweenDashesTime;
    [SerializeField, Range(0f, 10f)] private float dashRefillTime;
    [Space]
    [SerializeField] private bool ghostedWhileDashing;

    [Header("Other")]
    [SerializeField] private int dashesPerformed;
    [SerializeField] private Vector2 currentDashDirection;

    private float dashPerformTimer;
    private float betweenDashesTimer;
    private float dashRefillTimer;

    public bool IsDashing { get; private set; }

    private Rigidbody2D _rigidbody2D;

    private bool DashPressed => movementInput.GetDashDown();
    private bool shouldDash;

    private enum DirectionMode {MousePosition, LastMovementDirection}

    public static event EventHandler<OnPlayerDashEventArgs> OnPlayerDash;
    public static event EventHandler<OnPlayerDashEventArgs> OnPlayerDashPre;
    public static event EventHandler<OnPlayerDashEventArgs> OnPlayerDashStopped;

    public class OnPlayerDashEventArgs : EventArgs
    {
        public Vector2 dashDirection;
        public int dashesPerformed;
        public bool ghostedWhileDashing;
    }

    private void OnEnable()
    {
        DashesStatManager.OnDashesStatInitialized += DashesStatManager_OnDashesStatInitialized;
        DashesStatManager.OnDashesStatUpdated += DashesStatManager_OnDashesStatUpdated;
    }

    private void OnDisable()
    {
        DashesStatManager.OnDashesStatInitialized -= DashesStatManager_OnDashesStatInitialized;
        DashesStatManager.OnDashesStatUpdated -= DashesStatManager_OnDashesStatUpdated;
    }

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        SetSingleton();
    }

    private void Update()
    {
        if (!dashEnabled) return;

        HandleBetweenDashesTimer();
        HandleDashesRefill();

        DashUpdateLogic();
    }

    private void FixedUpdate()
    {
        DashFixedUpdateLogic();
        HandleDashResistance();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one PlayerDash instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    private void DashUpdateLogic()
    {
        HandleDashPerformance();

        if (!CanDash()) return;

        if (DashPressed) shouldDash = true;
    }

    private void HandleDashPerformance()
    {
        if (dashPerformTimer > 0) dashPerformTimer -= Time.deltaTime;
        else if (IsDashing) StopDash();
    }

    private bool CanDash()
    {
        if (!playerHealth.IsAlive()) return false;
        if (!HasDashesLeft()) return false;
        if (InBetweenDashesInCooldown()) return false;

        return true;
    }

    private void DashFixedUpdateLogic()
    {
        if (shouldDash)
        {
            AddDashesPerformed(1);
            Dash();

            shouldDash = false;

            SetDashRefillTimer(dashRefillTime);
            SetTimerBetweenDashes(betweenDashesTime);
            SetDashPerformTimer(dashTime);
        }
    }

    #region RefillDashes
    private void HandleDashesRefill()
    {
        if (dashesPerformed <= 0) return;

        HandleDashesRefillTimer();
    }

    private void HandleDashesRefillTimer()
    {
        if (dashRefillTimer > 0) dashRefillTimer -= Time.deltaTime;
        else RefillDashes();
    }

    private void RefillDashes() => SetDashesPerformed(0);
    private void SetDashRefillTimer(float time) => dashRefillTimer = time;
    #endregion

    #region BetweenDashes
    private void HandleBetweenDashesTimer()
    {
        if (betweenDashesTimer > 0) betweenDashesTimer -= Time.deltaTime;
    }
    private bool InBetweenDashesInCooldown() => betweenDashesTimer > 0;
    private void SetTimerBetweenDashes(float time) => betweenDashesTimer = time;

    #endregion

    #region Dash
    public void Dash()
    {
        OnPlayerDashPre?.Invoke(this, new OnPlayerDashEventArgs { dashesPerformed = dashesPerformed, dashDirection = currentDashDirection, ghostedWhileDashing = ghostedWhileDashing});

        currentDashDirection = DefineDashDirection();

        float dashForce = dashDistance / dashTime;
        Vector2 dashVector = currentDashDirection * dashForce;

        Vector2 scaledDashVector = GeneralGameplayUtilities.ScaleVector2ToPerspective(dashVector);

        _rigidbody2D.velocity = scaledDashVector;
        IsDashing = true;

        OnPlayerDash?.Invoke(this, new OnPlayerDashEventArgs { dashesPerformed = dashesPerformed, dashDirection = currentDashDirection, ghostedWhileDashing = ghostedWhileDashing});
    }

    private void StopDash()
    {
        if (!IsDashing) return;

        _rigidbody2D.velocity = Vector2.zero;
        IsDashing = false;

        OnPlayerDashStopped?.Invoke(this, new OnPlayerDashEventArgs { dashesPerformed = dashesPerformed, dashDirection = currentDashDirection, ghostedWhileDashing = ghostedWhileDashing });

        currentDashDirection = Vector2.zero;
    }

    private Vector2 DefineDashDirection()
    {
        switch (directionMode)
        {
            case DirectionMode.MousePosition:
                return mouseDirectionHandler.NormalizedMouseDirection;
            case DirectionMode.LastMovementDirection:
            default:
                return movementDirectionHandler.LastMovementDirection;
        }
    }
    #endregion

    #region DashResistance
    private void HandleDashResistance()
    {
        if (!IsDashing) return;

        Vector2 dashResistanceForce = -currentDashDirection * dashResistance;
        _rigidbody2D.AddForce(dashResistanceForce, ForceMode2D.Force);   
    }
    #endregion


    private void SetDashPerformTimer(float time) => dashPerformTimer = time;
    private float SetDashesPerformed(int dashesPerformed) => this.dashesPerformed = dashesPerformed;
    private void AddDashesPerformed(int quantity) => dashesPerformed += quantity;
    private bool HasDashesLeft() => dashesPerformed < dashLimit;

    private void SetDashesLimit(int value) => dashLimit = value;

    #region Subscriptions
    private void DashesStatManager_OnDashesStatInitialized(object sender, DashesStatManager.OnDashesStatEventArgs e)
    {
        SetDashesLimit(e.dashesStat);
    }

    private void DashesStatManager_OnDashesStatUpdated(object sender, DashesStatManager.OnDashesStatEventArgs e)
    {
        SetDashesLimit(e.dashesStat);
    }

    #endregion
}
