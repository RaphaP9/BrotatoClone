using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootPointHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private EnemyMovement enemyMovement;
    [SerializeField] private EnemyShoot enemyShoot;
    [SerializeField] private EnemySpawningHandler enemySpawningHandler;
    [SerializeField] private EnemyHealth enemyHealth;
    [Space]
    [SerializeField] private Transform shootPoint;

    private Vector3 originalPos;
    private bool facingRight = true;

    private void Start()
    {
        InitializeVariables();
    }

    private void Update()
    {
        HandlePointDueToMovement();
    }

    private void InitializeVariables()
    {
        originalPos = shootPoint.localPosition;
    }

    private void HandlePointDueToMovement()
    {
        if (enemySpawningHandler.IsSpawning) return;
        if (!enemyHealth.IsAlive()) return;
        if (enemyShoot.IsShooting()) return;

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
        shootPoint.localPosition = originalPos;
    }

    private void LookLeft()
    {
        shootPoint.localPosition = new Vector3(-originalPos.x, originalPos.y, originalPos.z);
    }
}
