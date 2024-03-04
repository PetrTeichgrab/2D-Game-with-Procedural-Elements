using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
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
    public int damage;
    public float dashTime;
    public float dashSpeed;
    public CapsuleCollider2D capsuleCollider;
    private float attackCooldownTimer = Mathf.Infinity;
    public Transform player;
    public TrailRenderer trailRenderer;
    private bool canDash;
    private bool isDashing;
    public Rigidbody2D rb;
    public float attackCooldown;
    private float startMovementSpeed;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        attackCooldownTimer = 0;
        trailRenderer.enabled = false;
        startMovementSpeed = movementSpeed;
    }
    private IEnumerator Dash()
    {
        trailRenderer.enabled = true;
        movementSpeed = dashSpeed;
        yield return new WaitForSeconds(dashTime);
        trailRenderer.enabled = false;
        movementSpeed = startMovementSpeed;
    }

    protected void BasicEnemyMovement()
    {
        if (IsInApproachDistance())
        {
            MoveToPlayer();
        }
    }

    protected void MoveToPlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.position, movementSpeed * Time.deltaTime);
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
        }
    }


}
