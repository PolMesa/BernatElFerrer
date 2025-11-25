using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 2f;

    [Header("Detection & Attack")]
    public float detectionRange = 5f;
    public float attackRange = 1f;
    public Transform player;

    [Header("Patrol Points")]
    public Transform leftPoint;
    public Transform rightPoint;
    [HideInInspector] public Transform targetPoint;

    [Header("Stats")]
    public int maxHealth = 3;
    private int currentHealth;

    [HideInInspector] public Rigidbody2D rb2D;

    // Estados
    public EnemyState currentState;
    public EnemyPatrolState patrolState;
    public EnemyChaseState chaseState;
    public EnemyAttackState attackState;
    public EnemyDeathState deathState;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        rb2D.gravityScale = 0; // si solo se mueve horizontal

        currentHealth = maxHealth;

        // Inicializar estados
        patrolState = new EnemyPatrolState(this);
        chaseState = new EnemyChaseState(this);
        attackState = new EnemyAttackState(this);
        deathState = new EnemyDeathState(this);

        targetPoint = rightPoint; // Comienza patrullando hacia la derecha

        ChangeState(patrolState);
    }

    void Update()
    {
        currentState?.Update();
    }

    // Mover hacia un objetivo
    public void MoveTowards(Vector2 target)
    {
        Vector2 direction = (target - (Vector2)transform.position).normalized;
        rb2D.linearVelocity = new Vector2(direction.x * moveSpeed, rb2D.linearVelocity.y);

        // Flip del sprite seg�n direcci�n
        if (direction.x != 0)
            transform.localScale = new Vector3(Mathf.Sign(direction.x), 1, 1);
    }

    public void StopMoving()
    {
        rb2D.linearVelocity = Vector2.zero;
    }

    // Cambiar estado
    public void ChangeState(EnemyState newState)
    {
        if (currentState == newState) return;

        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    // Cambiar punto de patrulla
    public void SwitchTargetPoint()
    {
        targetPoint = targetPoint == leftPoint ? rightPoint : leftPoint;
    }

    // Recibir da�o
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
            Die();
    }

    // Morir
    public void Die()
    {
        ChangeState(deathState);
    }
}



