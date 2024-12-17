using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinkColorCore : ColorCore
{
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
