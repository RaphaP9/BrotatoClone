using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionIgnoreHandler : MonoBehaviour
{
    private const int PLAYER_LAYER = 6;
    private const int ENEMY_LAYER = 7;

    private void Start()
    {
        IgnorePlayerEnemiesCollision();
    }

    private void IgnorePlayerEnemiesCollision()
    {
        Physics2D.IgnoreLayerCollision(PLAYER_LAYER, ENEMY_LAYER);
    }
}
