using UnityEngine;

public class EnemyDeathState : EnemyState
{
    private float deathTimer = 0f;
    private float deathDuration = 2f; // Tiempo de animación de muerte

    public EnemyDeathState(EnemyStateMachine enemy) : base(enemy) { }

    public override void Enter()
    {
        Debug.Log("💀 ENEMIGO MUERTO");

        // Detener movimiento
        enemy.StopMoving();

        // Desactivar scripts de IA
        enemy.enabled = false;

        // Reproducir animación de muerte
        enemy.animator.Play("EnemyDeath"); // Ajusta al nombre de tu animación

        // Desactivar barra de vida
        EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
        if (enemyHealth != null && enemyHealth.healthBarCanvas != null)
        {
            enemyHealth.healthBarCanvas.SetActive(false);
        }
    }

    public override void Update()
    {
        deathTimer += Time.deltaTime;

        // Destruir después de la animación
        if (deathTimer >= deathDuration)
        {
            // Ya se destruye desde EnemyHealth
        }
    }

    public override void Exit()
    {
        // No se necesita
    }
}
