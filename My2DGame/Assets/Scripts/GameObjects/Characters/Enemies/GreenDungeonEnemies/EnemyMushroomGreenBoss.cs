using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMushroomGreenBoss : EnemyMushroomGreen
{
    void Update()
    {
        basicBehaviour();
        if (!isAlive && rewardMoney != 0)
        {
            if (Player.Instance != null)
            {
                Player.Instance.AddMoney(rewardMoney);
                rewardMoney = 0;
            }
        }
    }

    protected override void shootBehaviour()
    {
        ShootMultipleShotsInCircle(amountOfProjectiles, true);
    }
}
