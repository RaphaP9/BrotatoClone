using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public abstract class EntityHealth : MonoBehaviour
{
    [Header("Entity Settings - RuntimeFilled")]
    [SerializeField] protected int maxHealth;
    [SerializeField] protected int currentHealth;
    [SerializeField, Range(0f,1f)] protected float armorPercentage;
    [SerializeField, Range(0f, 1f)] protected float dodgeChance;
    [SerializeField, Range(0f, 1f)] protected float lifeSteal;
    [SerializeField] protected bool isGhosted; 

    public int MaxHealth => maxHealth;
    public int CurrentHealth => currentHealth;
    public float ArmorPercentage => armorPercentage;
    public float DodgeChance => dodgeChance;
    public float LifeSteal => lifeSteal;    
    public bool IsGhosted => isGhosted;

    protected const int INSTA_KILL_DAMAGE = 999;

    public class OnEntityTakeDamageEventArgs : EventArgs
    {
        public int damageTaken;
        public int newCurrentHealth;
        public bool isCrit;
        public IDamageDealer damageSource;
        public EntityHealth entityHealth;
    }

    public class OnEntityDodgeChanceEventArgs : EventArgs
    {
        public float dodgeChance;
    }

    public class OnEntityArmorPercentageEventArgs : EventArgs
    {
        public float armorPercentage;
    }
    public class OnEntityLifestealEventArgs : EventArgs
    {
        public float lifeSteal;
    }

    public class OnEntityHealthEventArgs : EventArgs
    {
        public int health;
    }

    public class OnEntityHealEventArgs : EventArgs
    {
        public int healAmount;
        public int newCurrentHealth;
        public EntityHealth entityHealth;
    }

    protected abstract bool CanTakeDamage();

    public bool TryDodge()
    {
        bool dodged = GeneralGameplayUtilities.EvaluateDodgeChance(dodgeChance);
        if (dodged) OnDodge();
        return dodged;
    }
    
    protected int MitigateByArmor(int baseDamage)
    {
        int mitigatedDamage = GeneralGameplayUtilities.MitigateDamageByPercentage(baseDamage, armorPercentage);
        return mitigatedDamage;
    }

    #region RegularDamage
    public void TakeRegularDamage(int baseDamage, bool isCrit, IDamageDealer damageSource)
    {
        if (!CanTakeDamage()) return;
        if (isGhosted) return;

        int mitigatedBleedDamage = MitigateByArmor(baseDamage);
        TakeFinalRegularDamage(mitigatedBleedDamage, isCrit, damageSource);
    }

    protected void TakeFinalRegularDamage(int damage, bool isCrit, IDamageDealer damageSource)
    {
        currentHealth = currentHealth < damage ? 0 : currentHealth - damage;

        OnCurrentHealthSet(currentHealth);
        OnTakeRegularDamage(damage, currentHealth, isCrit, damageSource);

        if (!IsAlive()) OnDeath();
    }
    #endregion


    #region Bleed

    public void Bleed(int baseDamage, float bleedDuration, float tickTime, bool isCrit, IDamageDealer damageSource)
    {
        if (!CanTakeDamage()) return;

        StartCoroutine(BleedCoroutine(baseDamage, bleedDuration, tickTime, isCrit, damageSource));
    }

    protected IEnumerator BleedCoroutine(int baseDamage, float bleedDuration, float tickTime, bool isCrit, IDamageDealer damageSource)
    {
        int tickNumber = Mathf.FloorToInt(bleedDuration / tickTime);

        for (int i = 0; i < tickNumber; i++)
        {
            float tickTimer = 0f;

            while(tickTimer < tickTime)
            {
                if (!IsAlive()) yield break;

                tickTimer += Time.deltaTime;
                yield return null;
            }

            TakeBleedDamage(baseDamage, isCrit, damageSource);
        }
    }
    protected void TakeBleedDamage(int baseDamage, bool isCrit, IDamageDealer damageSource)
    {
        int mitigatedBleedDamage = MitigateByArmor(baseDamage);
        TakeFinalBleedDamage(mitigatedBleedDamage, isCrit, damageSource);
    }

    protected void TakeFinalBleedDamage(int damage, bool isCrit, IDamageDealer damageSource)
    {
        if (!IsAlive()) return;
        if (isGhosted) return;

        currentHealth = currentHealth < damage ? 0 : currentHealth - damage;

        OnCurrentHealthSet(currentHealth);
        OnTakeBleedDamage(damage, currentHealth, isCrit, damageSource);

        if (!IsAlive()) OnDeath();
    }
    #endregion


    public bool IsAlive() => currentHealth > 0;

    protected void SetDodgeChance(float value)
    {
        dodgeChance = value;
        OnDodgeChanceSet(dodgeChance);
    }

    protected void SetArmorPercentage(float value)
    {
        armorPercentage = value;
        OnArmorPercentageSet(armorPercentage);
    }

    protected void SetLifeSteal(float value)
    {
        lifeSteal = value;
        OnLifeStealSet(lifeSteal);
    }

    protected void SetMaxHealth(int health)
    {
        maxHealth = health;

        OnMaxHealhSet(maxHealth);

        if(currentHealth > maxHealth)
        {
            SetCurrentHealth(maxHealth);
        }
    }

    protected void SetCurrentHealth(int health)
    {
        currentHealth = health;
        OnCurrentHealthSet(currentHealth);
    }

    protected void RestoreAllCurrentHealth()
    {
        currentHealth = maxHealth;
        OnAllHealthRestored(currentHealth);
    }

    protected void Heal(int healAmount)
    {
        int previousHealth = currentHealth;
        currentHealth = currentHealth + healAmount > maxHealth? maxHealth : currentHealth + healAmount;

        int realHealAmount = currentHealth - previousHealth;

        OnHeal(realHealAmount, currentHealth);
    }

    protected void HealFromLifeSteal(int damage)
    {
        int healAmount = Mathf.RoundToInt(damage * lifeSteal);

        if (healAmount <= 0) return;

        Heal(healAmount);
    }

    protected void SetIsGhosted(bool value) => isGhosted = value;

    public abstract void InstaKill();

    #region Abstracts For Events
    protected abstract void OnDodge();
    protected abstract void OnTakeRegularDamage(int damage, int currentHealth, bool isCrit, IDamageDealer damageSource);
    protected abstract void OnTakeBleedDamage(int bleedDamage, int currentHealth, bool isCrit, IDamageDealer damageSource);
    protected abstract void OnDeath();
    //
    protected abstract void OnDodgeChanceSet(float dodgeChance);
    protected abstract void OnArmorPercentageSet(float armorPercentage);
    protected abstract void OnLifeStealSet(float lifeSteal);
    protected abstract void OnMaxHealhSet(int maxHealth);
    protected abstract void OnCurrentHealthSet(int currentHealth);
    protected abstract void OnAllHealthRestored(int currentHealth);
    protected abstract void OnHeal(int healAmount, int currentHealth);
    #endregion
}
