using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public abstract class EntityHealth : MonoBehaviour
{
    [Header("Entity Health Settings")]
    [SerializeField] private BleedType bleedType;

    private enum BleedType { Unrestricted, OnlyOneBleed, HighestBleedReplacement}

    [Header("RuntimeFilled")]
    [SerializeField] protected int maxHealth;
    [SerializeField] protected int currentHealth;
    [SerializeField, Range(0f,1f)] protected float armorPercentage;
    [SerializeField, Range(0f, 1f)] protected float dodgeChance;
    [SerializeField, Range(0f, 1f)] protected float lifeSteal;
    [SerializeField] protected bool isGhosted; 
    [SerializeField] protected bool isBleeding;

    public int MaxHealth => maxHealth;
    public int CurrentHealth => currentHealth;
    public float ArmorPercentage => armorPercentage;
    public float DodgeChance => dodgeChance;
    public float LifeSteal => lifeSteal;    
    public bool IsGhosted => isGhosted;
    public bool IsBleeding => isBleeding;

    private int currentBleedDamage = 0;

    protected const int INSTA_KILL_DAMAGE = 999;

    private bool hasRestoredCurrentHealthFirstUpdate = false;

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

    public class OnEntityDeathEventArgs : EventArgs
    {
        public IDamageDealer damageSource;
    }

    protected virtual void Update()
    {
        HandleRestoreCurrentHealthFirstUpdate();
    }

    //Processed in first Update, to avoid RestoringHealth(Initialization of health) before updating it due to objects altering health stats
    //(initial object addition happens on Start())
    private void HandleRestoreCurrentHealthFirstUpdate() 
    {
        if (!hasRestoredCurrentHealthFirstUpdate)
        {
            RestoreAllCurrentHealth();
            hasRestoredCurrentHealthFirstUpdate = true;
        }
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
        if (!IsAlive()) return;
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

        if (!IsAlive()) OnDeath(damageSource);
    }
    #endregion


    #region Bleed
    public void Bleed(int baseDamage, float bleedDuration, float tickTime, bool isCrit, IDamageDealer damageSource)
    {
        if (!CanTakeDamage()) return;
        if (isGhosted) return;

        if (bleedType == BleedType.OnlyOneBleed && IsBleeding) return;

        if (bleedType == BleedType.HighestBleedReplacement && IsBleeding)
        {
            int realBleedDamage = MitigateByArmor(baseDamage);

            //If post-armor mitigation bleed damage is lower or same as current bleed damage, do not apply bleeding.
            //Otherwise, cancel current bleeding (Stop Bleeding Coroutine) and apply new bleeding
            //Ex. if entity is bleeding for 2 and will take damage that causes it to bleed for 2, do not apply that bleeding
            //Ex. if entity is bleeding for 2 and will take damage that causes it to bleed for 3, cancel the bleeding for 2 and apply bleeding for 3

            if (realBleedDamage <= currentBleedDamage) return;
            else StopAllCoroutines();
        }

        StartCoroutine(BleedCoroutine(baseDamage, bleedDuration, tickTime, isCrit, damageSource));
    }

    protected IEnumerator BleedCoroutine(int baseDamage, float bleedDuration, float tickTime, bool isCrit, IDamageDealer damageSource)
    {
        SetIsBleeding(true);

        int tickNumber = Mathf.FloorToInt(bleedDuration / tickTime);

        for (int i = 0; i < tickNumber; i++)
        {
            float tickTimer = 0f;

            while(tickTimer < tickTime)
            {
                if (!IsAlive())
                {
                    SetIsBleeding(false);
                    SetCurrentBleedDamage(0);
                    yield break;
                }

                tickTimer += Time.deltaTime;
                yield return null;
            }

            TakeBleedDamage(baseDamage, isCrit, damageSource);
        }

        float finalTickTimer = 0f; //Final While Loop To SetIsBleeding(false) after a tickTime

        while (finalTickTimer < tickTime)
        {
            if (!IsAlive()) break;

            finalTickTimer += Time.deltaTime;
            yield return null;
        }

        SetCurrentBleedDamage(0);
        SetIsBleeding(false);
    }

    protected void TakeBleedDamage(int baseDamage, bool isCrit, IDamageDealer damageSource)
    {
        if (!IsAlive()) return;

        int mitigatedBleedDamage = MitigateByArmor(baseDamage);
        TakeFinalBleedDamage(mitigatedBleedDamage, isCrit, damageSource);
    }

    protected void TakeFinalBleedDamage(int damage, bool isCrit, IDamageDealer damageSource)
    {
        SetCurrentBleedDamage(damage);

        currentHealth = currentHealth < damage ? 0 : currentHealth - damage;

        OnCurrentHealthSet(currentHealth);
        OnTakeBleedDamage(damage, currentHealth, isCrit, damageSource);

        if (!IsAlive()) OnDeath(damageSource);
    }
    #endregion

    #region InstaDamage 
    protected void TakeInstaDamage(IDamageDealer damageDealer, int damage) //Damage with no events triggered (No Feedback)
    {
        currentHealth = currentHealth < damage ? 0 : currentHealth - damage;

        OnCurrentHealthSet(currentHealth);

        if (!IsAlive()) OnDeath(damageDealer);
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
        OnCurrentHealthSet(currentHealth);
        OnAllHealthRestored(currentHealth);
    }

    protected void Heal(int healAmount)
    {
        if (IsFullHealth()) return;

        int previousHealth = currentHealth;
        int newHealth = currentHealth + healAmount > maxHealth? maxHealth : currentHealth + healAmount;

        SetCurrentHealth(newHealth);

        int realHealAmount = currentHealth - previousHealth;

        OnHeal(realHealAmount, currentHealth);
    }

    protected void HealFromLifeSteal(int damage)
    {
        int healAmount = Mathf.RoundToInt(damage * lifeSteal);

        if (healAmount <= 0) return;

        Heal(healAmount);
    }

    protected void SetIsBleeding(bool bleeding) => isBleeding = bleeding;
    protected void SetCurrentBleedDamage(int damage) => currentBleedDamage = damage;
    public void SetIsGhosted(bool value) => isGhosted = value; //When Ghosted, entity does not take damage and proyectiles do not collide;
    protected bool IsFullHealth() => currentHealth == maxHealth;

    public abstract void InstaKill(IDamageDealer damageSource);

    #region Abstracts For Events
    protected abstract void OnDodge();
    protected abstract void OnTakeRegularDamage(int damage, int currentHealth, bool isCrit, IDamageDealer damageSource);
    protected abstract void OnTakeBleedDamage(int bleedDamage, int currentHealth, bool isCrit, IDamageDealer damageSource);
    protected abstract void OnDeath(IDamageDealer damageDealer);
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
