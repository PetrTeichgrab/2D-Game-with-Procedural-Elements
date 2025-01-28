using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public virtual void TakeDamage(int damage)
    {
        if (currentHP > 0)
        {
            currentHP -= damage;
        }
        if (animator != null)
        {
            animator.SetTrigger("hit");
        }
        if(currentHP <= 0){
            isAlive = false;

            foreach (var collider in GetComponents<Collider2D>())
            {
                Debug.Log("collider v pryc");
                collider.enabled = false;
            }
        }
    }

    public void AddHP(int healthPoints)
    {
        currentHP += healthPoints;
        if (currentHP > maxHP)
        {
            currentHP = maxHP;
        }
    }

}
