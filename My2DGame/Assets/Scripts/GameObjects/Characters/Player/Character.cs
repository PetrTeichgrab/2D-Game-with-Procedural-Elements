using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public int maxHP;

    public int currentHP;

    public bool isAlive;

    public Animator animator;

    public Vector2Int Position { get; set; }
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
            if(animator != null)
            {
                animator.SetTrigger("hit");
            }
        }
    }
}
