using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHandler : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private LayerMask targetLayermask;

    [Header("Runtime Filled Settings")]
    [SerializeField, Range(5f, 15f)] private float speed;
    [SerializeField, Range(5f, 10f)] private float lifespan;
    [Space]
    [SerializeField] private float regularDamage;
    [Space]
    [SerializeField] private float bleedDamage;
    [SerializeField] private float bleedDuration;
    [SerializeField] private float bleedTickTime;
    [Space]
    [SerializeField] private ProjectileDamageType damageType;
    [SerializeField] private float projectileArea;
    [Space]
    [SerializeField] private Vector2 direction;

    private IDamageDealer projectileSource;

    private Rigidbody2D _rigidbody2D;

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

    public void SetProjectile(float projectileSpeed, float range, float regularDamage, float bleedDamage, float bleedDuration, float bleedTickTime, ProjectileDamageType projectileDamageType, float projectileArea, IDamageDealer projectileSource, Vector2 direction)
    {
        SetSpeed(projectileSpeed);
        SetLifespan(projectileSpeed, range);
        SetAllDamage(regularDamage, bleedDamage, bleedDuration, bleedTickTime);
        SetProjectileDamageType(projectileDamageType,projectileArea);
        SetProjectileSource(projectileSource);
        SetProjectileDirection(direction);
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
        Destroy(gameObject);
    }

    protected void SetSpeed(float speed) => this.speed = speed;
    protected void SetLifespan(float speed, float range) => lifespan = range/speed;
    protected void SetAllDamage(float regularDamage, float bleedDamage, float bleedDuration, float bleedTickTime)
    {
        this.regularDamage = regularDamage;
        this.bleedDamage = bleedDamage;
        this.bleedDuration = bleedDuration;
        this.bleedTickTime = bleedTickTime;
    }
    protected void SetProjectileDamageType(ProjectileDamageType damageType, float projectileArea)
    {
        this.damageType = damageType;
        this.projectileArea = projectileArea;
    }
    protected void SetProjectileSource(IDamageDealer projectileSource) => this.projectileSource = projectileSource;
    protected void SetProjectileDirection(Vector2 direction) => this.direction = direction;
    protected bool HasBleed() => bleedDamage > 0f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!GeneralUtilities.CheckGameObjectInLayerMask(collision.gameObject, targetLayermask)) return;

        DealDamageToTransform(collision.transform);
    }

    private void DealDamageToTransform(Transform transform)
    {
        EntityHealth entityHealth = transform.GetComponent<EntityHealth>();

        if(entityHealth == null)
        {
            Debug.Log("Transform to damage does not contain an EntityHealth component, damage will be ignored");
        }
    }
}
