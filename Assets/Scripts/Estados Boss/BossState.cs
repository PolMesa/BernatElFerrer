using UnityEngine;

public abstract class BossState
{
    protected BossStateMachine boss;

    public BossState(BossStateMachine boss)
    {
        this.boss = boss;
    }

    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();
}
