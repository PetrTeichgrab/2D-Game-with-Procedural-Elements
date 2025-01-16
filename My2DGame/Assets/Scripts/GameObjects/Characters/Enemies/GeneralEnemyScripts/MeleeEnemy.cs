using UnityEngine;
using System.Collections;

public abstract class MeleeEnemy : Character
{
    public float movementSpeed;
    public float visionDistance;
    public float approachDistance;
    public int attackMaxCD;
    public int attackMinCD;
    public int damage = 5;
    public float dashTime;
    public float dashSpeed;
    public Collider2D characterCollider;
    private float attackCooldownTimer = Mathf.Infinity;
    public Transform player;
    public TrailRenderer trailRenderer;
    public Rigidbody2D rb;
    public float attackCooldown = 2f;
    protected float startMovementSpeed;
    protected bool isDashing;
    [SerializeField]
    private float lastAttackTime = 0f;
    private Vector2 avoidDirection; // Pamatuje si aktu�ln� sm�r vyh�b�n�
    private float avoidTimer = 0f;  // �as vyh�b�n� p�ek�ce
    private const float avoidDuration = 0.3f; // D�lka �asu, po kterou se nep��tel vyh�b� p�ek�ce

    private Vector2 lastKnownPlayerPosition;
    private bool isAvoidingObstacle = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        attackCooldownTimer = 0;
        if (trailRenderer != null)
        {
            trailRenderer.enabled = false;
        }
        isAlive = true;
        startMovementSpeed = movementSpeed;
    }


    protected IEnumerator DashAttack()
    {
        isDashing = true;
        if (trailRenderer != null)
        {
            trailRenderer.enabled = true;
        }
        Vector2 direction = (player.position - transform.position).normalized;

        rb.velocity = direction * dashSpeed;

        yield return new WaitForSeconds(dashTime);

        rb.velocity = Vector2.zero;
        if (trailRenderer != null)
        {
            trailRenderer.enabled = false;
        }
        isDashing = false;
    }

    protected void MoveToPlayer()
    {
        Vector2 directionToPlayer = (player.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, 1, LayerMask.GetMask("Obstacle"));

        if (hit.collider != null)
        {
            // Pokud je p�ek�ka, aktivuj vyh�b�n�
            AvoidObstacle(hit, directionToPlayer);
        }
        else if (avoidTimer > 0)
        {
            // Pokra�uj v aktu�ln�m sm�ru vyh�b�n�
            avoidTimer -= Time.deltaTime;
            transform.position += (Vector3)(avoidDirection * movementSpeed * Time.deltaTime);
        }
        else
        {
            // P��m� pohyb k hr��i, pokud nen� p�ek�ka
            transform.position = Vector2.MoveTowards(transform.position, player.position, movementSpeed * Time.deltaTime);
        }
    }

    private void AvoidObstacle(RaycastHit2D hit, Vector2 directionToPlayer)
    {
        if (avoidTimer <= 0)
        {
            // Nastav sm�r vyh�b�n� jen jednou p�i zji�t�n� p�ek�ky
            Vector2 obstacleDirection = hit.normal; // Norm�la ur�uje sm�r od p�ek�ky
            avoidDirection = (directionToPlayer + obstacleDirection).normalized;
            avoidTimer = avoidDuration; // Nastav �as vyh�b�n�
        }

        // Pohyb ve sm�ru vyh�b�n�
        transform.position += (Vector3)(avoidDirection * movementSpeed * Time.deltaTime);

        // Debugging (voliteln�)
        Debug.DrawLine(transform.position, hit.point, Color.red); // Linie k p�ek�ce
        Debug.DrawRay(transform.position, avoidDirection * visionDistance, Color.green); // Linie sm�ru vyh�b�n�
    }



    protected void Die()
    {
        rb.velocity = new Vector2(0, 0);
    }

    protected void StopOnCurrentPosition()
    {
        if (TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        movementSpeed = 0;
    }

    protected bool IsInApproachDistance()
    {
        return Vector2.Distance(transform.position, player.position) < approachDistance &&
               Vector2.Distance(transform.position, player.position) < visionDistance;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player character = collision.gameObject.GetComponent<Player>();
            if (character != null && isAlive)
            {
                if (Time.time >= lastAttackTime + attackCooldown)
                {
                    animator.SetTrigger("attack");
                    character.TakeDamage(10);
                    lastAttackTime = Time.time;
                }
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector2 pushbackDirection = (transform.position - collision.transform.position).normalized;
            rb.AddForce(pushbackDirection * 50f);
        }
    }
}
