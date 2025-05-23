using System.Collections;
using System.Collections.Generic;
using UnityEditor;
//using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Player : Character
{
    Vector2 moveDirection = new Vector2();
    private float moveX, moveY;
    private bool canDash = true;
    private bool isDashing;
    public bool recentlyTeleported = false;

    public static float DEF_DASH_SPEED = 12f;
    public static float DEF_DASH_TIME = 0.2f;
    public static float DEF_DASH_CD = 3f;
    public static float DEF_JUMP_FORCE = 10f;
    public static float DEF_MOVEMENT_SPEED = 3f;
    public static int DEF_MAX_HP = 100;
    public static int DEF_MONEY_AMOUNT = 0;

    [SerializeField]
    public float dashSpeedPermanent = DEF_DASH_SPEED;
    public float dashSpeed = 0;
    [SerializeField]
    public float dashTimePermanent = DEF_DASH_TIME;
    public float dashTime = 0;
    [SerializeField]
    public float dashCDPermanent = DEF_DASH_CD;
    public float dashCD = 0;
    [SerializeField]
    public float jumpForcePermanent = DEF_JUMP_FORCE;
    public float jumpForce = 0;
    public float movementSpeedPermanent = DEF_MOVEMENT_SPEED;
    public float movementSpeed = 0;
    public bool hasMovementSpeedSpell;
    public bool hasAttackSpeedSpell;
    public bool hasHealSpell;
    public bool hasTimeSlowSpell;
    public bool cantUseSpells;
    [SerializeField]
    private StatusBar playerHpBar;

    [SerializeField]
    public int money = DEF_MONEY_AMOUNT;

    public float minDashCD = 0.8f;

    [SerializeField]
    private TrailRenderer trailRenderer;

    public Rigidbody2D rb;

    public GameObject castPoint;

    public List<ColorCore> colorCores;

    private bool canDoubleJump = true;
    private bool isGrounded;
    private bool usesGravity = false;

    [SerializeField]
    private Transform groundCheck;

    [SerializeField]
    private LayerMask groundLayer;

    [SerializeField]
    private Light2D playerLight;

    public bool isDead;

    public Vector3 deathPosition;

    public bool isPlayerInUnderground;

    public CastSpell spell;

    public bool canBeAttacked = true;

    public static Player Instance { get; private set; }

    [SerializeField]
    Abilities abilities;

    private bool messageWasShown = false;


    void Start()
    {
        SaveSystem.LoadPlayer(this);
        currentHP = maxHPpermanent;
        isDead = false;
        colorCores = new List<ColorCore>();
        resetTrailRendered();
        DisableGravityMode();
        SetTransparency(1f);
        cantUseSpells = false;
        canBeAttacked = true;
        abilities.UpdateSpellsVisibility();
        messageWasShown = false;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        
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

        if (Input.GetKeyUp(KeyCode.Delete) && PlayerPrefs.GetInt("DevTools", 0) == 1)
        {
            ResetStats();
        }

        if (Input.GetKeyUp(KeyCode.M) && PlayerPrefs.GetInt("DevTools", 0) == 1)
        {
            money += 100;
        }

        if (Input.GetKeyUp(KeyCode.N) && PlayerPrefs.GetInt("DevTools", 0) == 1)
        {
            currentHP += 100;
        }

        if (usesGravity && Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        if (HasAllColorCores() && !messageWasShown)
        {
            AlertText.Instance.ShowAlert("TELEPORT TO FINAL DESTINATION BY PRESSING [G]", 3f);
            messageWasShown = true;
        }

    }

    public bool HasAllColorCores()
    {
        return colorCores.Count >= 4;
    }

    public void ResetStats()
    {
        dashSpeedPermanent = DEF_DASH_SPEED;
        dashSpeed = 0;
        dashTimePermanent = DEF_DASH_TIME;
        dashTime = 0;
        dashCDPermanent = DEF_DASH_CD;
        dashCD = 0;
        jumpForcePermanent = DEF_JUMP_FORCE;
        jumpForce = 0;
        movementSpeedPermanent = DEF_MOVEMENT_SPEED;
        movementSpeed = 0;
        money = DEF_MONEY_AMOUNT;
    }

    public void AddMoney(int amount)
    {
        money += amount;
    }

    public void BoostMovementSpeedSpell(float speedBoost, float duration)
    {
        StartCoroutine(BoostMovementSpeedCoroutine(speedBoost, duration));
    }

    public void BoostAttackSpeedSpell(float cooldownReduction, float duration)
    {
        StartCoroutine(BoostAttackSpeed(cooldownReduction, duration));
    }

    public void HealSpell()
    {
        currentHP += maxHPpermanent/3;
        audioManager.PlaySFX(audioManager.healSpell);
    }

    public void TimeSlowSpell(float duration)
    {
        StartCoroutine(TimeSlowCoroutine(duration));
        audioManager.PlaySFX(audioManager.timeSlowSpell);
    }

    private IEnumerator TimeSlowCoroutine(float duration)
    {
        Time.timeScale = 0.4f;
        yield return new WaitForSeconds(duration);
        Time.timeScale = 1f;
    }

    private IEnumerator BoostMovementSpeedCoroutine(float speedBoost, float duration)
    {
        movementSpeed += speedBoost;
        audioManager.PlaySFX(audioManager.movementSpeedSpell);
        yield return new WaitForSeconds(duration);
        movementSpeed -= speedBoost; 
    }

    private IEnumerator BoostAttackSpeed(float cooldownReduction, float duration)
    {
        spell.cooldownTime -= cooldownReduction;
        yield return new WaitForSeconds(duration);
        spell.cooldownTime += cooldownReduction;
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
        TakeDamage(maxHPpermanent);
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

        rb.velocity = new Vector2(moveX, moveY).normalized * dashSpeedPermanent;
        setTrailrendererForDash();

        yield return new WaitForSeconds(dashTimePermanent);

        resetTrailRendered();
        isDashing = false;

        yield return new WaitForSeconds(dashCDPermanent);
        canDash = true;
    }

    void Move()
    {
        if (usesGravity)
        {
            rb.velocity = new Vector2(moveDirection.x * (movementSpeedPermanent + movementSpeed), rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(moveDirection.x * (movementSpeedPermanent + movementSpeed), moveDirection.y * (movementSpeedPermanent + movementSpeed));
        }
    }

    void Jump()
    {
        if (isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForcePermanent);
        }
        else if (canDoubleJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForcePermanent);
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
        if(currentHP > 0)
        {
            currentHP -= damage;
        }
        if (animator != null)
        {
            animator.SetTrigger("hit");
        }
        if (currentHP <= 0)
        {
            deathPosition = transform.position;
            isAlive = false;
            audioManager.PlaySFX(audioManager.Death);
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
        canBeAttacked = false;
        currentHP = maxHPpermanent/2;
        isAlive = true;
        transform.position = deathPosition;
        deathPosition = Vector3.zero;
        gameObject.SetActive(true);
        playerHpBar.gameObject.SetActive(true);
        isPlayerInUnderground = false;
        audioManager.StopTickingSound();
        audioManager.PlaySFX(audioManager.Revive);
        cantUseSpells = false;
        StartCoroutine(TemporaryInvulnerability(2f));
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

        canBeAttacked = true;
    }

    public void ReduceDashCD(float duration)
    {
        if(dashCDPermanent - duration >= minDashCD)
        {
            return;
        }
        dashCDPermanent -= duration;
    }

    public void ReduceDashCDPermanent(float duration)
    {
        if (dashCD - duration >= minDashCD)
        {
            return;
        }
        dashCD -= duration;
    }



}
