using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    Vector2 moveDirection = new Vector2();
    public Animator animator;
    private float moveX, moveY;
    private bool canDash = true;
    private bool isDashing;

    [SerializeField]
    private float dashSpeed = 12f;
    [SerializeField]
    private float dashTime = 0.2f;
    [SerializeField]
    private float dashCD = 1f;

    [SerializeField]
    private TrailRenderer trailRenderer;

    public float movementSpeed = 2;
    public Rigidbody2D rb;
    void Start()
    {
        resetTrailRendered();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDashing)
        {
            return;
        }
        ProcessInputs();
        animator.SetFloat("verticalSpeed", moveY);
        animator.SetFloat("horizontalSpeed", moveX);
        if(Input.GetKeyDown(KeyCode.LeftShift) && canDash) {
            StartCoroutine(Dash());
        }
    }

    void FixedUpdate()
    {
        if(isDashing) { 
            return ; 
        }
        Move();
    }

    void ProcessInputs()
    {
        moveX = Input.GetAxisRaw("Horizontal");
        moveY = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector2(moveX, moveY).normalized;
    }

    void Move()
    {
        rb.velocity = new Vector2(moveDirection.x * movementSpeed, moveDirection.y * movementSpeed);
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        rb.velocity = new Vector2(moveX, moveY).normalized*dashSpeed;
        setTrailrendererForDash();
        yield return new WaitForSeconds(dashTime);
        resetTrailRendered();
        isDashing = false;
        yield return new WaitForSeconds(dashCD);
        canDash = true;
    }

    private void setTrailrendererForDash()
    {
        trailRenderer.startWidth = 0.18f;
        trailRenderer.startColor = new UnityEngine.Color(255f, 255f, 255f, 0.1f);
        trailRenderer.endColor = new UnityEngine.Color(255f, 255f, 255f, 0f);
        trailRenderer.endWidth = 0f;
    }

    private void resetTrailRendered()
    {
        trailRenderer.startWidth = 0.15f;
        trailRenderer.startColor = new UnityEngine.Color(255f, 255f, 255f, 0.1f);
        trailRenderer.endColor = new UnityEngine.Color(0, 0, 0, 0f);
        trailRenderer.endWidth = 0.1f;
    }
}
