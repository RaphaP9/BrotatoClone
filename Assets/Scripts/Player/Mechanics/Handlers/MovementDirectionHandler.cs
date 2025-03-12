using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementDirectionHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Rigidbody2D _rigidbody2D;

    [Header("Settings")]
    [SerializeField] private Vector2 lastMovementDirection;

    public Vector2 LastMovementDirection => lastMovementDirection;
}
