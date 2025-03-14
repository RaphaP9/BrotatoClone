using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdentifier : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private EnemySO enemySO;

    public EnemySO EnemySO => enemySO;
}
