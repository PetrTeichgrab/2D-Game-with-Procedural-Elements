using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenSlime : MeleeEnemy
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
                animator.SetBool("move", true);
                MoveToPlayer(movementSpeed);
                dashAttack();
            }
            else
            {
                animator.SetBool("move", false);
                animator.SetTrigger("idle");
            }
        }
        else
        {
            Die();
            animator.SetBool("move", false);
        }

        if (animator != null)
        {
            animator.SetBool("isAlive", isAlive);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player character = collision.gameObject.GetComponent<Player>();
            if (character != null && isAlive)
            {
                animator.SetTrigger("attack");
                character.TakeDamage(10);
            }
        }
    }
}
