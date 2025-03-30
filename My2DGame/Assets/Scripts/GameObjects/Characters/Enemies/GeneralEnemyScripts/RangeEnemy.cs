using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UIElements;

public abstract class RangeEnemy : Character
{
    [SerializeField]
    protected float movementSpeed;
    [SerializeField]
    protected float visionDistance;
    [SerializeField]
    protected float approachDistance;
    [SerializeField]
    protected float retreatDistance;
    [SerializeField]
    protected float shootDistance;
    [SerializeField]
    protected float projectileMaxLifeTime;
    [SerializeField]
    protected bool waitBeforeFirstShot;
    [SerializeField]
    protected Transform player;
    [SerializeField]
    protected GameObject projectile;
    [SerializeField]
    protected float startTimeBetweenShots;
    [SerializeField]
    private float timeBetweenShots;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected void BasicEnemyMovement()
    {
        if (IsInApproachDistance() && Player.Instance.canBeAttacked)
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

    private void ShootMultipleProjectiles(int projectileCount)
    {
        float angleStep = 360f / projectileCount;
        float angle = 0f;

        for (int i = 0; i < projectileCount; i++)
        {
            float projectileDirX = Mathf.Cos(angle * Mathf.Deg2Rad);
            float projectileDirY = Mathf.Sin(angle * Mathf.Deg2Rad);
            Vector2 projectileDirection = new Vector2(projectileDirX, projectileDirY).normalized;

            var projectileObject = Instantiate(projectile, transform.position, Quaternion.identity);
            audioManager.PlaySFX(audioManager.EnemyAttackRange);
            var mushroomProjectile = projectileObject.GetComponent<MushroomProjectile>();

            if (mushroomProjectile != null)
            {
                mushroomProjectile.SetDirection(projectileDirection);
            }

            Destroy(projectileObject, projectileMaxLifeTime);

            angle += angleStep;
        }
    }


    protected void ShootMultipleShotsInCircle(int projectileCount, bool followPlayer)
    {
        if (timeBetweenShots <= 0)
        {
            if (projectile != null)
            {
                if (Vector2.Distance(player.transform.position, transform.position) < shootDistance)
                {
                    if (followPlayer)
                    {
                        ShootHomingProjectiles(projectileCount);
                    }
                    else
                    {
                        ShootMultipleProjectiles(projectileCount);
                    }
                }
            }
            timeBetweenShots = startTimeBetweenShots;
        }
        else
        {
            timeBetweenShots -= Time.deltaTime;
        }
    }


    private void ShootHomingProjectiles(int projectileCount)
    {
        float angleStep = 360f / projectileCount;
        float angle = 0f;

        for (int i = 0; i < projectileCount; i++)
        {
            float projectileDirX = Mathf.Cos(angle * Mathf.Deg2Rad);
            float projectileDirY = Mathf.Sin(angle * Mathf.Deg2Rad);
            Vector2 spawnDirection = new Vector2(projectileDirX, projectileDirY).normalized;

            var projectileObject = Instantiate(projectile, transform.position, Quaternion.identity);
            audioManager.PlaySFX(audioManager.EnemyAttackRange);

            Destroy(projectileObject, projectileMaxLifeTime);

            angle += angleStep;
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
