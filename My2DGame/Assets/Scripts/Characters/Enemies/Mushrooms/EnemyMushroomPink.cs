using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMushroomPink : RangeEnemy
{
    public Animator animator;
    public bool Appeard { get; set; } = false;

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
            Shoot();
        }
        animator.SetBool("appeard", Appeard);
    }

    public void CheckAppearence()
    {
        if (Vector2.Distance(player.transform.position, transform.position) < 4f)
        {
            Appeard = true;
        }
    }

}
