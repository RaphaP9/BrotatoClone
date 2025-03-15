using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangedMovement : EnemyMovement
{
    [Header("Components")]
    [SerializeField] private EnemyShoot enemyShoot;

    [Header("States")]
    [SerializeField] private State state;

    public enum State { Still, TowardsPreferredPoint}
    public State MovingState => state;

    private RangedEnemySO RangedEnemySO => enemyIdentifier.EnemySO as RangedEnemySO;

    private const float PREFERRED_POSITION_THRESHOLD = 0.1f;

    private void Start()
    {
        SetMovingState(State.Still);
    }

    private void Update()
    {
        HandleMovingState();
    }

    private void HandleMovingState()
    {
        switch (state)
        {
            case State.Still:
                StillLogic();
                break;
            case State.TowardsPreferredPoint:
                TowardsPreferredPointLogic();
                break;
        }
    }

    private void StillLogic()
    {
        if (!CanMove()) return;

        StopMovement();

        if (!InStillDistance())
        {          
            SetMovingState(State.TowardsPreferredPoint);
            return;
        } 
    }

    private void TowardsPreferredPointLogic()
    {
        Vector2 targetPreferredPosition = CalculatePreferredPosition();
        MoveTowardsPosition(targetPreferredPosition, RangedEnemySO.moveSpeed);

        if (InPreferredDistance())
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

    private bool InStillDistance()
    {
        if (GetDistanceToPlayer() < RangedEnemySO.tooCloseDistance) return false;
        if (GetDistanceToPlayer() > RangedEnemySO.tooFarDistance) return false;

        return true;
    }

    private bool InPreferredDistance()
    {
        if (Mathf.Abs(GetDistanceToPlayer()-RangedEnemySO.preferredDistance) > PREFERRED_POSITION_THRESHOLD ) return false;
        return true;
    }

    private Vector2 CalculatePreferredPosition()
    {
        Vector2 playerToEnemyVector = -GetNormalizedDirectionToPlayer();
        Vector2 targetPosition = GetPlayerPosition() + playerToEnemyVector* RangedEnemySO.preferredDistance;

        return targetPosition;
    }

    protected override bool CanMove()
    {
        if (enemyShoot.IsShooting()) return false;
        if (spawningHandler.IsSpawning) return false;
        if (!enemyHealth.IsAlive()) return false;

        return true;
    }

    private void SetMovingState(State state) => this.state = state;

    public bool IsStill() => state == State.Still;
}
