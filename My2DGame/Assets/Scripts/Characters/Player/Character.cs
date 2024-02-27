using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public int maxHP;

    public int currentHP;

    public bool isAlive;
    private void Start()
    {
        isAlive = true;
        currentHP = maxHP;
    }
    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        if (currentHP <= 0)
        {
            isAlive = false;
            Destroy(gameObject);
        }
    }
}
