using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueSlime : MeleeEnemy
{
    void Update()
    {
        if(isDashing) {
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
            Die();
        }

        if (animator != null)
        {
            animator.SetFloat("movementSpeed", movementSpeed);
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
