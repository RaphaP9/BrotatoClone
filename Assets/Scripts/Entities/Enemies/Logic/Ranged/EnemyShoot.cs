using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private EnemyIdentifier enemyIdentifier;
    [SerializeField] private EnemyMovement enemyMovement;
    [SerializeField] private EnemyHealth enemyHealth;
    [Space]
    [SerializeField] private Transform shootPoint;

    [Header("States")]
    [SerializeField] private State state;

    public enum State { NotShooting, Aiming, Shooting, PostShot}
    public State ShootState => state;

    private RangedEnemySO RangedEnemySO => enemyIdentifier.EnemySO as RangedEnemySO;

    private float timer;

    private void Start()
    {
        SetShootState(State.NotShooting);
    }

    private void Update()
    {
        HandleShootState();
    }

    private void HandleShootState()
    {

    }

    private void NotShootingLogic()
    {
        //
    }

    private void AimingLogic()
    {

    }

    private void ShootingLogic()
    {

    }

    private void PostShotLogic()
    {

    }

    private bool CanShoot()
    {
        if (!enemyHealth.IsAlive()) return false;
        if (!PlayerInShootRange()) return false;

        return true;
    }

    private bool PlayerInShootRange()
    {
        if(enemyMovement.GetDistanceToPlayer() < RangedEnemySO.minShootDistance) return false;
        if(enemyMovement.GetDistanceToPlayer() > RangedEnemySO.maxShootDistance) return false;

        return true;
    }

    private void SetShootState(State state) => this.state = state;

    public bool IsShooting() => state != State.NotShooting;
}
