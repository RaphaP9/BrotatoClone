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
        IgnoreEnemiesEnemiesCollision();
    }

    private void IgnorePlayerEnemiesCollision()
    {
        Physics2D.IgnoreLayerCollision(PLAYER_LAYER, ENEMY_LAYER);
    }

    private void IgnoreEnemiesEnemiesCollision()
    {
        Physics2D.IgnoreLayerCollision(ENEMY_LAYER, ENEMY_LAYER);
    }
}
