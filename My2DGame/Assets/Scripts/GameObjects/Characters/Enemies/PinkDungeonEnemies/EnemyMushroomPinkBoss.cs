using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMushroomPinkBoss : EnemyMushroomPink
{
    void Update()
    {
        basicBehaviour();
    }

    private new void shootBehaviour()
    {
        ShootMultipleShotsInCircle(amountOfProjectiles, true);
    }

}
