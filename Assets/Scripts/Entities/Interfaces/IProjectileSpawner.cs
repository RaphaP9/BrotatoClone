using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProjectileSpawner
{
    public ProjectileDamageType GetProjectileDamageType();
    public float GetProjectileArea();
}
