using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    public float projectileSpeed;
    public float projectileMaxLifeTime;
    public GameObject hitEffect;
    protected Transform player;
    protected Vector2 target;

    protected void Start()
    {
        target = new Vector2(transform.position.x, transform.position.y);
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    protected void ShootAndFollowPlayer(){
        transform.position = Vector2.MoveTowards(transform.position, player.position, projectileSpeed* Time.deltaTime);
    }

    protected void ShootOnPlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, target, projectileSpeed * Time.deltaTime);
    }

    protected void ShootToDirection(Vector2 direction)
    {
        transform.position += (Vector3)direction * projectileSpeed * Time.deltaTime;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player character = collision.gameObject.GetComponent<Player>();
            if (character != null)
            {
                character.TakeDamage(2);
            }
        }

        Destroy(gameObject);
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
