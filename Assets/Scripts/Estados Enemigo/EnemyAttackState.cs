using UnityEngine;

public class EnemyAttackState : EnemyState
{
    private float attackCooldown = 1f;
    private float lastAttackTime = 0f;

    public EnemyAttackState(EnemyStateMachine enemy) : base(enemy) { }

    public override void Enter()
    {
        enemy.StopMoving();
    }

    public override void Update()
    {
        if (enemy.player == null)
        {
            enemy.ChangeState(enemy.patrolState);
            return;
        }

        float distance = Vector2.Distance(enemy.transform.position, enemy.player.position);

        // Si el jugador se aleja, volver a perseguir
        if (distance > enemy.attackRange)
        {
            enemy.ChangeState(enemy.chaseState);
            return;
        }

        // Aplicar ataque (aquí puedes llamar a un método de daño en el jugador)
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            Debug.Log("Enemigo ataca al jugador!");
            lastAttackTime = Time.time;

            // Ejemplo: puedes hacer que el jugador pierda vida
            // enemy.player.GetComponent<PlayerStateMachine>().TakeDamage(1);
        }
    }

    public override void Exit() { }
}

