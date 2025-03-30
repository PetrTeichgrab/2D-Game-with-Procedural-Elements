using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenSlime : MeleeEnemy
{
    private float damageCooldown = 1.0f;
    private float lastDamageTime = -Mathf.Infinity;

    void Update()
    {
        if (isDashing)
        {
            return;
        }

        if (isAlive)
        {
            if (IsInApproachDistance() && Player.Instance.canBeAttacked)
            {
                animator.SetBool("move", true);
                MoveToPlayer();
                DashAttack();
            }
            else
            {
                animator.SetTrigger("idle");
            }
        }
        else
        {
            StopOnCurrentPosition();
            animator.SetBool("move", false);
        }

        if (animator != null)
        {
            animator.SetBool("isAlive", isAlive);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isAlive) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            Player character = collision.gameObject.GetComponent<Player>();
            if (character != null)
            {
                if (Time.time - lastDamageTime >= damageCooldown)
                {
                    lastDamageTime = Time.time;
                    animator.SetTrigger("attack");
                    character.TakeDamage(damage);
                }
            }
        }
    }
}

