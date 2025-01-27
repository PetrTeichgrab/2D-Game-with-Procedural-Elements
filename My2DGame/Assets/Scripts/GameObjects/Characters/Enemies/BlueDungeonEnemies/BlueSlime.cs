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

        dashCooldownTimer -= Time.deltaTime;

        if (isAlive)
        {
            if (IsInApproachDistance())
            {
                animator.SetTrigger("move");
                MoveToPlayer();
                if (dashCooldownTimer <= 0f && !isDashing)
                {
                    StartCoroutine(DashAttack());
                    dashCooldownTimer = dashCooldown;
                }
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
