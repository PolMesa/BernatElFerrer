using UnityEngine;

public class EnemyAttackState : EnemyState
{
    private float attackCooldown = 1f;
    private float currentCooldown = 0f;
    private bool canAttack = true;
    private int damageAmount = 1;

    public EnemyAttackState(EnemyStateMachine enemy) : base(enemy) { }

    public override void Enter()
    {
        Debug.Log("🎯 ENEMIGO ATACA");
        enemy.StopMoving();
        currentCooldown = 0f;
        canAttack = true;

        // Iniciar ataque
        StartAttack();
    }

    public override void Update()
    {
        if (enemy.player == null)
        {
            ReturnToPatrol();
            return;
        }

        float distance = Vector2.Distance(enemy.transform.position, enemy.player.position);

        if (distance > enemy.attackRange)
        {
            if (distance <= enemy.detectionRange)
            {
                enemy.ChangeState(enemy.chaseState);
            }
            else
            {
                ReturnToPatrol();
            }
            return;
        }

        if (!canAttack)
        {
            currentCooldown -= Time.deltaTime;
            if (currentCooldown <= 0f)
            {
                canAttack = true;
                StartAttack();
            }
        }
    }

    private void StartAttack()
    {
        Debug.Log("⚔️ Iniciando ataque enemigo");
        enemy.animator.Play("Ataque");
        canAttack = false;
        currentCooldown = attackCooldown;

        // El daño se aplicará con Animation Event
        // Invoke(nameof(ApplyDamage), 0.3f); // Opcional: si no usas Animation Events
    }

    // MÉTODO PARA ANIMATION EVENT
    public void ApplyDamage()
    {
        Debug.Log("💥 Enemigo aplica daño (Animation Event)");

        if (enemy.player == null) return;

        float distance = Vector2.Distance(enemy.transform.position, enemy.player.position);
        if (distance <= enemy.attackRange)
        {
            PlayerHealth playerHealth = enemy.player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
                Debug.Log($"✅ Enemigo hizo {damageAmount} de daño al jugador");
            }
        }
    }

    private void ReturnToPatrol()
    {
        enemy.ChangeState(enemy.patrolState);
    }

    public override void Exit()
    {
        Debug.Log("Enemigo deja de atacar");
    }
}
