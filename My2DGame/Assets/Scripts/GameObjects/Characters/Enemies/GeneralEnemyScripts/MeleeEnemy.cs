using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UIElements;

public abstract class MeleeEnemy : Character
{
    public float movementSpeed;
    public float visionDistance;
    public float approachDistance;
    public int attackMaxCD;
    public int attackMinCD;
    public int damage = 5;
    public float dashTime;
    public float dashSpeed;
    public Collider2D characterCollider;
    private float attackCooldownTimer = Mathf.Infinity;
    public Transform player;
    public TrailRenderer trailRenderer;
    public Rigidbody2D rb;
    public float attackCooldown = 2f;
    protected float startMovementSpeed;
    protected bool isDashing;
    [SerializeField]
    private float lastAttackTime = 0f;


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        attackCooldownTimer = 0;
        if (trailRenderer != null)
        {
            trailRenderer.enabled = false;
        }
        isAlive = true;
        startMovementSpeed = movementSpeed;
    }
    private IEnumerator Dash()
    {
        isDashing = true;
        if (trailRenderer != null)
        {
            trailRenderer.enabled = true;
        }
        Vector2 direction = (player.position - transform.position).normalized;

        rb.velocity = direction * dashSpeed;

        yield return new WaitForSeconds(dashTime);

        rb.velocity = direction.normalized * movementSpeed;
        if (trailRenderer != null)
        {
            trailRenderer.enabled = false;
        }
        isDashing = false;
    }


    protected void MoveToPlayer(float approachMovementSpeed)
    {
        transform.position = Vector2.MoveTowards(transform.position, player.position, approachMovementSpeed * Time.deltaTime);
    }

    protected void StopOnCurrentPosition()
    {
        if (TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        movementSpeed = 0;
    }


    protected void Die()
    {
        rb.velocity = new Vector2 (0, 0);
    }

    protected bool IsInApproachDistance()
    {
        return (Vector2.Distance(transform.position, player.position) < approachDistance) &&
            (Vector2.Distance(transform.position, player.position) < visionDistance);
    }

    protected void dashAttack()
    {
        attackCooldownTimer += Time.deltaTime;

        if (attackCooldownTimer >= attackCooldown)
        {
            StartCoroutine(Dash());
            attackCooldownTimer = 0;
            if (animator != null)
            {
                animator.SetTrigger("attack");
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player character = collision.gameObject.GetComponent<Player>();
            if (character != null && isAlive)
            {
                if (Time.time >= lastAttackTime + attackCooldown)
                {
                    animator.SetTrigger("attack");
                    character.TakeDamage(10);

                    lastAttackTime = Time.time;
                }
            }
        }
    }


}
