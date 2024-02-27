using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UIElements;

public abstract class Enemy : Character
{
    public float movementSpeed;
    public float visionDistance;
    public float approachDistance;
    public float retreatDistance;
    public float shootDistance;
    public float meleeDistance;
    public bool waitBeforeFirstShot;
    public Transform player;

    public GameObject projectile;

    private float timeBetweenShots;
    public float startTimeBetweenShots;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected void BasicEnemyMovement()
    {
        if (IsInApproachDistance())
        {
            MoveToPlayer();
        }
        else if (IsInRetreatDistance())
        {
            RetreatFromPlayer();
        }
        else if (IsInStopDistance())
        {
            StopOnCurrentPosition();
        }
    }

    protected void Shoot()
    {
        if (timeBetweenShots <= 0)
        {
            if (projectile != null)
            {
                if (Vector2.Distance(player.transform.position, transform.position) < shootDistance)
                {
                    var projectileObject = Instantiate(projectile, transform.position, Quaternion.identity);
                    Destroy(projectileObject, 4f);
                }
            }
            timeBetweenShots = startTimeBetweenShots;
        }
        else
        {
            timeBetweenShots -= Time.deltaTime;
        }
    }

    protected void MoveToPlayer()
    {
       transform.position = Vector2.MoveTowards(transform.position, player.position, movementSpeed * Time.deltaTime);
    }

    protected void StopOnCurrentPosition()
    {
        transform.position = this.transform.position;
    }

    protected void RetreatFromPlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.position, -movementSpeed * Time.deltaTime);
    }

    protected bool IsInApproachDistance()
    {
        return (Vector2.Distance(transform.position, player.position) > approachDistance) && 
            (Vector2.Distance(transform.position, player.position) < visionDistance);
    }

    protected bool IsInStopDistance()
    {
        return (Vector2.Distance(transform.position, player.position) > approachDistance) &&
            (Vector2.Distance(transform.position, player.position) > retreatDistance);
    }

    protected bool IsInRetreatDistance()
    {
         return (Vector2.Distance(transform.position, player.position) <= retreatDistance);
    }




}
