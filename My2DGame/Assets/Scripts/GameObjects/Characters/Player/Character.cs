using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public int maxHPpermanent;

    public int currentHP;

    public bool isAlive;

    public Animator animator;

    protected AudioManager audioManager;

    public Vector2Int Position { get; set; }
    private void Start()
    {
        isAlive = true;
        currentHP = maxHPpermanent;
    }
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    public virtual void TakeDamage(int damage)
    {
        if (currentHP > 0)
        {
            currentHP -= damage;
            Debug.Log("Daaaavamm dmg " + damage);
        }
        if (animator != null)
        {
            animator.SetTrigger("hit");
        }
        if(currentHP <= 0){
            isAlive = false;

            foreach (var collider in GetComponents<Collider2D>())
            {
                collider.enabled = false;
            }
        }
    }

    public void AddHP(int healthPoints)
    {
        currentHP += healthPoints;
        if (currentHP > maxHPpermanent)
        {
            currentHP = maxHPpermanent;
        }
    }

}
