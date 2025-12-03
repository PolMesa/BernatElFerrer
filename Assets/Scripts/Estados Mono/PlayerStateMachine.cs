using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerStateMachine : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 7f;

    [Header("Attack Settings")]
    public float attackRange = 1f;
    public float attackDamage = 50f;
    public Transform attackPoint;
    public LayerMask enemyLayer;
    public float attackCooldown = 0.5f;
    public float lastAttackTime = 0f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float checkRadius = 0.05f;
    public LayerMask groundLayer;
    public bool isGrounded;

    [Header("Score")]
    public TMP_Text scoreText;
    public int score = 0;
    public Image gema;

    [Header("Respawn")]
    public Vector3 respawnPosition;

    [HideInInspector] public Rigidbody2D rb2D;
    [HideInInspector] public Animator animator;

    public PlayerState currentState;
    public IdleState idleState;
    public WalkState walkState;
    public JumpState jumpState;
    public FallingState fallingState;
    public AttackState attackState;

    float moveInput;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        respawnPosition = transform.position;

        idleState = new IdleState(this);
        walkState = new WalkState(this);
        jumpState = new JumpState(this);
        fallingState = new FallingState(this);
        attackState = new AttackState(this);

        ChangeState(idleState);
    }

    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetMouseButtonDown(0) && Time.time > lastAttackTime + attackCooldown && !(currentState is AttackState))
        {
            ChangeState(attackState);
        }

        currentState?.Update();

        if (!(currentState is AttackState))
        {
            animator.SetFloat("Speed", Mathf.Abs(moveInput));
            animator.SetFloat("VerticalVelocity", rb2D.linearVelocity.y);
            animator.SetBool("IsGrounded", isGrounded);
        }

        if (moveInput != 0 && !(currentState is AttackState))
            transform.localScale = new Vector3(Mathf.Sign(moveInput), 1, 1);
    }

    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
    }

    public void MoveHorizontal(float input)
    {
        rb2D.linearVelocity = new Vector2(input * moveSpeed, rb2D.linearVelocity.y);
    }

    public void Jump()
    {
        rb2D.linearVelocity = new Vector2(rb2D.linearVelocity.x, jumpForce);
    }

    public void ChangeState(PlayerState newState)
    {
        if (currentState == newState) return;

        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public float GetMoveInput() => moveInput;

    public void OnAttackAnimationHit()
    {
        PerformAttack();
    }

    public void OnAttackAnimationEnd()
    {
    }

    public void PerformAttack()
    {
        lastAttackTime = Time.time;

        Collider2D[] allColliders = Physics2D.OverlapCircleAll(attackPoint.position, attackRange);

        bool hitEnemy = false;

        foreach (Collider2D collider in allColliders)
        {
            EnemyHealth enemyHealth = collider.GetComponent<EnemyHealth>();
            if (enemyHealth == null)
            {
                enemyHealth = collider.GetComponentInParent<EnemyHealth>();
            }

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(attackDamage);
                hitEnemy = true;
                StartCoroutine(FlashEnemy(collider.gameObject));
                break;
            }
        }
    }

    private System.Collections.IEnumerator FlashEnemy(GameObject enemy)
    {
        SpriteRenderer sprite = enemy.GetComponent<SpriteRenderer>();
        if (sprite != null)
        {
            Color original = sprite.color;
            sprite.color = Color.white;
            yield return new WaitForSeconds(0.1f);
            sprite.color = original;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }

    void UpdateScore()
    {
        scoreText.text = "Score: " + score;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Item"))
        {
            score++;
            UpdateScore();
            Destroy(other.gameObject);
        }

        if (other.CompareTag("Checkpoint"))
        {
            if (CheckpointManager.Instance != null)
                CheckpointManager.Instance.ActivateCheckpoint(other.transform.position);
        }

        if (other.CompareTag("Gema"))
        {
            Destroy(other.gameObject);
            gema.gameObject.SetActive(true);
            GameObject Puerta = GameObject.FindWithTag("Puerta");
            if (Puerta != null)
            {
                Destroy(Puerta);
            }
        }
    }

    // ========== RETROCESO FUNCIONAL ==========
    public void Die()
    {
        Respawn();
    }

    public void ApplyDamage()
    {
        // 1. RETROCESO INMEDIATO
        if (rb2D != null)
        {
            // Dirección opuesta a donde mira
            float direction = -Mathf.Sign(transform.localScale.x);

            // Fuerza del retroceso
            float knockbackX = direction * 15f;
            float knockbackY = 8f;

            // Aplicar velocidad directamente
            rb2D.linearVelocity = new Vector2(knockbackX, knockbackY);
        }

        // 2. DAÑO
        PlayerHealth playerHealth = GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(1);
        }
    }
    public void ApplyKnockbackFromEnemy(Vector2 enemyPosition)
    {
        if (rb2D == null) return;

        // Calcular dirección AWAY del enemigo
        Vector2 knockbackDirection = ((Vector2)transform.position - enemyPosition).normalized;

        // Añadir un poco de fuerza hacia arriba
        knockbackDirection.y = Mathf.Max(knockbackDirection.y, 0.3f);
        knockbackDirection.Normalize();

        // Aplicar fuerza
        float knockbackForce = 20f;
        rb2D.linearVelocity = knockbackDirection * knockbackForce;

        Debug.Log($"💥 Retroceso desde enemigo: {knockbackDirection * knockbackForce}");
    }


    public void Respawn()
    {
        // Aplicar daño/retroceso
        ApplyDamage();

        // Teletransportar
        Vector3 targetPos = respawnPosition;

        if (CheckpointManager.Instance != null && CheckpointManager.Instance.HasCheckpoint())
        {
            targetPos = CheckpointManager.Instance.GetCurrentCheckpoint();
        }

        transform.position = targetPos;

        // Resetear estado
        ChangeState(idleState);
    }
}






