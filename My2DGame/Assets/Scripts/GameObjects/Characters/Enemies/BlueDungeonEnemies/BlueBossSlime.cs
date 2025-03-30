using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueBossSlime : MeleeEnemy
{
    [SerializeField]
    BlueSlime blueSlime;

    [SerializeField]
    float spawnSlimesCooldown;

    [SerializeField]
    int minSlimeSpawnAmount;

    [SerializeField]
    int maxSlimeSpawnAmount;

    System.Random random = new System.Random();

    float spawnSlimesCooldownTimer;

    private void Start()
    {
        spawnSlimesCooldownTimer = 1;
        isAlive = true;
    }
    void Update()
    {
        if (isDashing)
        {
            return;
        }

        if (isAlive)
        {
            if (IsInApproachDistance() && Player.Instance.canBeAttacked)
            {
                animator.SetTrigger("move");
                MoveToPlayer();
                spawnSlimesCooldownTimer += Time.deltaTime;

                if (spawnSlimesCooldownTimer >= spawnSlimesCooldown)
                {
                    SpawnSlimes();
                    spawnSlimesCooldownTimer = 0;
                }

                dashCooldownTimer -= Time.deltaTime;

                if (dashCooldownTimer <= 0f && !isDashing)
                {
                    StartCoroutine(DashAttack());
                    dashCooldownTimer = dashCooldown;
                }
            }
            else
            {
                animator.SetTrigger("idle");
            }
        }
        else
        {
            StopOnCurrentPosition();
            animator.SetTrigger("die");
        }

        if (animator != null)
        {
            animator.SetFloat("movementSpeed", movementSpeed);
            animator.SetBool("isAlive", isAlive);
        }
    }

    private void SpawnSlimes()
    {
        for (int i = 0; i < random.Next(minSlimeSpawnAmount, maxSlimeSpawnAmount);  i++)
        {
            BlueSlime blueSlime = Instantiate(this.blueSlime, transform.position, this.blueSlime.transform.rotation);
            blueSlime.attackCooldown = UnityEngine.Random.Range(blueSlime.attackMinCD, blueSlime.attackMaxCD);
        }
    }
}
