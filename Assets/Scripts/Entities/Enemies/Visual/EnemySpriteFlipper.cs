using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpriteFlipper : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private EnemyMovement enemyMovement;
    [SerializeField] private EnemySpawningHandler enemySpawningHandler;
    [SerializeField] private EnemyHealth enemyHealth;
    [Space]
    [SerializeField] private SpriteRenderer spriteRenderer;

    private bool facingRight = true;

    private void Update()
    {
        HandleFacingDueToMovement();
    }

    private void HandleFacingDueToMovement()
    {
        if (enemySpawningHandler.IsSpawning) return;
        if (!enemyHealth.IsAlive()) return;

        if (enemyMovement.PlayerOnRight())
        {
            CheckFaceRight();
        }

        if (!enemyMovement.PlayerOnRight())
        {
            CheckFaceLeft();
        }
    }

    private void CheckFaceRight()
    {
        if (facingRight) return;

        LookRight();
        facingRight = true;
    }

    private void CheckFaceLeft()
    {
        if (!facingRight) return;

        LookLeft();
        facingRight = false;
    }

    private void LookRight()
    {
        spriteRenderer.flipX = false;
    }

    private void LookLeft()
    {
        spriteRenderer.flipX = true;
    }
}
