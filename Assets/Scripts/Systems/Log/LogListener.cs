using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogListener : MonoBehaviour
{
    private void OnEnable()
    {
        PlayerHealth.OnPlayerTakeRegularDamage += PlayerHealth_OnPlayerTakeRegularDamage;
        PlayerHealth.OnPlayerTakeBleedDamage += PlayerHealth_OnPlayerTakeBleedDamage;

        EnemyKamikaze.OnEnemyExplosion += EnemyKamikaze_OnEnemyExplosion;

        GameManager.OnGameLost += GameManager_OnGameLost;
    }

    private void OnDisable()
    {
        PlayerHealth.OnPlayerTakeRegularDamage -= PlayerHealth_OnPlayerTakeRegularDamage;
        PlayerHealth.OnPlayerTakeBleedDamage -= PlayerHealth_OnPlayerTakeBleedDamage;

        EnemyKamikaze.OnEnemyExplosion -= EnemyKamikaze_OnEnemyExplosion;

        GameManager.OnGameLost += GameManager_OnGameLost;
    }


    #region Subscriptions
    private void PlayerHealth_OnPlayerTakeRegularDamage(object sender, EntityHealth.OnEntityTakeDamageEventArgs e) => GameLogManager.Instance.Log("Player/TakeDamage/Regular");
    private void PlayerHealth_OnPlayerTakeBleedDamage(object sender, EntityHealth.OnEntityTakeDamageEventArgs e) => GameLogManager.Instance.Log("Player/TakeDamage/Bleed");

    private void EnemyKamikaze_OnEnemyExplosion(object sender, EnemyKamikaze.OnEnemyExplosionEventArgs e) => GameLogManager.Instance.Log($"Enemy/SelfDestroy/{e.enemySO.id}");

    private void GameManager_OnGameLost(object sender, System.EventArgs e) => GameLogManager.Instance.Log($"Game/Lose");
    #endregion
}
