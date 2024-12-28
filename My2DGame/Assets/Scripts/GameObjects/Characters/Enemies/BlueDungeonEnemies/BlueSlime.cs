using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueSlime : MeleeEnemy
{
    void Update()
    {
        if (isDashing)
        {
            return;
        }

        if (isAlive)
        {
            if (IsInApproachDistance())
            {
                animator.SetTrigger("move");
                MoveToPlayer(movementSpeed);
                dashAttack();
            }
            else
            {
                animator.SetTrigger("idle");
            }
        }
        else
        {
            StopOnCurrentPosition();
        }

        if (animator != null)
        {
            animator.SetFloat("movementSpeed", movementSpeed);
            animator.SetBool("isAlive", isAlive);
        }
    }

}
