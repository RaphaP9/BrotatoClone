using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileRotationHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private ProjectileHandler projectileHandler;
    [SerializeField] private Transform transformToRotate;

    private void OnEnable()
    {
        projectileHandler.OnProjectileDirectionSet += ProjectileHandler_OnProjectileDirectionSet;
    }

    private void OnDisable()
    {
        projectileHandler.OnProjectileDirectionSet -= ProjectileHandler_OnProjectileDirectionSet;
    }

    private void RotateTowardsDirection(Vector2 direction)
    {
        GeneralUtilities.RotateTransformTowardsVector2(transformToRotate, direction);
    }

    private void ProjectileHandler_OnProjectileDirectionSet(object sender, ProjectileHandler.OnProjectileDirectionEventArgs e)
    {
        RotateTowardsDirection(e.direction);
    }
}
