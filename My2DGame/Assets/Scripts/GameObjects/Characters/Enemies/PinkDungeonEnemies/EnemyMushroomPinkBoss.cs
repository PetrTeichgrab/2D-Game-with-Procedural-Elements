using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMushroomPinkBoss : EnemyMushroomPink
{
    void Update()
    {
        CheckAppearence();
        if (!Appeard)
        {
            isAlive = false;
            GetComponent<EdgeCollider2D>().enabled = false;
        }
        else
        {
            isAlive = true;
            GetComponent<EdgeCollider2D>().enabled = true;
            ShootMultipleShotsInCircle(amountOfProjectiles ,true);
        }
        animator.SetBool("appeard", Appeard);
    }

}
