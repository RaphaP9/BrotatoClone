using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseDirectionHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private ScreenInput screenInput;

    [Header("Settings")]
    [SerializeField] private Vector2 normalizedMouseDirection;

    public Vector2 NormalizedMouseDirection => normalizedMouseDirection;

    private void Update()
    {
        HandleMouseDirection();
    }

    private void HandleMouseDirection()
    {
        Vector2 rawDirection = screenInput.GetWorldMousePosition() - GeneralMethods.TransformPositionVector2(transform);
        normalizedMouseDirection = rawDirection.normalized;
    }
}
