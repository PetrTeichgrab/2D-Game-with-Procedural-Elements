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
    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        if (currentHP <= 0)
        {
            isAlive = false;
            if(animator != null)
            {
                bool hasHitTrigger = animator.parameters.Any(p => p.type == AnimatorControllerParameterType.Trigger && p.name == "hit");
                if (hasHitTrigger)
                {
                    animator.SetTrigger("hit");
                }
            }
            if (GetComponent<EdgeCollider2D>() != null)
            {
                GetComponent<EdgeCollider2D>().enabled = false;
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
