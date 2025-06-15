using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class EnemySpawnVFXHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private EnemySpawningHandler enemySpawningHandler;
    [SerializeField] private EnemyHealth enemyHealth;
    [SerializeField] private VisualEffect visualEffect;

    private const float LIFESPAN = 2f;

    private void OnEnable()
    {
        enemySpawningHandler.OnThisEnemySpawnStart += EnemySpawningHandler_OnThisEnemySpawnStart;
        enemySpawningHandler.OnThisEnemySpawnAlmostComplete += EnemySpawningHandler_OnThisEnemySpawnComplete;

        enemyHealth.OnThisEnemyDeath += EnemyHealth_OnThisEnemyDeath;
    }

    private void OnDisable()
    {
        enemySpawningHandler.OnThisEnemySpawnStart -= EnemySpawningHandler_OnThisEnemySpawnStart;
        enemySpawningHandler.OnThisEnemySpawnAlmostComplete -= EnemySpawningHandler_OnThisEnemySpawnComplete;

        enemyHealth.OnThisEnemyDeath -= EnemyHealth_OnThisEnemyDeath;
    }

    private void StartVFX()
    {
        visualEffect.Play();
        visualEffect.gameObject.SetActive(true);
    }

    private void EndVFX()
    {
        transform.SetParent(null);
        visualEffect.Stop();

        StartCoroutine(DestroyCoroutine());
    }

    private IEnumerator DestroyCoroutine()
    {
        yield return new WaitForSeconds(LIFESPAN);
        Destroy(gameObject);
    }

    private void EnemySpawningHandler_OnThisEnemySpawnStart(object sender, EnemySpawningHandler.OnEnemySpawnEventArgs e)
    {
        StartVFX();
    }

    private void EnemySpawningHandler_OnThisEnemySpawnComplete(object sender, EnemySpawningHandler.OnEnemySpawnEventArgs e)
    {
        EndVFX();
    }

    private void EnemyHealth_OnThisEnemyDeath(object sender, EnemyHealth.OnEnemyDeathEventArgs e)
    {
        EndVFX();
    }
}
