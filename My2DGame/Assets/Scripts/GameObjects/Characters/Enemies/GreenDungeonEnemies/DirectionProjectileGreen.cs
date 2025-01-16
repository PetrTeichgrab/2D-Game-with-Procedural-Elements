using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionProjectileGreen : EnemyShoot
{
    private Vector2 direction;

    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection.normalized;
    }

    void Update()
    {
        if (direction != Vector2.zero)
        {
            ShootToDirection(direction);
        }
    }
}
