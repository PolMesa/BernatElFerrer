using UnityEngine;

public class BossIdleState : BossState
{
    private float idleTime = 0f;

    public BossIdleState(BossStateMachine boss) : base(boss) { }

    public override void Enter()
    {
        Debug.Log("😴 Boss en IDLE (esperando jugador)");
        boss.StopMoving();

        if (boss.animator != null)
        {
            boss.animator.Play("BossIdle");
        }

        idleTime = 0f;
    }

    public override void Update()
    {
        idleTime += Time.deltaTime;

        // Verificar si detecta al jugador
        if (boss.IsPlayerDetected)
        {
            Debug.Log("🎯 Jugador detectado! Cambiando a CHASE");
            boss.ChangeState(boss.chaseState);
            return;
        }

        // (Opcional) Patrullar si pasa mucho tiempo idle
        if (idleTime > 5f)
        {
            // Podrías añadir patrulla simple aquí
            // boss.ChangeState(patrolState);
        }
    }

    public override void Exit()
    {
        Debug.Log("Saliendo de BossIdle");
    }
}
