using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestScript : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private WeaponSO testWeapon;

    private void OnEnable()
    {
        PlayerHealth.OnPlayerTakeBleedDamage += PlayerHealth_OnPlayerTakeBleedDamage;
        PlayerHealth.OnPlayerTakeRegularDamage += PlayerHealth_OnPlayerTakeRegularDamage;

        EnemyHealth.OnEnemyDodge += EnemyHealth_OnEnemyDodge;
        EnemyKamikaze.OnEnemySelfDestroyCompleted += EnemyKamikaze_OnEnemySelfDestroy;
    }



    private void OnDisable()
    {
        PlayerHealth.OnPlayerTakeBleedDamage -= PlayerHealth_OnPlayerTakeBleedDamage;
        PlayerHealth.OnPlayerTakeRegularDamage -= PlayerHealth_OnPlayerTakeRegularDamage;

        EnemyHealth.OnEnemyDodge -= EnemyHealth_OnEnemyDodge;
        EnemyKamikaze.OnEnemySelfDestroyCompleted -= EnemyKamikaze_OnEnemySelfDestroy;

    }


    private void Update()
    {
        
    }


    private void PlayerHealth_OnPlayerTakeBleedDamage(object sender, EntityHealth.OnEntityTakeDamageEventArgs e)
    {
        Debug.Log($"Bleeding: {e.damageTaken}, IsCrit {e.isCrit}, From: {e.damageSource.GetName()}");
    }
    private void PlayerHealth_OnPlayerTakeRegularDamage(object sender, EntityHealth.OnEntityTakeDamageEventArgs e)
    {
        Debug.Log($"Damage: {e.damageTaken}, IsCrit {e.isCrit}, From: {e.damageSource.GetName()}");

    }
    private void EnemyHealth_OnEnemyDodge(object sender, System.EventArgs e)
    {
        Debug.Log("EnemyDodged!");
    }
    private void EnemyKamikaze_OnEnemySelfDestroy(object sender, System.EventArgs e)
    {
        Debug.Log("Kaboom");
    }
}
