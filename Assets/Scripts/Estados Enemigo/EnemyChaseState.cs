using UnityEngine;

public class EnemyChaseState : EnemyState
{
    public EnemyChaseState(EnemyStateMachine enemy) : base(enemy) { }

    public override void Enter()
    {
        // Activar animación de andar al entrar en chase
        if (enemy.animator != null)
        {
            enemy.animator.Play("Walk"); // Ajusta al nombre de tu animación de andar
        }
    }

    public override void Update()
    {
        if (enemy.player == null)
        {
            enemy.ChangeState(enemy.patrolState);
            return;
        }

        float distance = Vector2.Distance(enemy.transform.position, enemy.player.position);

        if (distance > enemy.detectionRange)
        {
            enemy.ChangeState(enemy.patrolState);
            return;
        }
        else if (distance <= enemy.attackRange)
        {
            enemy.ChangeState(enemy.attackState);
            return;
        }

        // Perseguir al jugador
        enemy.MoveTowards(enemy.player.position);
    }

    public override void Exit()
    {
        enemy.StopMoving();
    }
}







