using UnityEngine;

public abstract class EnemyState
{
    protected EnemyStateMachine enemy;

    public EnemyState(EnemyStateMachine enemy)
    {
        this.enemy = enemy;
    }

    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();
}


