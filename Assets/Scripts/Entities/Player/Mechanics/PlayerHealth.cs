using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerHealth : EntityHealth
{
    public static PlayerHealth Instance { get; private set; }

    [Header("Player Settings")]
    [SerializeField] private int healthRegen;

    public static event EventHandler OnPlayerDodge;
    public static event EventHandler OnPlayerDeath;
    public static event EventHandler<OnEntityTakeDamageEventArgs> OnPlayerTakeBleedDamage;
    public static event EventHandler<OnEntityTakeDamageEventArgs> OnPlayerTakeRegularDamage;

    public static event EventHandler<OnEntityHealthEventArgs> OnPlayerMaxHealthSet;
    public static event EventHandler<OnEntityHealthEventArgs> OnPlayerCurrentHealthSet;
    public static event EventHandler<OnEntityHealthEventArgs> OnPlayerAllHealthRestored;
    public static event EventHandler<OnEntityHealEventArgs> OnPlayerHeal;

    public int HealthRegen => healthRegen;

    private void OnEnable()
    {
        MaxHealthStatManager.OnMaxHealthStatInitialized += MaxHealthStatManager_OnMaxHealthStatInitialized;
        MaxHealthStatManager.OnMaxHealthStatUpdated += MaxHealthStatManager_OnMaxHealthStatUpdated;

        HealthRegenStatManager.OnHealthRegenStatInitialized += HealthRegenStatManager_OnHealthRegenStatInitialized;
        HealthRegenStatManager.OnHealthRegenStatUpdated += HealthRegenStatManager_OnHealthRegenStatUpdated;
    }

    private void OnDisable()
    {
        MaxHealthStatManager.OnMaxHealthStatInitialized -= MaxHealthStatManager_OnMaxHealthStatInitialized;
        MaxHealthStatManager.OnMaxHealthStatUpdated -= MaxHealthStatManager_OnMaxHealthStatUpdated;

        HealthRegenStatManager.OnHealthRegenStatInitialized -= HealthRegenStatManager_OnHealthRegenStatInitialized;
        HealthRegenStatManager.OnHealthRegenStatUpdated -= HealthRegenStatManager_OnHealthRegenStatUpdated;
    }

    private void Awake()
    {
        SetSingleton();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one PlayerHealth instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    #region Damage
    protected override void OnDeath()
    {
        OnPlayerDeath?.Invoke(this, EventArgs.Empty);
    }

    protected override void OnDodge()
    {
        OnPlayerDodge?.Invoke(this, EventArgs.Empty);
    }

    protected override void OnTakeRegularDamage(int damage, int currentHealth, bool isCrit)
    {
        OnPlayerTakeRegularDamage?.Invoke(this, new OnEntityTakeDamageEventArgs { damageTaken = damage, newCurrentHealth = currentHealth, isCrit = isCrit});
    }

    protected override void OnTakeBleedDamage(int bleedDamage, int currentHealth, bool isCrit)
    {
        OnPlayerTakeBleedDamage?.Invoke(this, new OnEntityTakeDamageEventArgs { damageTaken = bleedDamage, newCurrentHealth = currentHealth, isCrit = isCrit});
    }
    #endregion

    #region Health
    protected override void OnMaxHealhSet(int maxHealth)
    {
        OnPlayerMaxHealthSet?.Invoke(this, new OnEntityHealthEventArgs { health = maxHealth });
    }
    protected override void OnCurrentHealthSet(int currentHealth)
    {
        OnPlayerCurrentHealthSet?.Invoke(this, new OnEntityHealthEventArgs { health = currentHealth });
    }

    protected override void OnAllHealthRestored(int currentHealth)
    {
        OnPlayerAllHealthRestored?.Invoke(this, new OnEntityHealthEventArgs {health = currentHealth});
    }

    protected override void OnHeal(int healAmount, int currentHealth)
    {
        OnPlayerHeal?.Invoke(this, new OnEntityHealEventArgs {healAmount = healAmount, newCurrentHealth = currentHealth});
    }
    #endregion

    #region HealthRegen
    private void SetHealthRegen(int value) => healthRegen = value;
    #endregion

    #region Subscriptions
    private void MaxHealthStatManager_OnMaxHealthStatInitialized(object sender, MaxHealthStatManager.OnMaxHealthStatEventArgs e)
    {
        SetMaxHealth(e.maxHealthStat);
        SetCurrentHealth(e.maxHealthStat);
    }

    private void MaxHealthStatManager_OnMaxHealthStatUpdated(object sender, MaxHealthStatManager.OnMaxHealthStatEventArgs e)
    {
        SetMaxHealth(e.maxHealthStat);
    }
    private void HealthRegenStatManager_OnHealthRegenStatInitialized(object sender, HealthRegenStatManager.OnHealthRegenStatEventArgs e)
    {
        SetHealthRegen(e.healthRegenStat);
    }

    private void HealthRegenStatManager_OnHealthRegenStatUpdated(object sender, HealthRegenStatManager.OnHealthRegenStatEventArgs e)
    {
        SetHealthRegen(e.healthRegenStat);
    }
    #endregion
}
