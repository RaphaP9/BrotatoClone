using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityHealth : MonoBehaviour
{
    [Header("Entity Settings")]
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;
    [SerializeField, Range(0f,1f)] private float armorPercentage;
    [SerializeField, Range(0f, 1f)] private float dodgeChance;

    public int MaxHealth => maxHealth;
    public int CurrentHealth => currentHealth;
    public float ArmorPercentage => armorPercentage;
    public float DodgeChance => dodgeChance;

    public class OnEntityTakeDamageEventArgs : EventArgs
    {
        public int damageTaken;
        public int newCurrentHealth;
    }

    public class OnEntityHealthEventArgs : EventArgs
    {
        public int health;
    }

    public class OnEntityHealEventArgs : EventArgs
    {
        public int healAmount;
        public int newCurrentHealth;
    }

    protected bool TryDodge()
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
    public void TakeRegularDamage(int baseDamage)
    {
        int mitigatedBleedDamage = MitigateByArmor(baseDamage);
        TakeFinalRegularDamage(mitigatedBleedDamage);
    }

    protected void TakeFinalRegularDamage(int damage)
    {
        currentHealth = currentHealth < damage ? 0 : currentHealth - damage;

        OnTakeRegularDamage(damage, currentHealth);

        if (!IsAlive()) OnDeath();
    }
    #endregion


    #region Bleed

    public void Bleed(int baseDamage, float bleedDuration, float tickTime)
    {
        StartCoroutine(BleedCoroutine(baseDamage, bleedDuration, tickTime));
    }

    protected IEnumerator BleedCoroutine(int baseDamage, float bleedDuration, float tickTime)
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

            TakeBleedDamage(baseDamage);
        }
    }
    protected void TakeBleedDamage(int baseDamage)
    {
        int mitigatedBleedDamage = MitigateByArmor(baseDamage);
        TakeFinalBleedDamage(mitigatedBleedDamage);
    }

    protected void TakeFinalBleedDamage(int damage)
    {
        currentHealth = currentHealth < damage ? 0 : currentHealth - damage;

        OnTakeBleedDamage(damage, currentHealth);

        if (!IsAlive()) OnDeath();
    }
    #endregion


    protected bool IsAlive() => currentHealth > 0;

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

    #region Abstracts For Events
    protected abstract void OnDodge();
    protected abstract void OnTakeRegularDamage(int damage, int currentHealth);
    protected abstract void OnTakeBleedDamage(int bleedDamage, int currentHealth);
    protected abstract void OnDeath();
    //
    protected abstract void OnMaxHealhSet(int maxHealth);
    protected abstract void OnCurrentHealthSet(int currentHealth);
    protected abstract void OnAllHealthRestored(int currentHealth);
    protected abstract void OnHeal(int healAmount, int currentHealth);
    #endregion
}
