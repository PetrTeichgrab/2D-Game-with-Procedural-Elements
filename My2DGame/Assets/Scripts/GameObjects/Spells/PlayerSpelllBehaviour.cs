using Unity.VisualScripting;
using UnityEngine;

public class PlayerSpellBehavior : SpellBehavior
{
    CastSpell spell;
    public void Initialize(CastSpell spell)
    {
        this.spell = spell;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Character character = collision.gameObject.GetComponent<Character>();
            if (character != null)
            {
                int damage = spell.damagePermanent + spell.damage;
                character.TakeDamage(damage);
                Debug.Log("Davam damage " + damage);
            }
        }

        else
        {
            UndergroundTilemap undergroundTilemap = collision.gameObject.GetComponent<UndergroundTilemap>();

            if (undergroundTilemap != null)
            {
                Vector3 hitPosition = collision.contacts[0].point;
                Vector3 bulletDirection = GetComponent<Rigidbody2D>().velocity.normalized;
                undergroundTilemap.BreakTile(hitPosition, bulletDirection);
            }
        }

        if (gameObject != null)
        {
            Destroy(gameObject);
        }
    }
}
