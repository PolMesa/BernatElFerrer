using UnityEngine;

public class EnemyAttackState : EnemyState
{
    private float attackDuration = 1f;
    private float currentAttackTime = 0f;
    private bool attackExecuted = false;

    public EnemyAttackState(EnemyStateMachine enemy) : base(enemy) { }

    public override void Enter()
    {
        Debug.Log("🎯 ENTRANDO EN ESTADO ATAQUE");

        enemy.StopMoving();
        currentAttackTime = 0f;
        attackExecuted = false;

        // REPRODUCIR ANIMACIÓN DE ATAQUE
        enemy.animator.Play("Ataque");
        Debug.Log("🎭 Animación de Ataque activada");
    }

    public override void Update()
    {
        if (enemy.player == null)
        {
            ReturnToPatrol();
            return;
        }

        currentAttackTime += Time.deltaTime;

        // Ejecutar lógica de ataque a mitad de la animación
        if (!attackExecuted && currentAttackTime >= attackDuration * 0.3f)
        {
            ExecuteAttack();
            attackExecuted = true;
        }

        // Cuando termina la animación de ataque
        if (currentAttackTime >= attackDuration)
        {
            DecideNextState();
        }
    }

    private void ExecuteAttack()
    {
        Debug.Log("⚔️ Ejecutando ataque");
        float distance = Vector2.Distance(enemy.transform.position, enemy.player.position);
        if (distance <= enemy.attackRange)
        {
            Debug.Log("💥 Ataque conectado!");
            // player.GetComponent<PlayerHealth>().TakeDamage(1);
        }
    }

    private void DecideNextState()
    {
        if (enemy.player == null)
        {
            ReturnToPatrol();
            return;
        }

        float distance = Vector2.Distance(enemy.transform.position, enemy.player.position);

        if (distance <= enemy.attackRange)
        {
            // Sigue en rango, atacar de nuevo
            Debug.Log("🔁 Atacar de nuevo");
            enemy.ChangeState(enemy.attackState);
        }
        else if (distance <= enemy.detectionRange)
        {
            // Perseguir - LA ANIMACIÓN DE ANDAR SE ACTIVARÁ AUTOMÁTICAMENTE EN CHASESTATE
            Debug.Log("🎯 Cambiar a Chase");
            enemy.ChangeState(enemy.chaseState);
        }
        else
        {
            // Volver a patrullar
            ReturnToPatrol();
        }
    }

    private void ReturnToPatrol()
    {
        Debug.Log("🚶 Volver a Patrol");
        enemy.ChangeState(enemy.patrolState);
    }

    public override void Exit()
    {
        Debug.Log("Saliendo del estado de ataque");
    }
}












