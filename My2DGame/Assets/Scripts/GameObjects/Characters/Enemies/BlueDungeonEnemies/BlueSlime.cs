using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueSlime : MeleeEnemy
{
    public int rewardMoney = 1;
    void Update()
    {
        if (isDashing || rewardMoney == 0)
        {
            return;
        }

        dashCooldownTimer -= Time.deltaTime;

        if (isAlive)
        {
            if (IsInApproachDistance() && Player.Instance.canBeAttacked)
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
            if (Player.Instance != null)
            {
                Player.Instance.AddMoney(rewardMoney);
                rewardMoney = 0;
            }
        }

        if (animator != null)
        {
            animator.SetFloat("movementSpeed", movementSpeed);
            animator.SetBool("isAlive", isAlive);
        }
    }

}
