using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileImpactVFXHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private ProjectileHandler projectileHandler;
    [Space]
    [SerializeField] private Transform projectileVFXTransform;

    private const float LIFESPAN = 3f;

    private void OnEnable()
    {
        projectileHandler.OnProjectileDestroyByImpact += ProjectileHandler_OnProjectileDestroyByImpact;
    }

    private void OnDisable()
    {
        projectileHandler.OnProjectileDestroyByImpact -= ProjectileHandler_OnProjectileDestroyByImpact;
    }

    private void PlayVFX()
    {
        transform.parent = null;
        projectileVFXTransform.gameObject.SetActive(true);
        StartCoroutine(DestroyCoroutine());
    }

    private IEnumerator DestroyCoroutine()
    {
        yield return new WaitForSeconds(LIFESPAN);
        Destroy(gameObject);
    }

    private void ProjectileHandler_OnProjectileDestroyByImpact(object sender, System.EventArgs e)
    {
        PlayVFX();
    }
}
