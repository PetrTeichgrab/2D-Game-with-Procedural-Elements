using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Player : Character
{
    Vector2 moveDirection = new Vector2();
    private float moveX, moveY;
    private bool canDash = true;
    private bool isDashing;

    [SerializeField]
    public float dashSpeed = 12f;
    [SerializeField]
    public float dashTime = 0.2f;
    [SerializeField]
    public float dashCD = 1f;
    [SerializeField]
    private StatusBar playerHpBar;

    public float minDashCD = 0.8f;

    [SerializeField]
    private TrailRenderer trailRenderer;

    public float movementSpeed = 2;

    public Rigidbody2D rb;

    public GameObject castPoint;

    public List<ColorCore> colorCores;

    private bool canDoubleJump = true;
    private bool isGrounded;
    private bool usesGravity = false;

    [SerializeField]
    private float jumpForce = 5f;

    [SerializeField]
    private Transform groundCheck;

    [SerializeField]
    private LayerMask groundLayer;

    [SerializeField]
    private Light2D playerLight;

    public bool isDead;

    public Vector3 deathPosition;

    public bool isPlayerInUnderground;


    void Start()
    {
        currentHP = maxHP;
        isDead = false;
        colorCores = new List<ColorCore>();
        resetTrailRendered();
        DisableGravityMode();
        SetTransparency(1f);
    }

    void Update()
    {
        if (isDead)
        {
            return;
        }

        if (isDashing)
        {
            return;
        }

        CheckGrounded();

        ProcessInputs();
        animator.SetFloat("verticalSpeed", moveY);
        animator.SetFloat("horizontalSpeed", moveX);

        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing && canDash)
        {
            StartCoroutine(Dash());
        }

        if (Input.GetKeyUp(KeyCode.End))
        {
            Suicide();
        }

        if (usesGravity && Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

    }

    private void CheckGrounded()
    {
        bool wasGrounded = isGrounded;
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.3f, groundLayer);

        if (isGrounded && !wasGrounded)
        {
            canDoubleJump = true;
        }
    }

    private void Suicide()
    {
        TakeDamage(maxHP);
    }


    void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }
        Move();
    }

    void ProcessInputs()
    {
        moveX = Input.GetAxisRaw("Horizontal");
        moveY = usesGravity ? 0 : Input.GetAxisRaw("Vertical");
        moveDirection = new Vector2(moveX, moveY).normalized;
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;

        rb.velocity = new Vector2(moveX, moveY).normalized * dashSpeed;
        setTrailrendererForDash();

        yield return new WaitForSeconds(dashTime);

        resetTrailRendered();
        isDashing = false;

        yield return new WaitForSeconds(dashCD);
        canDash = true;
    }

    void Move()
    {
        if (usesGravity)
        {
            rb.velocity = new Vector2(moveDirection.x * movementSpeed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(moveDirection.x * movementSpeed, moveDirection.y * movementSpeed);
        }
    }

    void Jump()
    {
        if (isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        else if (canDoubleJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            canDoubleJump = false;
        }
    }

    public void EnableGravityMode()
    {
        usesGravity = true;
        rb.gravityScale = 3;
        canDoubleJump = true;
    }

    public void DisableGravityMode()
    {
        usesGravity = false;
        rb.gravityScale = 0;
        rb.velocity = Vector2.zero;
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
        trailRenderer.endWidth = 0f;
    }

    private void SetPermanentTrailTransparency(float alpha)
    {
        if (trailRenderer == null)
        {
            Debug.LogWarning("TrailRenderer nen� p�ipojen k objektu.");
            return;
        }

        alpha = Mathf.Clamp01(alpha);

        Color startColor = trailRenderer.startColor;
        Color endColor = trailRenderer.endColor;

        trailRenderer.startColor = new Color(startColor.r, startColor.g, startColor.b, alpha);
        trailRenderer.endColor = new Color(endColor.r, endColor.g, endColor.b, alpha * 0.5f);
    }



    public void DisableLightForDuration(float duration)
    {
        if (playerLight == null)
        {
            Debug.LogWarning("Player light is not assigned.");
            return;
        }

        StartCoroutine(DisableLightCoroutine(duration));
    }

    private IEnumerator DisableLightCoroutine(float duration)
    {
        playerLight.enabled = false;

        yield return new WaitForSeconds(duration);

        playerLight.enabled = true;
    }

    public override void TakeDamage(int damage)
    {
        currentHP -= damage;
        if (animator != null)
        {
            animator.SetTrigger("hit");
        }
        if (currentHP <= 0)
        {
            deathPosition = transform.position;
            isAlive = false;
        }
    }

    public void SetTransparency(float alpha)
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            Color color = spriteRenderer.color;
            color.a = Mathf.Clamp01(alpha);
            spriteRenderer.color = color;
        }
        else
        {
            Debug.LogWarning("SpriteRenderer nen� p�ipojen k objektu hr��e.");
        }
        SetPermanentTrailTransparency(0.01f);
    }
    public void Respawn()
    {
        currentHP = 50;
        isAlive = true;
        transform.position = deathPosition;
        deathPosition = Vector3.zero;
        gameObject.SetActive(true);
        playerHpBar.gameObject.SetActive(true);
        isPlayerInUnderground = false;
        StartCoroutine(TemporaryInvulnerability(3f));
    }

    private IEnumerator TemporaryInvulnerability(float duration)
    {
        Collider2D playerCollider = GetComponent<Collider2D>();
        if (playerCollider != null)
        {
            playerCollider.enabled = false;
        }

        SetTransparency(0.5f);

        yield return new WaitForSeconds(duration);

        if (playerCollider != null)
        {
            playerCollider.enabled = true;
        }

        SetTransparency(1f);

    }

    public void ReduceDashCD(float duration)
    {
        if(dashCD - duration >= minDashCD)
        {
            return;
        }
        dashCD -= duration;
    }



}
