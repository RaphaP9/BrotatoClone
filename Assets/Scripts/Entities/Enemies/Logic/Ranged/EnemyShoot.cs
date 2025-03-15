using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyShoot : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private EnemyIdentifier enemyIdentifier;
    [SerializeField] private EnemyRangedMovement enemyRangedMovement;
    [SerializeField] protected EnemySpawningHandler enemySpawningHandler;
    [SerializeField] private EnemyHealth enemyHealth;
    [Space]
    [SerializeField] private Transform projectilePrefab;
    [SerializeField] private Transform shootPoint;

    [Header("States")]
    [SerializeField] private State state;

    [Header("Debug")]
    [SerializeField] private bool debug;

    public enum State { NotShooting, Aiming, Shooting, PostShot}
    public State ShootState => state;

    private RangedEnemySO RangedEnemySO => enemyIdentifier.EnemySO as RangedEnemySO;

    private float timer;

    public static event EventHandler<OnEnemyShootEventArgs> OnEnemyAim;
    public static event EventHandler<OnEnemyShootEventArgs> OnEnemyShoot;
    public static event EventHandler<OnEnemyShootEventArgs> OnEnemyPostShoot;
    public static event EventHandler<OnEnemyShootEventArgs> OnEnemyStopShooting;

    public event EventHandler<OnEnemyShootEventArgs> OnThisEnemyAim;
    public event EventHandler<OnEnemyShootEventArgs> OnThisEnemyShoot;
    public event EventHandler<OnEnemyShootEventArgs> OnThisEnemyPostShoot;
    public event EventHandler<OnEnemyShootEventArgs> OnThisEnemyStopShooting;

    public class OnEnemyShootEventArgs : EventArgs
    {
        public RangedEnemySO rangedEnemySO;
        public Transform shootPoint;
    }

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
        switch (state)
        {
            case State.NotShooting:
                NotShootingLogic();
                break;
            case State.Aiming:
                AimingLogic();
                break;
            case State.Shooting:
                ShootingLogic();
                break;
            case State.PostShot:
                PostShotLogic();
                break;
        }
    }

    private void NotShootingLogic()
    {
        if (enemyRangedMovement.InPreferredDistance())
        {
            SetShootState(State.Aiming);
        }

        ResetTimer();
    }

    private void AimingLogic()
    {
        if(timer < RangedEnemySO.aimingTime)
        {
            if (!CriticalCanShoot())
            {
                SetShootState(State.NotShooting);
                ResetTimer();
                return;
            }

            timer += Time.deltaTime;
            return;
        }

        ShootProjectile(projectilePrefab,shootPoint);
        SetShootState(State.Shooting);
   
        ResetTimer();
    }

    private void ShootingLogic()
    {
        if (timer < RangedEnemySO.shootingTime)
        {
            if (!CriticalCanShoot())
            {
                SetShootState(State.NotShooting);
                ResetTimer();
                return;
            }

            timer += Time.deltaTime;
            return;
        }

        SetShootState(State.PostShot);

        OnEnemyPostShoot?.Invoke(this, new OnEnemyShootEventArgs { rangedEnemySO = RangedEnemySO, shootPoint = shootPoint });
        OnThisEnemyPostShoot?.Invoke(this, new OnEnemyShootEventArgs { rangedEnemySO = RangedEnemySO, shootPoint = shootPoint });

        ResetTimer();
    }

    private void PostShotLogic()
    {
        if (timer < RangedEnemySO.postShootTime)
        {
            if (!CriticalCanShoot())
            {
                SetShootState(State.NotShooting);
                ResetTimer();
                return;
            }

            timer += Time.deltaTime;
            return;
        }

        if (enemyRangedMovement.InStillDistance())
        {
            SetShootState(State.Aiming);
            OnEnemyAim?.Invoke(this, new OnEnemyShootEventArgs { rangedEnemySO = RangedEnemySO, shootPoint = shootPoint });
            OnThisEnemyAim?.Invoke(this, new OnEnemyShootEventArgs { rangedEnemySO = RangedEnemySO, shootPoint = shootPoint });
        }
        else
        {
            SetShootState(State.NotShooting);
            OnEnemyStopShooting?.Invoke(this, new OnEnemyShootEventArgs { rangedEnemySO = RangedEnemySO, shootPoint = shootPoint });
            OnThisEnemyStopShooting?.Invoke(this, new OnEnemyShootEventArgs { rangedEnemySO = RangedEnemySO, shootPoint = shootPoint });
        }

        ResetTimer();
    }

    private bool CanShoot()
    {
        if (enemySpawningHandler.IsSpawning) return false;
        if (!enemyHealth.IsAlive()) return false;

        return true;
    }

    private bool CriticalCanShoot()
    {
        if (enemySpawningHandler.IsSpawning) return false;
        if (!enemyHealth.IsAlive()) return false;

        return true;
    }


    public bool IsShooting()
    {
        if(state == State.NotShooting) return false;
        return true;
    }
    private void SetShootState(State state) => this.state = state;
    private void ResetTimer() => timer = 0f;

    ////////////////////////////////////////////////

    protected void ShootProjectile(Transform projectilePrefab, Transform firePoint)
    {
        bool isCrit = GeneralGameplayUtilities.EvaluateCritAttack(RangedEnemySO.critChance);

        Vector2 shootDirection = CalculateShootDirection();
        Vector2 shootPosition = GeneralUtilities.TransformPositionVector2(firePoint);

        CreateProjectile(projectilePrefab, shootPosition, shootDirection, isCrit);

        OnEnemyShoot?.Invoke(this, new OnEnemyShootEventArgs { rangedEnemySO = RangedEnemySO, shootPoint = shootPoint });
        OnThisEnemyShoot?.Invoke(this, new OnEnemyShootEventArgs { rangedEnemySO = RangedEnemySO, shootPoint = shootPoint });
    }

    protected void CreateProjectile(Transform projectile, Vector2 position, Vector2 shootDirection, bool isCrit)
    {
        Vector3 vector3Position = GeneralUtilities.Vector2ToVector3(position);
        Transform instantiatedProjectile = Instantiate(projectile, vector3Position, Quaternion.identity);

        ProjectileHandler projectileHandler = instantiatedProjectile.GetComponent<ProjectileHandler>();

        if (projectileHandler == null)
        {
            if (debug) Debug.Log("Instantiated projectile does not contain a ProjectileHandler component. Set will be ignored.");
            return;
        }

        Vector2 processedShootDirection = GeneralGameplayUtilities.DeviateShootDirection(shootDirection, RangedEnemySO.dispersionAngle);

        projectileHandler.SetProjectile(RangedEnemySO.projectileSpeed, RangedEnemySO.projectileRange, RangedEnemySO.projectileRegularDamage,
            RangedEnemySO.projectileBleedDamage, RangedEnemySO.projectileBleedDuration, RangedEnemySO.projectileBleedTickTime, RangedEnemySO.critChance, RangedEnemySO.critDamageMultiplier, RangedEnemySO.projectileDamageType,
            RangedEnemySO.projectileArea, RangedEnemySO, processedShootDirection, isCrit);
    }

    private Vector2 CalculateShootDirection()
    {
        Vector2 direction = GeneralUtilities.TransformPositionVector2(PlayerPositionHandler.Instance.Player) - GeneralUtilities.TransformPositionVector2(shootPoint);
        direction.Normalize();
        return direction;
    }

}
