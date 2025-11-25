using UnityEngine;

public class EnemyDeathState : EnemyState
{
    public EnemyDeathState(EnemyStateMachine enemy) : base(enemy) { }

    public override void Enter()
    {
        // Desaparece inmediatamente
        Object.Destroy(enemy.gameObject);
    }

    public override void Update() { }

    public override void Exit() { }
}

