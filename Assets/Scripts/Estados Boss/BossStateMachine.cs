using UnityEngine;
using UnityEngine.UI;

public class BossStateMachine : MonoBehaviour
{
    [Header("Referencias")]
    public Transform player;
    public Animator animator;
    public Rigidbody2D rb2D;

    [Header("Salud")]
    public float maxHealth = 500f;
    private float currentHealth;
    public Slider healthSlider;

    [Header("Rangos")]
    public float detectionRange = 8f;
    public float attackRange = 2f;

    [Header("Movimiento")]
    public float moveSpeed = 3f;

    [Header("Ataque")]
    public float attackDamage = 2f;
    public float attackCooldown = 2f;
    private float lastAttackTime = 0f;
    public Transform attackPoint;
    public LayerMask playerLayer;

    [Header("Estados")]
    public BossState currentState;
    public BossIdleState idleState;
    public BossChaseState chaseState;
    public BossAttackState attackState;

    // Propiedades
    public bool IsPlayerDetected => player != null &&
        Vector2.Distance(transform.position, player.position) <= detectionRange;

    public bool IsPlayerInAttackRange => player != null &&
        Vector2.Distance(transform.position, player.position) <= attackRange;

    public bool CanAttack => Time.time > lastAttackTime + attackCooldown;

    void Start()
    {
        // Obtener componentes
        if (rb2D == null) rb2D = GetComponent<Rigidbody2D>();
        if (animator == null) animator = GetComponent<Animator>();

        // Inicializar salud
        currentHealth = maxHealth;
        UpdateHealthUI();

        // Buscar jugador automáticamente
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) player = playerObj.transform;
        }

        // Inicializar estados
        idleState = new BossIdleState(this);
        chaseState = new BossChaseState(this);
        attackState = new BossAttackState(this);

        // Empezar en idle
        ChangeState(idleState);
    }

    void Update()
    {
        currentState?.Update();
    }

    public void ChangeState(BossState newState)
    {
        if (currentState == newState) return;

        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public void MoveTowards(Vector2 target)
    {
        if (rb2D == null) return;

        Vector2 direction = (target - (Vector2)transform.position).normalized;

        if (direction.magnitude > 0.1f)
        {
            rb2D.linearVelocity = new Vector2(direction.x * moveSpeed, rb2D.linearVelocity.y);

            // ✅ INVERTIDO: Si el sprite mira a la derecha por defecto
            if (direction.x > 0) // Jugador a la DERECHA
            {
                transform.localScale = new Vector3(-1, 1, 1); // FLIP a izquierda
            }
            else if (direction.x < 0) // Jugador a la IZQUIERDA
            {
                transform.localScale = new Vector3(1, 1, 1); // Sin flip (mira derecha)
            }

            if (animator != null)
            {
                animator.Play("BossWalk");
            }
        }
        else
        {
            StopMoving();
        }
    }


    public void StopMoving()
    {
        if (rb2D != null)
        {
            rb2D.linearVelocity = new Vector2(0, rb2D.linearVelocity.y);
        }

        // Reproducir animación de idle
        if (animator != null && !animator.GetCurrentAnimatorStateInfo(0).IsName("BossIdle"))
        {
            animator.Play("BossIdle");
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(0, currentHealth);

        UpdateHealthUI();

        // Flash de daño
        StartCoroutine(DamageFlash());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private System.Collections.IEnumerator DamageFlash()
    {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        if (sprite != null)
        {
            Color original = sprite.color;
            sprite.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            sprite.color = original;
        }
    }

    private void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }

    private void Die()
    {
        // Detener movimiento
        StopMoving();

        // Desactivar colliders
        Collider2D[] colliders = GetComponents<Collider2D>();
        foreach (Collider2D col in colliders)
        {
            col.enabled = false;
        }

        // Reproducir animación de muerte
        if (animator != null)
        {
            animator.Play("BossDeath");
        }

        // Desactivar este script
        enabled = false;

        // Destruir después de 3 segundos
        Destroy(gameObject, 3f);
    }

    public void PerformAttack()
    {
        lastAttackTime = Time.time;

        // Reproducir animación de ataque
        if (animator != null)
        {
            animator.Play("BossAttack");
        }
    }

    // Llamado por Animation Event en la animación BossAttack
    public void OnAttackAnimationHit()
    {
        if (player == null || !IsPlayerInAttackRange) return;

        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage((int)attackDamage); // AQUÍ LA CORRECCIÓN
        }
    }


    // Para debug en el editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    public float GetCurrentHealth() => currentHealth;
}
