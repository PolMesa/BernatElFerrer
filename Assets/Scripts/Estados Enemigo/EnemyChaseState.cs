using UnityEngine;

public class EnemyChaseState : EnemyState
{
    public EnemyChaseState(EnemyStateMachine enemy) : base(enemy) { }

    public override void Enter() { }

    public override void Update()
    {
        if (enemy.player == null) return;

        float distance = Vector2.Distance(enemy.transform.position, enemy.player.position);

        // Si está fuera del rango de detección, volver a patrullar
        if (distance > enemy.detectionRange)
        {
            enemy.ChangeState(enemy.patrolState);
            return;
        }

        // Si está lo suficientemente cerca, atacar
        if (distance <= enemy.attackRange)
        {
            enemy.ChangeState(enemy.attackState);
            return;
        }

        // Moverse hacia el jugador
        enemy.MoveTowards(enemy.player.position);
    }

    public override void Exit()
    {
        enemy.StopMoving();
    }
}


