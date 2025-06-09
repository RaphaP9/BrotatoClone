using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private EnemyIdentifier enemyIdentifier;
    [SerializeField] private MeleeEnemyMovement meleeEnemyMovement;
    [SerializeField] protected EnemySpawningHandler enemySpawningHandler;
    [SerializeField] private EnemyHealth enemyHealth;
    [Space]
    [SerializeField] private List<Transform> attackPoints;

    [Header("Settings")]
    [SerializeField] private LayerMask playerLayerMask;

    [Header("States")]
    [SerializeField] private State state;

    [Header("Debug")]
    [SerializeField] private bool debug;
    [SerializeField] private Color gizmosColor;

    public enum State { NotAttacking, Charging, Attacking, PostAttack }
    public State AttackState => state;

    private MeleeEnemySO MeleeEnemySO => enemyIdentifier.EnemySO as MeleeEnemySO;

    private float timer;

    public static event EventHandler<OnEnemyAttackEventArgs> OnEnemyCharge;
    public static event EventHandler<OnEnemyAttackEventArgs> OnEnemyAttack;
    public static event EventHandler<OnEnemyAttackEventArgs> OnEnemyPostAttack;
    public static event EventHandler<OnEnemyAttackEventArgs> OnEnemyStopAttacking;

    public event EventHandler<OnEnemyAttackEventArgs> OnThisEnemyCharge;
    public event EventHandler<OnEnemyAttackEventArgs> OnThisEnemyAttack;
    public event EventHandler<OnEnemyAttackEventArgs> OnThisEnemyPostAttack;
    public event EventHandler<OnEnemyAttackEventArgs> OnThisEnemyStopAttacking;

    public class OnEnemyAttackEventArgs : EventArgs
    {
        public MeleeEnemySO meleeEnemySO;
        public List<Transform> attackPoints;
    }

    private void Start()
    {
        SetAttackState(State.NotAttacking);
    }

    private void Update()
    {
        HandleAttackState();
    }

    private void HandleAttackState()
    {
        switch (state)
        {
            case State.NotAttacking:
                NotAttackingLogic();
                break;
            case State.Charging:
                ChargingLogic();
                break;
            case State.Attacking:
                AttackingLogic();
                break;
            case State.PostAttack:
                PostAttackLogic();
                break;
        }
    }

    private void NotAttackingLogic()
    {
        if (!CanAttack())
        {
            ResetTimer();
            return;
        }

        if (meleeEnemyMovement.InAttackDistance())
        {
            SetAttackState(State.Charging);
            OnEnemyCharge?.Invoke(this, new OnEnemyAttackEventArgs { meleeEnemySO = MeleeEnemySO, attackPoints = attackPoints });
            OnThisEnemyCharge?.Invoke(this, new OnEnemyAttackEventArgs { meleeEnemySO = MeleeEnemySO, attackPoints = attackPoints });
        }

        ResetTimer();
    }

    private void ChargingLogic()
    {
        if (timer < MeleeEnemySO.chargingTime)
        {
            if (!CanAttack())
            {
                SetAttackState(State.NotAttacking);
                ResetTimer();
                return;
            }

            timer += Time.deltaTime;
            return;
        }

        Attack(attackPoints);
        SetAttackState(State.Attacking);

        ResetTimer();
    }

    private void AttackingLogic()
    {
        if (timer < MeleeEnemySO.attackingTime)
        {
            if (!CanAttack())
            {
                SetAttackState(State.NotAttacking);
                ResetTimer();
                return;
            }

            timer += Time.deltaTime;
            return;
        }

        SetAttackState(State.PostAttack);

        OnEnemyPostAttack?.Invoke(this, new OnEnemyAttackEventArgs { meleeEnemySO = MeleeEnemySO, attackPoints = attackPoints });
        OnThisEnemyPostAttack?.Invoke(this, new OnEnemyAttackEventArgs { meleeEnemySO = MeleeEnemySO, attackPoints = attackPoints });

        ResetTimer();
    }

    private void PostAttackLogic()
    {
        if (timer < MeleeEnemySO.postAttackTime)
        {
            if (!CanAttack())
            {
                SetAttackState(State.NotAttacking);
                ResetTimer();
                return;
            }

            timer += Time.deltaTime;
            return;
        }

        if (meleeEnemyMovement.InAttackDistance())
        {
            SetAttackState(State.Charging);
            OnEnemyCharge?.Invoke(this, new OnEnemyAttackEventArgs { meleeEnemySO = MeleeEnemySO, attackPoints = attackPoints });
            OnThisEnemyCharge?.Invoke(this, new OnEnemyAttackEventArgs { meleeEnemySO = MeleeEnemySO, attackPoints = attackPoints });
        }
        else
        {
            SetAttackState(State.NotAttacking);
            OnEnemyStopAttacking?.Invoke(this, new OnEnemyAttackEventArgs { meleeEnemySO = MeleeEnemySO, attackPoints = attackPoints });
            OnThisEnemyStopAttacking?.Invoke(this, new OnEnemyAttackEventArgs { meleeEnemySO = MeleeEnemySO, attackPoints = attackPoints });
        }

        ResetTimer();
    }

    private bool CanAttack()
    {
        if (enemySpawningHandler.IsSpawning) return false;
        if (!enemyHealth.IsAlive()) return false;

        return true;
    }


    public bool IsAttacking()
    {
        if (state == State.NotAttacking) return false;
        return true;
    }
    private void SetAttackState(State state) => this.state = state;
    private void ResetTimer() => timer = 0f;

    ////////////////////////////////////////////////

    protected void Attack(List<Transform> attackPoints)
    {
        bool isCrit = GeneralGameplayUtilities.EvaluateCritAttack(MeleeEnemySO.critChance);

        int damage = MeleeEnemySO.attackRegularDamage;
        int bleedDamage = MeleeEnemySO.attackBleedDamage;
        float areaRadius = MeleeEnemySO.attackArea;

        List<Vector2> positions = GeneralUtilities.TransformPositionVector2List(attackPoints);

        if (isCrit)
        {
            damage = GeneralGameplayUtilities.CalculateCritDamage(damage, MeleeEnemySO.critDamageMultiplier);
            bleedDamage = GeneralGameplayUtilities.CalculateCritDamage(bleedDamage, MeleeEnemySO.critDamageMultiplier);
        }

        OnEnemyAttack?.Invoke(this, new OnEnemyAttackEventArgs { meleeEnemySO = MeleeEnemySO, attackPoints = attackPoints });
        OnThisEnemyAttack?.Invoke(this, new OnEnemyAttackEventArgs { meleeEnemySO = MeleeEnemySO, attackPoints = attackPoints });

        if (damage>0 && bleedDamage>0)
        {
            GeneralGameplayUtilities.DealRegularAndBleedDodgeableDamageInArea(damage, bleedDamage, MeleeEnemySO.attackBleedDuration, MeleeEnemySO.attackBleedTickTime, positions, areaRadius, isCrit, playerLayerMask, MeleeEnemySO);
            return;
        }

        if (damage > 0)
        {
            GeneralGameplayUtilities.DealDodgeableRegularDamageInArea(damage, positions, areaRadius, isCrit, playerLayerMask, MeleeEnemySO);
            return;
        }

        if (bleedDamage > 0)
        {
            GeneralGameplayUtilities.DealDodgeableBleedDamageInArea(bleedDamage, MeleeEnemySO.attackBleedDuration, MeleeEnemySO.attackBleedTickTime, positions, areaRadius, isCrit, playerLayerMask, MeleeEnemySO);

            return;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmosColor;

        foreach (Transform attackPoint in attackPoints)
        {
            Gizmos.DrawWireSphere(attackPoint.position, MeleeEnemySO.attackArea);
        }
    }
}
