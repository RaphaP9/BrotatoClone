using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance {  get; private set; }

    [Header("Enabler")]
    [SerializeField] private bool movementEnabled;

    [Header("Components")]
    [SerializeField] private MovementInput movementInput;
    [Space]
    [SerializeField] private CheckWall checkWall;
    [SerializeField] private PlayerDash playerDash;

    [Header("Movement Settings")]
    [SerializeField,Range(1f, 10f)] private float moveSpeed;

    [Header("Smooth Settings")]
    [SerializeField, Range(1f, 100f)] private float smoothVelocityFactor = 5f;
    [SerializeField, Range(1f, 100f)] private float smoothDirectionFactor = 5f;

    private Rigidbody2D _rigidbody2D;

    public Vector2 DirectionInput => movementInput.GetMovementInputNormalized();

    public float DesiredSpeed { get; private set; }
    public float SmoothCurrentSpeed { get; private set; }

    public Vector2 SmoothDirectionInput { get; private set; }
    public Vector2 LastNonZeroInput { get; private set; }
    public Vector2 FinalMoveValue { get; private set; }
    public bool MovementEnabled => movementEnabled;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        SetSingleton();
    }

    private void Update()
    {
        HandleMovement();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one PlayerMovement instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    private void HandleMovement()
    {
        if (!movementEnabled) return;

        CalculateDesiredSpeed();
        SmoothSpeed();

        CalculateLastNonZeroDirection();
        SmoothDirection();

        CalculateFinalMovement();
    }

    private void CalculateDesiredSpeed()
    {
        DesiredSpeed = CanMove() ? moveSpeed : 0f;
    }

    private bool CanMove()
    {
        if (DirectionInput == Vector2.zero) return false;
        if (checkWall.HitWall) return false;

        return true;
    }

    private void SmoothSpeed()
    {
        SmoothCurrentSpeed = Mathf.Lerp(SmoothCurrentSpeed, DesiredSpeed, Time.deltaTime * smoothVelocityFactor);
    }

    private void CalculateLastNonZeroDirection() => LastNonZeroInput = DirectionInput != Vector2.zero ? DirectionInput : LastNonZeroInput;
    private void SmoothDirection() => SmoothDirectionInput = Vector2.Lerp(SmoothDirectionInput, DirectionInput, Time.deltaTime * smoothDirectionFactor);

    private void CalculateFinalMovement()
    {
        Vector2 finalInput = SmoothDirectionInput * SmoothCurrentSpeed;

        Vector2 roundedFinalInput;
        roundedFinalInput.x = Mathf.Abs(finalInput.x) < 0.01f ? 0f : finalInput.x;
        roundedFinalInput.y = Mathf.Abs(finalInput.y) < 0.01f ? 0f : finalInput.y;

        FinalMoveValue = roundedFinalInput;
    }

    private void ApplyMovement()
    {
        if (playerDash.IsDashing) return;

        _rigidbody2D.velocity = new Vector2(FinalMoveValue.x, FinalMoveValue.y);
    }
}
