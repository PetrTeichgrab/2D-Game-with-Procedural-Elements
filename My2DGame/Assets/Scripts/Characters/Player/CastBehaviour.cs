using Unity.VisualScripting;
using UnityEngine;

public class CastBehaviour : MonoBehaviour
{
    public GameObject hitEffect;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            RangeEnemy character = collision.gameObject.GetComponent<RangeEnemy>();
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
