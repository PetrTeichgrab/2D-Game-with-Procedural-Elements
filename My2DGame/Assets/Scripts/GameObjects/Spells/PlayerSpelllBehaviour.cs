using Unity.VisualScripting;
using UnityEngine;

public class PlayerSpellBehavior : SpellBehavior
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Character character = collision.gameObject.GetComponent<Character>();
            if (character != null)
            {
                character.TakeDamage(50);
            }
        }

        if (gameObject != null)
        {
            Destroy(gameObject);
        }
    }
}
