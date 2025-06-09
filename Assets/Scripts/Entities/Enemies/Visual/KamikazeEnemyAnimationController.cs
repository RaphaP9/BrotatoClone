using UnityEngine;

public class KamikazeEnemyAnimationController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;
    [Space]
    [SerializeField] private EnemyMovement enemyMovement;
    [SerializeField] private EnemySpawningHandler enemySpawningHandler;
    [SerializeField] private EnemyHealth enemyHealth;
    [SerializeField] private EnemyKamikaze enemyKamikaze;

    private const string SPEED_FLOAT = "Speed";

    private const string MOVEMENT_BLEND_TREE_NAME = "MovementBlendTree";

    private const string SPAWN_ANIMATION_NAME = "Spawn";
    private const string DEATH_ANIMATION_NAME = "Death";
    private const string KAMIKAZE_ANIMATION_NAME = "Kamikaze";

    private bool hasDied = false;

    private void OnEnable()
    {
        enemySpawningHandler.OnThisEnemySpawnStart += EnemySpawningHandler_OnThisEnemySpawnStart;
        enemySpawningHandler.OnThisEnemySpawnComplete += EnemySpawningHandler_OnThisEnemySpawnComplete;

        enemyKamikaze.OnThisEnemySelfDestroyBegin += EnemyKamikaze_OnThisEnemySelfDestroyBegin;
        enemyHealth.OnThisEnemyDeath += EnemyHealth_OnThisEnemyDeath;
    }

    private void OnDisable()
    {
        enemySpawningHandler.OnThisEnemySpawnStart -= EnemySpawningHandler_OnThisEnemySpawnStart;
        enemySpawningHandler.OnThisEnemySpawnComplete -= EnemySpawningHandler_OnThisEnemySpawnComplete;

        enemyKamikaze.OnThisEnemySelfDestroyBegin -= EnemyKamikaze_OnThisEnemySelfDestroyBegin;
        enemyHealth.OnThisEnemyDeath -= EnemyHealth_OnThisEnemyDeath;
    }

    private void Update()
    {
        HandleSpeedBlend();
    }

    private void HandleSpeedBlend()
    {
        animator.SetFloat(SPEED_FLOAT, enemyMovement.GetSpeed());
    }

    #region Subscriptions
    private void EnemySpawningHandler_OnThisEnemySpawnStart(object sender, EnemySpawningHandler.OnEnemySpawnEventArgs e)
    {
        animator.Play(SPAWN_ANIMATION_NAME);
    }

    private void EnemySpawningHandler_OnThisEnemySpawnComplete(object sender, EnemySpawningHandler.OnEnemySpawnEventArgs e)
    {
        if (hasDied) return;
        animator.Play(MOVEMENT_BLEND_TREE_NAME);
    }

    private void EnemyKamikaze_OnThisEnemySelfDestroyBegin(object sender, EnemyKamikaze.OnEnemyExplosionEventArgs e)
    {
        if (hasDied) return;
        animator.Play(KAMIKAZE_ANIMATION_NAME);
    }

    private void EnemyHealth_OnThisEnemyDeath(object sender, System.EventArgs e)
    {
        if (enemyKamikaze.HasExplodedKamikaze) return;
        hasDied = true;
        animator.Play(DEATH_ANIMATION_NAME);
    }

    #endregion


}
