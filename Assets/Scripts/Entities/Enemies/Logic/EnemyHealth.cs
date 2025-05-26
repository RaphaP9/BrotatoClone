using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : EntityHealth
{
    [Header("Enemy Components")]
    [SerializeField] private EnemyIdentifier enemyIdentifier;
    [SerializeField] private EnemySpawningHandler enemySpawningHandler;
    [SerializeField] private CircleCollider2D circleCollider2D;

    public static event EventHandler OnEnemyDodge;
    public static event EventHandler OnEnemyDeath;
    public static event EventHandler<OnEnemyTakeDamageEventArgs> OnEnemyTakeBleedDamage;
    public static event EventHandler<OnEnemyTakeDamageEventArgs> OnEnemyTakeRegularDamage;

    public static event EventHandler<OnEntityDodgeChanceEventArgs> OnEnemyDodgeChanceSet;
    public static event EventHandler<OnEntityArmorPercentageEventArgs> OnEnemyArmorPercentageSet;
    public static event EventHandler<OnEntityLifestealEventArgs> OnEnemyLifeStealSet;
    public static event EventHandler<OnEntityHealthEventArgs> OnEnemyMaxHealthSet;
    public static event EventHandler<OnEntityHealthEventArgs> OnEnemyCurrentHealthSet;
    public static event EventHandler<OnEntityHealthEventArgs> OnEnemyAllHealthRestored;
    public static event EventHandler<OnEntityHealEventArgs> OnEnemyHeal;

    public event EventHandler OnThisEnemyDodge;
    public event EventHandler OnThisEnemyDeath;
    public event EventHandler<OnEnemyTakeDamageEventArgs> OnThisEnemyTakeBleedDamage;
    public event EventHandler<OnEnemyTakeDamageEventArgs> OnThisEnemyTakeRegularDamage;

    public event EventHandler<OnEntityDodgeChanceEventArgs> OnThisEnemyDodgeChanceSet;
    public event EventHandler<OnEntityArmorPercentageEventArgs> OnThisEnemyArmorPercentageSet;
    public event EventHandler<OnEntityLifestealEventArgs> OnThisEnemyLifeStealSet;
    public event EventHandler<OnEntityHealthEventArgs> OnThisEnemyMaxHealthSet;
    public event EventHandler<OnEntityHealthEventArgs> OnThisEnemyCurrentHealthSet;
    public event EventHandler<OnEntityHealthEventArgs> OnThisEnemyAllHealthRestored;
    public event EventHandler<OnEntityHealEventArgs> OnThisEnemyHeal;

    public class OnEnemyTakeDamageEventArgs : OnEntityTakeDamageEventArgs
    {
        public int id;
    }

    private void OnEnable()
    {
        PlayerHealth.OnPlayerTakeRegularDamage += PlayerHealth_OnPlayerTakeRegularDamage;
        PlayerHealth.OnPlayerTakeBleedDamage += PlayerHealth_OnPlayerTakeBleedDamage;

        GeneralWavesManager.OnWaveCompleted += GeneralWavesManager_OnWaveCompleted;
    }

    private void OnDisable()
    {
        PlayerHealth.OnPlayerTakeRegularDamage -= PlayerHealth_OnPlayerTakeRegularDamage;
        PlayerHealth.OnPlayerTakeBleedDamage -= PlayerHealth_OnPlayerTakeBleedDamage;

        GeneralWavesManager.OnWaveCompleted -= GeneralWavesManager_OnWaveCompleted;
    }

    protected void Start()
    {
        InitializeStats();
    }

    private void InitializeStats()
    {
        SetMaxHealth(enemyIdentifier.EnemySO.maxHealth);
        SetCurrentHealth(enemyIdentifier.EnemySO.maxHealth);
        SetDodgeChance(enemyIdentifier.EnemySO.dodgeChance);
        SetArmorPercentage(enemyIdentifier.EnemySO.armorPercentage);
        SetLifeSteal(enemyIdentifier.EnemySO.lifeSteal);
    }

    #region DodgeChance
    protected override void OnDodgeChanceSet(float dodgeChance)
    {
        OnEnemyDodgeChanceSet?.Invoke(this, new OnEntityDodgeChanceEventArgs { dodgeChance = dodgeChance });
        OnThisEnemyDodgeChanceSet?.Invoke(this, new OnEntityDodgeChanceEventArgs { dodgeChance = dodgeChance });
    }
    #endregion

    #region ArmorPercentage
    protected override void OnArmorPercentageSet(float armorPercentage)
    {
        OnEnemyArmorPercentageSet?.Invoke(this, new OnEntityArmorPercentageEventArgs { armorPercentage = armorPercentage });
        OnThisEnemyArmorPercentageSet?.Invoke(this, new OnEntityArmorPercentageEventArgs { armorPercentage = armorPercentage });
    }
    #endregion

    #region LifeSteal
    protected override void OnLifeStealSet(float lifeSteal)
    {
        OnEnemyLifeStealSet?.Invoke(this, new OnEntityLifestealEventArgs { lifeSteal = lifeSteal });
        OnThisEnemyLifeStealSet?.Invoke(this, new OnEntityLifestealEventArgs { lifeSteal = lifeSteal });
    }
    #endregion

    #region Damage
    protected override void OnDeath(IDamageDealer damageDealer)
    {
        DisableCollider();

        OnEnemyDeath?.Invoke(this, EventArgs.Empty);
        OnThisEnemyDeath?.Invoke(this, EventArgs.Empty);
    }

    protected override void OnDodge()
    {
        OnEnemyDodge?.Invoke(this, EventArgs.Empty);
        OnThisEnemyDodge?.Invoke(this, EventArgs.Empty);
    }

    protected override void OnTakeRegularDamage(int damage, int currentHealth, bool isCrit, IDamageDealer damageSource)
    {
        OnEnemyTakeRegularDamage?.Invoke(this, new OnEnemyTakeDamageEventArgs { damageTaken = damage, newCurrentHealth = currentHealth, isCrit = isCrit, damageSource = damageSource , entityHealth = this, id = enemyIdentifier.EnemySO.id});
        OnThisEnemyTakeRegularDamage?.Invoke(this, new OnEnemyTakeDamageEventArgs { damageTaken = damage, newCurrentHealth = currentHealth, isCrit = isCrit, damageSource = damageSource ,entityHealth = this, id = enemyIdentifier.EnemySO.id});
    }

    protected override void OnTakeBleedDamage(int bleedDamage, int currentHealth, bool isCrit, IDamageDealer damageSource)
    {
        OnEnemyTakeBleedDamage?.Invoke(this, new OnEnemyTakeDamageEventArgs { damageTaken = bleedDamage, newCurrentHealth = currentHealth, isCrit = isCrit, damageSource = damageSource, entityHealth = this, id = enemyIdentifier.EnemySO.id });
        OnThisEnemyTakeBleedDamage?.Invoke(this, new OnEnemyTakeDamageEventArgs { damageTaken = bleedDamage, newCurrentHealth = currentHealth, isCrit = isCrit, damageSource = damageSource, entityHealth = this, id = enemyIdentifier.EnemySO.id });
    }
    #endregion

    #region Health
    protected override void OnMaxHealhSet(int maxHealth)
    {
        OnEnemyMaxHealthSet?.Invoke(this, new OnEntityHealthEventArgs { health = maxHealth });
        OnThisEnemyMaxHealthSet?.Invoke(this, new OnEntityHealthEventArgs { health = maxHealth });
    }

    protected override void OnCurrentHealthSet(int currentHealth)
    {
        OnEnemyCurrentHealthSet?.Invoke(this, new OnEntityHealthEventArgs { health = currentHealth });
        OnThisEnemyCurrentHealthSet?.Invoke(this, new OnEntityHealthEventArgs { health = currentHealth });
    }

    protected override void OnAllHealthRestored(int currentHealth)
    {
        OnEnemyAllHealthRestored?.Invoke(this, new OnEntityHealthEventArgs { health = currentHealth });
        OnThisEnemyAllHealthRestored?.Invoke(this, new OnEntityHealthEventArgs { health = currentHealth });
    }

    protected override void OnHeal(int healAmount, int currentHealth)
    {
        OnEnemyHeal?.Invoke(this, new OnEntityHealEventArgs { healAmount = healAmount, newCurrentHealth = currentHealth, entityHealth = this });
        OnThisEnemyHeal?.Invoke(this, new OnEntityHealEventArgs { healAmount = healAmount, newCurrentHealth = currentHealth, entityHealth = this });
    }
    #endregion

    protected override bool CanTakeDamage()
    {
        if (enemySpawningHandler.IsSpawning) return false;

        return true;
    }

    public override void InstaKill(IDamageDealer damageSource)
    {
        //TakeFinalRegularDamage(INSTA_KILL_DAMAGE, true, enemyIdentifier.EnemySO); //For damage with feedbacks
        TakeInstaDamage(damageSource, INSTA_KILL_DAMAGE); //For damage with no feedbacks
    }

    private void DisableCollider()
    {
        circleCollider2D.enabled = false;
    }

    #region Subscriptions
    private void PlayerHealth_OnPlayerTakeRegularDamage(object sender, OnEntityTakeDamageEventArgs e)
    {
        //HealFromLifeSteal(e.damageTaken);
    }

    private void PlayerHealth_OnPlayerTakeBleedDamage(object sender, OnEntityTakeDamageEventArgs e)
    {
        //HealFromLifeSteal(e.damageTaken);
    }

    private void GeneralWavesManager_OnWaveCompleted(object sender, GeneralWavesManager.OnWaveEventArgs e)
    {
        InstaKill(enemyIdentifier.EnemySO);
    }

    #endregion
}
