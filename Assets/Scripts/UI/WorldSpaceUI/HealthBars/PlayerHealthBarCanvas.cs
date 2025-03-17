using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthBarCanvas : EntityHealthBarUI
{
    private void OnEnable()
    {
        PlayerHealth.OnPlayerMaxHealthSet += PlayerHealth_OnPlayerMaxHealthSet;
        PlayerHealth.OnPlayerCurrentHealthSet += PlayerHealth_OnPlayerCurrentHealthSet;
    }

    private void OnDisable()
    {
        PlayerHealth.OnPlayerMaxHealthSet -= PlayerHealth_OnPlayerMaxHealthSet;
        PlayerHealth.OnPlayerCurrentHealthSet -= PlayerHealth_OnPlayerCurrentHealthSet;
    }

    private void PlayerHealth_OnPlayerMaxHealthSet(object sender, EntityHealth.OnEntityHealthEventArgs e)
    {
        SetMaxHealth(e.health);
        UpdateHealthBar(currentHealth, maxHealth);
    }

    private void PlayerHealth_OnPlayerCurrentHealthSet(object sender, EntityHealth.OnEntityHealthEventArgs e)
    {
        SetCurrentHealth(e.health);
        UpdateHealthBar(currentHealth, maxHealth);
    }
}
