using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerHealth : EntityHealth
{
    public static PlayerHealth Instance { get; private set; }

    [Header("Player Settings")]
    [SerializeField] private PlayerIdentifier playerIdentifier;
    [SerializeField] private PlayerDash playerDash;
    [SerializeField] private int healthRegen;

    public static event EventHandler OnPlayerDodge;
    public static event EventHandler<OnEntityDeathEventArgs> OnPlayerDeath;
    public static event EventHandler<OnEntityTakeDamageEventArgs> OnPlayerTakeBleedDamage;
    public static event EventHandler<OnEntityTakeDamageEventArgs> OnPlayerTakeRegularDamage;

    public static event EventHandler<OnEntityDodgeChanceEventArgs> OnPlayerDodgeChanceSet;
    public static event EventHandler<OnEntityArmorPercentageEventArgs> OnPlayerArmorPercentageSet;
    public static event EventHandler<OnEntityLifestealEventArgs> OnPlayerLifestealSet;
    public static event EventHandler<OnEntityHealthEventArgs> OnPlayerMaxHealthSet;
    public static event EventHandler<OnEntityHealthEventArgs> OnPlayerCurrentHealthSet;
    public static event EventHandler<OnEntityHealthEventArgs> OnPlayerAllHealthRestored;
    public static event EventHandler<OnEntityHealEventArgs> OnPlayerHeal;

    public int HealthRegen => healthRegen;

    private const int FIRST_WAVE_NUMBER = 1;

    private void OnEnable()
    {
        MaxHealthStatManager.OnMaxHealthStatInitialized += MaxHealthStatManager_OnMaxHealthStatInitialized;
        MaxHealthStatManager.OnMaxHealthStatUpdated += MaxHealthStatManager_OnMaxHealthStatUpdated;

        HealthRegenStatManager.OnHealthRegenStatInitialized += HealthRegenStatManager_OnHealthRegenStatInitialized;
        HealthRegenStatManager.OnHealthRegenStatUpdated += HealthRegenStatManager_OnHealthRegenStatUpdated;

        DodgeChanceStatManager.OnDodgeChanceStatInitialized += DodgeChanceStatManager_OnDodgeChanceStatInitialized;
        DodgeChanceStatManager.OnDodgeChanceStatUpdated += DodgeChanceStatManager_OnDodgeChanceStatUpdated;

        ArmorPercentageStatManager.OnArmorPercentageStatInitialized += ArmorPercentageStatManager_OnArmorPercentageStatInitialized;
        ArmorPercentageStatManager.OnArmorPercentageStatUpdated += ArmorPercentageStatManager_OnArmorPercentageStatUpdated;

        LifestealStatManager.OnLifestealStatInitialized += LifestealStatManager_OnLifestealStatInitialized;
        LifestealStatManager.OnLifestealStatUpdated += LifestealStatManager_OnLifestealStatUpdated;

        EnemyHealth.OnEnemyTakeRegularDamage += EnemyHealth_OnEnemyTakeRegularDamage;
        EnemyHealth.OnEnemyTakeBleedDamage += EnemyHealth_OnEnemyTakeBleedDamage;

        PlayerDash.OnPlayerDash += PlayerDash_OnPlayerDash;
        PlayerDash.OnPlayerDashStopped += PlayerDash_OnPlayerDashStopped;

        GeneralWavesManager.OnWaveStarting += GeneralWavesManager_OnWaveStarting;
    }

    private void OnDisable()
    {
        MaxHealthStatManager.OnMaxHealthStatInitialized -= MaxHealthStatManager_OnMaxHealthStatInitialized;
        MaxHealthStatManager.OnMaxHealthStatUpdated -= MaxHealthStatManager_OnMaxHealthStatUpdated;

        HealthRegenStatManager.OnHealthRegenStatInitialized -= HealthRegenStatManager_OnHealthRegenStatInitialized;
        HealthRegenStatManager.OnHealthRegenStatUpdated -= HealthRegenStatManager_OnHealthRegenStatUpdated;

        DodgeChanceStatManager.OnDodgeChanceStatInitialized -= DodgeChanceStatManager_OnDodgeChanceStatInitialized;
        DodgeChanceStatManager.OnDodgeChanceStatUpdated -= DodgeChanceStatManager_OnDodgeChanceStatUpdated;

        ArmorPercentageStatManager.OnArmorPercentageStatInitialized -= ArmorPercentageStatManager_OnArmorPercentageStatInitialized;
        ArmorPercentageStatManager.OnArmorPercentageStatUpdated -= ArmorPercentageStatManager_OnArmorPercentageStatUpdated;

        LifestealStatManager.OnLifestealStatInitialized -= LifestealStatManager_OnLifestealStatInitialized;
        LifestealStatManager.OnLifestealStatUpdated -= LifestealStatManager_OnLifestealStatUpdated;

        EnemyHealth.OnEnemyTakeRegularDamage -= EnemyHealth_OnEnemyTakeRegularDamage;
        EnemyHealth.OnEnemyTakeBleedDamage -= EnemyHealth_OnEnemyTakeBleedDamage;

        PlayerDash.OnPlayerDash -= PlayerDash_OnPlayerDash;
        PlayerDash.OnPlayerDashStopped -= PlayerDash_OnPlayerDashStopped;

        GeneralWavesManager.OnWaveStarting -= GeneralWavesManager_OnWaveStarting;
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

    #region DodgeChance
    protected override void OnDodgeChanceSet(float dodgeChance)
    {
        OnPlayerDodgeChanceSet?.Invoke(this, new OnEntityDodgeChanceEventArgs { dodgeChance = dodgeChance });
    }
    #endregion

    #region ArmorPercentage
    protected override void OnArmorPercentageSet(float armorPercentage)
    {
        OnPlayerArmorPercentageSet?.Invoke(this, new OnEntityArmorPercentageEventArgs { armorPercentage = armorPercentage });
    }
    #endregion

    #region LifeSteal
    protected override void OnLifeStealSet(float lifeSteal)
    {
        OnPlayerLifestealSet?.Invoke(this, new OnEntityLifestealEventArgs { lifeSteal = lifeSteal });
    }
    #endregion

    #region Damage
    protected override void OnDeath(IDamageDealer damageSource)
    {
        OnPlayerDeath?.Invoke(this, new OnEntityDeathEventArgs { damageSource = damageSource});
    }

    protected override void OnDodge()
    {
        OnPlayerDodge?.Invoke(this, EventArgs.Empty);
    }

    protected override void OnTakeRegularDamage(int damage, int currentHealth, bool isCrit, IDamageDealer damageSource)
    {
        OnPlayerTakeRegularDamage?.Invoke(this, new OnEntityTakeDamageEventArgs { damageTaken = damage, newCurrentHealth = currentHealth, isCrit = isCrit, damageSource = damageSource, entityHealth = this });
    }

    protected override void OnTakeBleedDamage(int bleedDamage, int currentHealth, bool isCrit, IDamageDealer damageSource)
    {
        OnPlayerTakeBleedDamage?.Invoke(this, new OnEntityTakeDamageEventArgs { damageTaken = bleedDamage, newCurrentHealth = currentHealth, isCrit = isCrit, damageSource = damageSource, entityHealth = this});
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
        OnPlayerHeal?.Invoke(this, new OnEntityHealEventArgs {healAmount = healAmount, newCurrentHealth = currentHealth, entityHealth = this});
    }
    #endregion

    #region HealthRegen
    private void SetHealthRegen(int value) => healthRegen = value;
    #endregion

    protected override bool CanTakeDamage()
    {
        return true;
    }

    public override void InstaKill(IDamageDealer damageSource)
    {
        TakeFinalRegularDamage(INSTA_KILL_DAMAGE, true, damageSource);
    }

    protected void HealFromHealthRegen()
    {
        Heal(healthRegen);
    }

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
    private void DodgeChanceStatManager_OnDodgeChanceStatInitialized(object sender, DodgeChanceStatManager.OnDodgeChanceStatEventArgs e)
    {
        SetDodgeChance(e.dodgeChanceStat);
    }

    private void DodgeChanceStatManager_OnDodgeChanceStatUpdated(object sender, DodgeChanceStatManager.OnDodgeChanceStatEventArgs e)
    {
        SetDodgeChance(e.dodgeChanceStat);
    }
    private void ArmorPercentageStatManager_OnArmorPercentageStatInitialized(object sender, ArmorPercentageStatManager.OnArmorPercentageStatEventArgs e)
    {
        SetArmorPercentage(e.armorPercentageStat);
    }

    private void ArmorPercentageStatManager_OnArmorPercentageStatUpdated(object sender, ArmorPercentageStatManager.OnArmorPercentageStatEventArgs e)
    {
        SetArmorPercentage(e.armorPercentageStat);
    }
    private void LifestealStatManager_OnLifestealStatInitialized(object sender, LifestealStatManager.OnLifestealStatEventArgs e)
    {
        SetLifeSteal(e.lifestealStat);
    }

    private void LifestealStatManager_OnLifestealStatUpdated(object sender, LifestealStatManager.OnLifestealStatEventArgs e)
    {
        SetLifeSteal(e.lifestealStat);
    }
    private void EnemyHealth_OnEnemyTakeRegularDamage(object sender, EnemyHealth.OnEnemyTakeDamageEventArgs e)
    {
        HealFromLifeSteal(e.damageTaken);
    }

    private void EnemyHealth_OnEnemyTakeBleedDamage(object sender, EnemyHealth.OnEnemyTakeDamageEventArgs e)
    {
        HealFromLifeSteal(e.damageTaken);
    }

    private void PlayerDash_OnPlayerDash(object sender, PlayerDash.OnPlayerDashEventArgs e)
    {
        if (!e.ghostedWhileDashing) return;
        SetIsGhosted(true);
    }

    private void PlayerDash_OnPlayerDashStopped(object sender, PlayerDash.OnPlayerDashEventArgs e)
    {
        if (!e.ghostedWhileDashing) return;
        SetIsGhosted(false);
    }

    private void GeneralWavesManager_OnWaveStarting(object sender, GeneralWavesManager.OnWaveEventArgs e)
    {
        if (e.waveSO.waveNumber == FIRST_WAVE_NUMBER) return;
        HealFromHealthRegen();
    }

    #endregion
}
