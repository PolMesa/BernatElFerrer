using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 2f;
    public float reachDistance = 0.3f; // ← AÑADE ESTA LÍNEA

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

    // States
    public EnemyState currentState;
    public EnemyPatrolState patrolState;
    public EnemyChaseState chaseState;
    public EnemyAttackState attackState;
    public EnemyDeathState deathState;

    public Animator animator;

    
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();

        // ❗ Para que no lo empuje el jugador ni flote
        rb2D.gravityScale = 0;
        rb2D.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;

        animator = GetComponent<Animator>();

        currentHealth = maxHealth;

        // Inicializar estados
        patrolState = new EnemyPatrolState(this);
        chaseState = new EnemyChaseState(this);
        attackState = new EnemyAttackState(this);
        deathState = new EnemyDeathState(this);

        // Empezar patrullando hacia la derecha
        targetPoint = rightPoint;

        ChangeState(patrolState);
 

            // VERIFICACIÓN CRÍTICA
            if (leftPoint != null && rightPoint != null)
            {
                float distanceBetweenPoints = Vector2.Distance(leftPoint.position, rightPoint.position);
                Debug.Log($"📏 Distancia entre puntos: {distanceBetweenPoints}");

                if (distanceBetweenPoints < 1f)
                {
                    Debug.LogError("❌ LOS PUNTOS ESTÁN DEMASIADO CERCA O EN LA MISMA POSICIÓN");
                }
            }
            else
            {
                Debug.LogError("❌ FALTAN PUNTOS DE PATROL ASIGNADOS");
            }

            // ... resto del código ...
        }


    void Update()
    {
        currentState?.Update();
    }

    public void MoveTowards(Vector2 target)
    {
        Vector2 direction = (target - (Vector2)transform.position).normalized;

        rb2D.linearVelocity = new Vector2(direction.x * moveSpeed, rb2D.linearVelocity.y);

        // Flip del sprite
        if (direction.x != 0)
            transform.localScale = new Vector3(Mathf.Sign(direction.x), 1, 1);
    }

    public void StopMoving()
    {
        rb2D.linearVelocity = Vector2.zero;
    }

    public void ChangeState(EnemyState newState)
    {
        if (currentState == newState) return;

        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public void SwitchTargetPoint()
    {
        if (targetPoint == leftPoint)
        {
            targetPoint = rightPoint;
            Debug.Log($"🔄 Cambio: Left → Right en posición {rightPoint.position}");
        }
        else if (targetPoint == rightPoint)
        {
            targetPoint = leftPoint;
            Debug.Log($"🔄 Cambio: Right → Left en posición {leftPoint.position}");
        }
        else
        {
            // Si es null o no coincide, resetear
            Debug.LogWarning("⚠️ TargetPoint no válido, reseteando a Right");
            targetPoint = rightPoint;
        }
    }





    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
            Die();
    }

    public void Die()
    {
        ChangeState(deathState);
    }

    public void PlayAttackAnimation()
    {
        if (animator != null)
        {
            Debug.Log("🎭 Reproduciendo animación de ataque");

            // Opción 1: Usar Play (si tienes un estado llamado "Ataque")
            animator.Play("Ataque");

            // Opción 2: O usar trigger (si tienes un trigger en el Animator)
            // animator.SetTrigger("Attack");
        }
        else
        {
            Debug.LogError("Animator no encontrado en el enemigo");
        }
    }


    // --- Helpers para los estados ---

    public bool PlayerInDetectionRange()
    {
        return Vector2.Distance(transform.position, player.position) <= detectionRange;
    }

    public bool PlayerInAttackRange()
    {
        return Vector2.Distance(transform.position, player.position) <= attackRange;
    }
    void OnDrawGizmosSelected()
    {
        // Rango de detección (Amarillo)
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // Rango de ataque (Rojo)
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // Línea hacia el jugador si está asignado
        if (player != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, player.position);
        }
    }
    public void OnAttackAnimationHit()
    {
        // Este método será llamado por Animation Event
        if (currentState is EnemyAttackState attackState)
        {
            attackState.ApplyDamage();
        }
    }



}









