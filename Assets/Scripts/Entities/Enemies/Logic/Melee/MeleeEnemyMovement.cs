using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyMovement : EnemyMovement
{
    [Header("Components")]
    [SerializeField] private EnemyAttack enemyAttack;

    [Header("States")]
    [SerializeField] private State state;

    public enum State { StartingStill, Still, TowardsPlayer }
    public State MovingState => state;

    private MeleeEnemySO MeleeEnemySO => enemyIdentifier.EnemySO as MeleeEnemySO;

    private const float ATTACK_DISTANCE_THRESHOLD = 0.1f;

    private void Start()
    {
        SetMovingState(State.StartingStill);
    }

    private void Update()
    {
        HandleMovingState();
    }

    private void HandleMovingState()
    {
        switch (state)
        {
            case State.StartingStill:
                StartingStillLogic();
                break;
            case State.Still:
                StillLogic();
                break;
            case State.TowardsPlayer:
                TowardsPlayerLogic();
                break;
        }
    }

    private void StartingStillLogic()
    {
        StopMovement();

        if (!CanMove()) return;

        if (!InAttackDistance())
        {
            SetMovingState(State.TowardsPlayer);
            return;
        }
    }

    private void StillLogic()
    {
        StopMovement();

        if (!CanMove()) return;

        if (!InAttackDistance())
        {
            SetMovingState(State.TowardsPlayer);
            return;
        }
    }

    private void TowardsPlayerLogic()
    {
        Vector2 targetAttackPosition = CalculateAttackPosition();
        MoveTowardsPosition(targetAttackPosition, MeleeEnemySO.moveSpeed);

        if (InAttackDistance())
        {
            SetMovingState(State.Still);
            return;
        }

        if (!CanMove())
        {
            SetMovingState(State.Still);
            return;
        }
    }

    public bool InAttackDistance()
    {
        if (GetDistanceToPlayer() > MeleeEnemySO.attackDistance) return false;
        return true;
    }

    private Vector2 CalculateAttackPosition()
    {
        Vector2 playerToEnemyVector = -GetNormalizedDirectionToPlayer();
        Vector2 targetPosition = GetPlayerPosition() + playerToEnemyVector * MeleeEnemySO.attackDistance;

        return targetPosition;
    }

    protected override bool CanMove()
    {
        if (enemyAttack.IsAttacking()) return false;
        if (spawningHandler.IsSpawning) return false;
        if (!enemyHealth.IsAlive()) return false;

        return true;
    }

    private void SetMovingState(State state) => this.state = state;

    public bool IsStill() => state == State.Still;
}
