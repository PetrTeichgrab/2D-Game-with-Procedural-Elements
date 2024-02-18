using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMushroomPink : MonoBehaviour
{
    public Animator animator;
    public bool Appeard { get; set; } = false;


    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!Appeard)
        {
            GetComponent<EdgeCollider2D>().enabled = false;
        }
        else
        {
            GetComponent<EdgeCollider2D>().enabled = true;
        }
        animator.SetBool("appeard", Appeard);
    }
}
