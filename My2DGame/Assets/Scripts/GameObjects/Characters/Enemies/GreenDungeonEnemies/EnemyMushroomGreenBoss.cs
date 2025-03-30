using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMushroomGreenBoss : EnemyMushroomGreen
{
    void Update()
    {
        basicBehaviour();
    }

    protected override void shootBehaviour()
    {
        ShootMultipleShotsInCircle(amountOfProjectiles, true);
    }
}
