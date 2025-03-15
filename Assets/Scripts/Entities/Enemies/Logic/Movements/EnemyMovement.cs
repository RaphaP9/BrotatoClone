using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] protected EnemyIdentifier enemyIdentifier;
    [SerializeField] protected EnemyHealth enemyHealth;
    [SerializeField] protected EnemySpawningHandler spawningHandler;

    private Rigidbody2D _rigidbody2D;

    protected virtual void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    protected virtual bool CanMove()
    {
        if (spawningHandler.IsSpawning) return false;
        if (!enemyHealth.IsAlive()) return false;

        return true;
    }

    protected void MoveTowardsDirection(Vector2 direction, float speed)
    {
        Vector2 normalizedDirection = direction.normalized;
        _rigidbody2D.velocity = normalizedDirection * speed;
    }

    protected Vector2 GetNormalizedDirectionToPlayer()
    {
        Vector2 directionVector = GeneralUtilities.TransformPositionVector2(PlayerPositionHandler.Instance.Player) - GeneralUtilities.TransformPositionVector2(transform);
        directionVector.Normalize();

        return directionVector;
    }


    protected void StopMovement()
    {
        _rigidbody2D.velocity = Vector2.zero;
    }
    public bool PlayerOnRight()
    {
        Vector2 movementDirectionVector = GetNormalizedDirectionToPlayer();

        if (movementDirectionVector.x > 0) return true;
        return false;
    }

    public float GetSpeed()
    {
        Vector2 speedVector = _rigidbody2D.velocity;
        float speed = speedVector.magnitude;
        return speed;
    }

}
