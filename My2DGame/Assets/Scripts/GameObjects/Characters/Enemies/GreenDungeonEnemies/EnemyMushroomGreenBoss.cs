using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMushroomGreenBoss : EnemyMushroomGreen
{
    private float shootCooldown = 1.5f;
    private float lastShootTime = 0f;
    void Update()
    {
        basicBehaviour();
    }

    protected override void shootBehaviour()
    {
        if (Time.time >= lastShootTime + shootCooldown)
        {
            ShootMultipleShotsInCircle(amountOfProjectiles, true);
            lastShootTime = Time.time;
        }
    }
}
