using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingProjectileGreenBoss : EnemyShoot
{
    public DirectionProjectileGreen projectilePrefab;
    public float spawnInterval = 1f;
    public int numberOfProjectiles = 8;
    private float spawnTimer;
    private AudioManager audioManager;

    private void Update()
    {
        ShootAndFollowPlayer();
        SpawnSurroundingProjectiles();
    }

    private void SpawnSurroundingProjectiles()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval)
        {
            spawnTimer = 0f;
            ShootAround();
        }
    }

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void ShootAround()
    {
        if (projectilePrefab == null) return;

        float angleStep = 360f / numberOfProjectiles;

        for (int i = 0; i < numberOfProjectiles; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

            DirectionProjectileGreen newProjectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            audioManager.PlaySFX(audioManager.EnemyAttackRange);
            newProjectile.SetDirection(direction);
        }
    }
}