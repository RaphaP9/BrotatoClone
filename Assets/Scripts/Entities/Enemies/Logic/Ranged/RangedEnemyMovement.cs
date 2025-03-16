using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyMovement : EnemyMovement
{
    [Header("Components")]
    [SerializeField] private EnemyShoot enemyShoot;

    [Header("States")]
    [SerializeField] private State state;

    public enum State {StartingStill, Still, TowardsPreferredPoint}
    public State MovingState => state;

    private RangedEnemySO RangedEnemySO => enemyIdentifier.EnemySO as RangedEnemySO;

    private const float PREFERRED_POSITION_THRESHOLD = 0.1f;

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
            case State.TowardsPreferredPoint:
                TowardsPreferredPointLogic();
                break;
        }
    }
    private void StartingStillLogic()
    {
        StopMovement();

        if (!CanMove()) return;

        if (!InPreferredDistance())
        {
            SetMovingState(State.TowardsPreferredPoint);
            return;
        }
    }

    private void StillLogic()
    {
        StopMovement();

        if (!CanMove()) return;

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

    public bool InStillDistance()
    {
        if (GetDistanceToPlayer() < RangedEnemySO.tooCloseDistance) return false;
        if (GetDistanceToPlayer() > RangedEnemySO.tooFarDistance) return false;

        return true;
    }

    public bool InPreferredDistance()
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
