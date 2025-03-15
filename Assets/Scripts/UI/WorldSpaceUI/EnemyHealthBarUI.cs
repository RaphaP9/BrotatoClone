using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthBarUI : EntityHealthBarUI
{
    [Header("Components")]
    [SerializeField] private EnemyHealth enemyHealth;

    private void OnEnable()
    {
        enemyHealth.OnThisEnemyMaxHealthSet += EnemyHealth_OnThisEnemyMaxHealthSet;
        enemyHealth.OnThisEnemyCurrentHealthSet += EnemyHealth_OnThisEnemyCurrentHealthSet;
    }

    private void OnDisable()
    {
        enemyHealth.OnThisEnemyMaxHealthSet -= EnemyHealth_OnThisEnemyMaxHealthSet;
        enemyHealth.OnThisEnemyCurrentHealthSet -= EnemyHealth_OnThisEnemyCurrentHealthSet;
    }

    private void EnemyHealth_OnThisEnemyMaxHealthSet(object sender, EntityHealth.OnEntityHealthEventArgs e)
    {
        SetMaxHealth(e.health);
        UpdateHealthBar(currentHealth, maxHealth);
    }

    private void EnemyHealth_OnThisEnemyCurrentHealthSet(object sender, EntityHealth.OnEntityHealthEventArgs e)
    {
        SetCurrentHealth(e.health);
        UpdateHealthBar(currentHealth, maxHealth);
    }
}
