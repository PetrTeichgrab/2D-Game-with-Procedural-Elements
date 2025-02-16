using Unity.VisualScripting;
using UnityEngine;

public class PlayerSpellBehavior : SpellBehavior
{
    [SerializeField]
    public int Damage = 10;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Character character = collision.gameObject.GetComponent<Character>();
            if (character != null)
            {
                character.TakeDamage(Damage);
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

    public void IncreaseDamage()
    {
        Damage += 100;
    }
}
