using UnityEngine;

public class EnemyPatrolState : EnemyState
{
    public EnemyPatrolState(EnemyStateMachine enemy) : base(enemy) { }

    public override void Enter()
    {
        //Debug.Log("🚶 Entrando en Patrol State");

        // Asegurar que tiene un punto objetivo
        if (enemy.targetPoint == null)
        {
            enemy.targetPoint = enemy.rightPoint;
            //Debug.Log("📍 TargetPoint era null, asignado a rightPoint");
        }

        //Debug.Log($"🎯 Punto objetivo: {enemy.targetPoint.name}");
    }

    public override void Update()
    {
        // Detectar jugador
        if (enemy.PlayerInDetectionRange())
        {
            enemy.ChangeState(enemy.chaseState);
            return;
        }

        // Verificar si los puntos están asignados
        if (enemy.targetPoint == null)
        {
            //Debug.LogError("❌ targetPoint es null!");
            return;
        }

        // Calcular distancia al punto objetivo
        float distanceToTarget = Vector2.Distance(enemy.transform.position, enemy.targetPoint.position);

        // DEBUG cada 2 segundos
        if (Time.frameCount % 120 == 0)
        {
            //Debug.Log($"📏 Distancia a {enemy.targetPoint.name}: {distanceToTarget:F2}, Reach: {enemy.reachDistance}");
        }

        // Si llegó al punto, cambiar al siguiente
        if (distanceToTarget <= enemy.reachDistance)
        {
            //Debug.Log($"✅ Llegó a {enemy.targetPoint.name} - Cambiando punto");
            enemy.SwitchTargetPoint();
        }
        else
        {
            // Moverse hacia el punto objetivo
            enemy.MoveTowards(enemy.targetPoint.position);
        }
    }

    public override void Exit()
    {
        //Debug.Log("Saliendo de Patrol State");
    }
}





































