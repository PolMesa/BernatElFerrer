using UnityEngine;

public class EnemyPatrolState : EnemyState
{
    private float arrivalThreshold = 0.1f; // margen para detectar llegada

    public EnemyPatrolState(EnemyStateMachine enemy) : base(enemy) { }

    public override void Enter() { }

    public override void Update()
    {
        if (enemy.targetPoint == null) return;

        // Movimiento más seguro hacia el punto
        Vector2 newPos = Vector2.MoveTowards(enemy.transform.position,
                                             enemy.targetPoint.position,
                                             enemy.moveSpeed * Time.deltaTime);
        enemy.transform.position = new Vector3(newPos.x, newPos.y, enemy.transform.position.z);

        // Flip del sprite
        float dir = enemy.targetPoint.position.x - enemy.transform.position.x;
        if (dir != 0)
            enemy.transform.localScale = new Vector3(Mathf.Sign(dir), 1, 1);

        // Cambiar de punto al llegar
        float distance = Vector2.Distance(enemy.transform.position, enemy.targetPoint.position);
        if (distance <= arrivalThreshold)
        {
            enemy.SwitchTargetPoint();
        }

        // Detectar jugador
        if (enemy.player != null && Vector2.Distance(enemy.transform.position, enemy.player.position) <= enemy.detectionRange)
        {
            enemy.ChangeState(enemy.chaseState);
        }
    }

    public override void Exit() { }
}




