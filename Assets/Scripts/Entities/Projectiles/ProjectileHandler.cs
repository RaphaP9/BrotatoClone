using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AttackBasedWeaponHandler;

public class ProjectileHandler : MonoBehaviour
{
    [Header("Identifiers")]
    [SerializeField] private int id;

    [Header("Settings")]
    [SerializeField] private LayerMask targetLayermask;
    [SerializeField] private LayerMask impactLayermask;

    [Header("Runtime Filled Settings")]
    [SerializeField, Range(5f, 15f)] private float speed;
    [SerializeField, Range(5f, 10f)] private float lifespan;
    [Space]
    [SerializeField] private int regularDamage;
    [Space]
    [SerializeField] private int bleedDamage;
    [SerializeField] private float bleedDuration;
    [SerializeField] private float bleedTickTime;
    [Space]
    [SerializeField] private float critChance;
    [SerializeField] private float critDamageMultiplier;
    [Space]
    [SerializeField] private ProjectileDamageType projectileDamageType;
    [SerializeField] private float projectileArea;
    [Space]
    [SerializeField] private Vector2 direction;

    private IDamageDealer projectileSource;

    private Rigidbody2D _rigidbody2D;

    public int ID => id;

    public static event EventHandler<OnProjectileEventArgs> OnProjectileImpact;
    public static event EventHandler<OnProjectileEventArgs> OnProjectileLifespanEnd;

    public class OnProjectileEventArgs : EventArgs
    {
        public int id;
    }

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        StartCoroutine(LifespanCoroutine());
    }

    private void Update()
    {
        HandleMovement();
    }

    private IEnumerator LifespanCoroutine()
    {
        yield return new WaitForSeconds(lifespan);

        EndLifespan();
    }

    private void HandleMovement()
    {
        _rigidbody2D.velocity = direction * speed;
    }

    private void EndLifespan()
    {
        OnProjectileLifespanEnd?.Invoke(this, new OnProjectileEventArgs { id = id });
        Destroy(gameObject);
    }

    private void ImpactProjectile()
    {
        OnProjectileImpact?.Invoke(this, new OnProjectileEventArgs { id = id });
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!GeneralUtilities.CheckGameObjectInLayerMask(collision.gameObject, impactLayermask)) return;

        switch (projectileDamageType)
        {
            case ProjectileDamageType.Singular:
            default:
                if (!GeneralUtilities.CheckGameObjectInLayerMask(collision.gameObject, targetLayermask)) return;
                DealDamageToTransform(collision.transform);
                break;
            case ProjectileDamageType.Area:
                DealDamageInArea();
                break;
        }

        ImpactProjectile();
    }

    private void DealDamageToTransform(Transform transform)
    {
        bool isCrit = GeneralGameplayUtilities.EvaluateCritAttack(GetProjectileModifiedCritChance());

        int damage = GetProjectileModifiedRegularDamage();
        int bleedDamage = GetProjectileModifiedBleedDamage();

        if (isCrit)
        {
            damage = GeneralGameplayUtilities.CalculateCritDamage(damage, GetProjectileModifiedCritDamageMultiplier());
            bleedDamage = GeneralGameplayUtilities.CalculateCritDamage(bleedDamage, GetProjectileModifiedCritDamageMultiplier());
        }

        if (HasRegularDamage())
        {
            GeneralGameplayUtilities.DealRegularDamageToTransform(damage, isCrit, transform, projectileSource);
        }

        if (HasBleed())
        {
            GeneralGameplayUtilities.DealBleedDamageToTransform(bleedDamage, bleedDuration, bleedTickTime, isCrit, transform, projectileSource);
        }
    }

    private void DealDamageInArea()
    {
        bool isCrit = GeneralGameplayUtilities.EvaluateCritAttack(GetProjectileModifiedCritChance());

        int damage = GetProjectileModifiedRegularDamage();
        int bleedDamage = GetProjectileModifiedBleedDamage();

        if (isCrit)
        {
            damage = GeneralGameplayUtilities.CalculateCritDamage(damage, GetProjectileModifiedCritDamageMultiplier());
            bleedDamage = GeneralGameplayUtilities.CalculateCritDamage(bleedDamage, GetProjectileModifiedCritDamageMultiplier());
        }

        if (HasRegularDamage())
        {
            GeneralGameplayUtilities.DealRegularDamageInArea(damage,GeneralUtilities.TransformPositionVector2(transform),projectileArea ,isCrit, targetLayermask, projectileSource);
        }

        if (HasBleed())
        {
            GeneralGameplayUtilities.DealBleedDamageInArea(bleedDamage,bleedDuration,bleedTickTime, GeneralUtilities.TransformPositionVector2(transform), projectileArea, isCrit, targetLayermask, projectileSource);
        }
    }

    public void SetProjectile(float projectileSpeed, float range, int regularDamage, int bleedDamage, float bleedDuration, float bleedTickTime, float critChance, float critDamageMultiplier, ProjectileDamageType projectileDamageType, float projectileArea, IDamageDealer projectileSource, Vector2 direction)
    {
        SetSpeed(projectileSpeed);
        SetLifespan(projectileSpeed, range);
        SetAllDamage(regularDamage, bleedDamage, bleedDuration, bleedTickTime, critChance, critDamageMultiplier);
        SetProjectileDamageType(projectileDamageType,projectileArea);
        SetProjectileSource(projectileSource);
        SetProjectileDirection(direction);
    }

    protected void SetSpeed(float speed) => this.speed = speed;
    protected void SetLifespan(float speed, float range) => lifespan = range/speed;
    protected void SetAllDamage(int regularDamage, int bleedDamage, float bleedDuration, float bleedTickTime, float critChance, float critDamageMultiplier)
    {
        this.regularDamage = regularDamage;
        this.bleedDamage = bleedDamage;
        this.bleedDuration = bleedDuration;
        this.bleedTickTime = bleedTickTime;
        this.critChance = critChance;
        this.critDamageMultiplier = critDamageMultiplier;
    }

    protected void SetProjectileDamageType(ProjectileDamageType projectileDamageType, float projectileArea)
    {
        this.projectileDamageType = projectileDamageType;
        this.projectileArea = projectileArea;
    }

    protected void SetProjectileSource(IDamageDealer projectileSource) => this.projectileSource = projectileSource;
    protected void SetProjectileDirection(Vector2 direction) => this.direction = direction;
    protected bool HasBleed() => bleedDamage > 0f;
    protected bool HasRegularDamage() => regularDamage > 0f;

    protected int GetProjectileModifiedRegularDamage() => GeneralGameplayUtilities.GetModifiedDamage(regularDamage, AttackDamageMultiplierStatManager.Instance.AttackDamageMultiplierStat);
    protected int GetProjectileModifiedBleedDamage() => GeneralGameplayUtilities.GetModifiedDamage(bleedDamage, AttackDamageMultiplierStatManager.Instance.AttackDamageMultiplierStat);
    protected float GetProjectileModifiedCritChance() => GeneralGameplayUtilities.GetModifiedCritChance(critChance, AttackCritChanceStatManager.Instance.AttackCritChanceStat);
    protected float GetProjectileModifiedCritDamageMultiplier() => GeneralGameplayUtilities.GetModifiedCritDamageMultiplier(critDamageMultiplier, AttackCritDamageMultiplierStatManager.Instance.AttackCritDamageMultiplierStat);

}
