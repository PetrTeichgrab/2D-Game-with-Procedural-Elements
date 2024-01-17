using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Vector2 position = new Vector2(50,50);
    Vector2 moveDirection = new Vector2();
    public float movementSpeed = 2;
    public Rigidbody2D rb;
    void Start()
    {
        transform.position = position;
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInputs();
    }

    void FixedUpdate()
    {
        Move();
    }

    void ProcessInputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector2(moveX, moveY).normalized;
    }

    void Move()
    {
        rb.velocity = new Vector2(moveDirection.x * movementSpeed, moveDirection.y * movementSpeed);
    }
}
