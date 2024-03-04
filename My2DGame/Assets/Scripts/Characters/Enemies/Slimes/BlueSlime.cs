using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueSlime : MeleeEnemy
{
    public Animator animator;


    void Update()
    {
        BasicEnemyMovement();
        dashAttack();
        animator.SetFloat("movementSpeed", movementSpeed);
    }

}
