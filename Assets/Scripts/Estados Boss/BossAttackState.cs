using UnityEngine;

public class BossAttackState : BossState
{
    private float attackDuration = 3f; // AUMENTA este tiempo
    private float currentAttackTime = 0f;
    private bool attackStarted = false;

    public BossAttackState(BossStateMachine boss) : base(boss) { }

    public override void Enter()
    {
        Debug.Log("👊 Boss ATACANDO");

        // Detener movimiento
        boss.StopMoving();

        // Iniciar ataque
        boss.PerformAttack();

        currentAttackTime = 0f;
        attackStarted = true;
    }

    public override void Update()
    {
        currentAttackTime += Time.deltaTime;

        // ESPERAR que termine la animación completa
        if (currentAttackTime >= attackDuration)
        {
            // Solo cambiar de estado cuando termine el tiempo
            DecideNextState();
        }

        // Opcional: Verificar si el jugador se fue MUY lejos
        if (boss.player == null || !boss.IsPlayerDetected)
        {
            boss.ChangeState(boss.idleState);
            return;
        }
    }

    private void DecideNextState()
    {
        // Decidir qué hacer después de atacar COMPLETAMENTE
        if (boss.IsPlayerInAttackRange && boss.CanAttack)
        {
            // Si sigue en rango, atacar de nuevo
            boss.ChangeState(boss.attackState);
        }
        else if (boss.IsPlayerDetected)
        {
            // Si detecta pero no está en rango, perseguir
            boss.ChangeState(boss.chaseState);
        }
        else
        {
            // Si no detecta, volver a idle
            boss.ChangeState(boss.idleState);
        }
    }

    public override void Exit()
    {
        Debug.Log("✅ Boss terminó ataque completo");
        attackStarted = false;
    }
}
