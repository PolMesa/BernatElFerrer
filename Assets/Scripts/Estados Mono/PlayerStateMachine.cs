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
    public float attackDamage = 25f;
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

    // -----------------------------
    // Máquina de Estados
    // -----------------------------
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

        // Inicializar estados
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

        // Detectar ataque con click izquierdo
        if (Input.GetMouseButtonDown(0) && Time.time > lastAttackTime + attackCooldown && !(currentState is AttackState))
        {
            ChangeState(attackState);
        }

        currentState?.Update();

        // Animaciones básicas (excepto durante ataque)
        if (!(currentState is AttackState))
        {
            animator.SetFloat("Speed", Mathf.Abs(moveInput));
            animator.SetFloat("VerticalVelocity", rb2D.linearVelocity.y);
            animator.SetBool("IsGrounded", isGrounded);
        }

        // Flip del sprite (excepto durante ataque)
        if (moveInput != 0 && !(currentState is AttackState))
            transform.localScale = new Vector3(Mathf.Sign(moveInput), 1, 1);
    }

    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
    }

    // -----------------------------
    // Funciones para los estados
    // -----------------------------
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

    // -----------------------------
    // ANIMATION EVENT: Frame de golpe
    // -----------------------------
    public void OnAttackAnimationHit()
    {
        Debug.Log("🎯 ANIMATION EVENT: Frame de golpe del jugador");
        PerformAttack();
    }

    // -----------------------------
    // ANIMATION EVENT: Fin de animación
    // -----------------------------
    public void OnAttackAnimationEnd()
    {
        Debug.Log("🏁 Animación de ataque terminada");
        // El AttackState manejará el cambio de estado
    }

    // -----------------------------
    // Función para realizar el ataque
    // -----------------------------
    public void PerformAttack()
    {
        Debug.Log("🗡️ Jugador realiza ataque!");
        lastAttackTime = Time.time;

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(
            attackPoint.position,
            attackRange,
            enemyLayer
        );

        foreach (Collider2D enemy in hitEnemies)
        {
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakePercentageDamage(attackDamage / 100f);
                Debug.Log($"✅ Golpeó a {enemy.name} - Daño: {attackDamage}%");
            }
        }
    }

    // -----------------------------
    // Debug: Ver rango de ataque
    // -----------------------------
    void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }

    // -----------------------------
    // Funciones de Score / Items
    // -----------------------------
    void UpdateScore()
    {
        scoreText.text = "Score: " + score;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Recolectables
        if (other.CompareTag("Item"))
        {
            score++;
            UpdateScore();
            Destroy(other.gameObject);
        }

        // Checkpoints
        if (other.CompareTag("Checkpoint"))
        {
            if (CheckpointManager.Instance != null)
                CheckpointManager.Instance.ActivateCheckpoint(other.transform.position);
        }

         if (other.CompareTag("Gema"))
         {
            Destroy(other.gameObject);
            gema.gameObject.SetActive(true);
         }
    }

    // -----------------------------
    // Respawn / Muerte
    // -----------------------------
    public void Die()
    {
        Debug.Log("💀 Jugador muerto - Llamando a Respawn");
        Respawn();
    }

    public void Respawn()
    {
        Vector3 targetPos = respawnPosition;

        if (CheckpointManager.Instance != null && CheckpointManager.Instance.HasCheckpoint())
        {
            targetPos = CheckpointManager.Instance.GetCurrentCheckpoint();
            Debug.Log("Jugador respawneado en checkpoint: " + targetPos);
        }
        else
        {
            Debug.Log("Jugador respawneado en posición inicial");
        }

        // Teletransportar
        transform.position = targetPos;

        // Reiniciar físicas
        if (rb2D != null)
        {
            rb2D.linearVelocity = Vector2.zero;
            rb2D.angularVelocity = 0f;
        }

        // Resetear estado a Idle
        ChangeState(idleState);



    }
    
}

