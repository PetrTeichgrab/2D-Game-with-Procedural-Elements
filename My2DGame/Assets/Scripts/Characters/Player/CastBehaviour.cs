using Unity.VisualScripting;
using UnityEngine;

public class CastBehaviour : MonoBehaviour
{
    public GameObject hitEffect;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy character = collision.gameObject.GetComponent<Enemy>();
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

    private void OnDestroy()
    {
        if (hitEffect != null)
        {
            var effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
            if (effect != null)
            {
                Destroy(effect, 0.5f);
            }
        }
    }
}
