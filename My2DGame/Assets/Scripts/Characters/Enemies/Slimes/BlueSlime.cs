using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueSlime : Enemy
{
    public Animator animator;

    void Update()
    {
        BasicEnemyMovement();
        animator.SetFloat("movementSpeed", movementSpeed);
    }

}
