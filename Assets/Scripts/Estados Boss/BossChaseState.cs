using UnityEngine;

public class BossChaseState : BossState
{
    public BossChaseState(BossStateMachine boss) : base(boss) { }

    public override void Enter()
    {
        // BossWalk se reproducirá al moverse en MoveTowards
    }

    public override void Update()
    {
        if (boss.player == null)
        {
            boss.ChangeState(boss.idleState);
            return;
        }

        if (boss.IsPlayerInAttackRange && boss.CanAttack)
        {
            boss.ChangeState(boss.attackState);
            return;
        }

        if (!boss.IsPlayerDetected)
        {
            boss.ChangeState(boss.idleState);
            return;
        }

        boss.MoveTowards(boss.player.position);
    }

    public override void Exit()
    {
        boss.StopMoving();
    }
}

