using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMushroomGreen : RangeEnemy
{
    protected bool Appeard = false;

    void Update()
    {
        basicBehaviour();
    }

    public void CheckAppearence()
    {
        if (Vector2.Distance(player.transform.position, transform.position) < appearenceDistance)
        {
            Appeard = true;
        }
    }

    protected void basicBehaviour()
    {
        if (isAlive)
        {
            CheckAppearence();
            if (!Appeard)
            {
                GetComponent<PolygonCollider2D>().enabled = false;
            }
            else
            {
                GetComponent<PolygonCollider2D>().enabled = true;
                shootBehaviour();
            }
            animator.SetBool("appeard", Appeard);
        }
        if (!isAlive && rewardMoney != 0)
        {
            if (Player.Instance != null)
            {
                Player.Instance.AddMoney(rewardMoney);
                rewardMoney = 0;
            }
        }
        animator.SetBool("alive", isAlive);
    }

    protected virtual void shootBehaviour()
    {
        ShootMultipleShotsInCircle(amountOfProjectiles, true);
    }

}
