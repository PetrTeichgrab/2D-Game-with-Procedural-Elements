using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMushroomGreen : RangeEnemy
{
    protected bool Appeard { get; set; } = false;

    [SerializeField]
     protected float appearenceDistance { get; set; } = 4f;

    [SerializeField]
    protected int amountOfProjectiles { get; set; } = 6; 

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
        animator.SetBool("alive", isAlive);
    }

    protected virtual void shootBehaviour()
    {
        ShootMultipleShotsInCircle(amountOfProjectiles, true);
    }

}
