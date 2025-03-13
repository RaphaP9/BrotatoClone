using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestScript : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;

    private void OnEnable()
    {
        PlayerHealth.OnPlayerTakeRegularDamage += PlayerHealth_OnPlayerTakeRegularDamage;
    }

    private void OnDisable()
    {
        PlayerHealth.OnPlayerTakeRegularDamage -= PlayerHealth_OnPlayerTakeRegularDamage;
    }

    private void PlayerHealth_OnPlayerTakeRegularDamage(object sender, EntityHealth.OnEntityTakeDamageEventArgs e)
    {
        Debug.Log($"Bleeding: {e.damageTaken}");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            playerHealth.TakeRegularDamage(1);
        }
    }
}
