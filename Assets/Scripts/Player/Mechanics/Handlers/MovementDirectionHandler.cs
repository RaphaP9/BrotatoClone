using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementDirectionHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private MovementInput movementInput;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private Rigidbody2D _rigidbody2D;

    [Header("Settings")]
    [SerializeField] private EvaluationMode evaluationMode;
    [SerializeField] private Vector2 lastMovementDirection;
    [SerializeField] private Vector2 defaultStartingMovementDirection;

    public Vector2 LastMovementDirection => lastMovementDirection;

    private enum EvaluationMode { RigidbodyVelocity, LastNonZeroInput, CurrentDirectionInput }


    private void Start()
    {
        InitializeVariables();
    }

    private void Update()
    {
        HandleLastMovementDirection();
    }

    private void InitializeVariables()
    {
        lastMovementDirection = defaultStartingMovementDirection.normalized;
    }

    private void HandleLastMovementDirection()
    {
        Vector2 valueToEvaluate;

        switch (evaluationMode)
        {
            case EvaluationMode.RigidbodyVelocity:
            default:
                valueToEvaluate = _rigidbody2D.velocity;
                break;
            case EvaluationMode.LastNonZeroInput:
                valueToEvaluate = playerMovement.LastNonZeroInput;
                break;
            case EvaluationMode.CurrentDirectionInput:
                valueToEvaluate = movementInput.GetMovementInputNormalized();
                break;

        }

        Vector2 rawLastMovementDirection = valueToEvaluate != Vector2.zero? valueToEvaluate : lastMovementDirection;

        lastMovementDirection = rawLastMovementDirection.normalized;
    }
}
