using UnityEngine;

public class EnemyBasicAttackState : EnemyBasicState
{
    public EnemyBasicAttackState(StateMachine stateMachine, EnemyAI enemyAI, Transform body) : base(stateMachine, enemyAI, body)
    {
    }

    public override void Enter()
    {
        base.Enter();
        
        enemyAI.attack.HandleAttack(attackDir);
        
        enemyAI.attack.currentAttackData.OnAttackEnd += () =>
        {
            stateMachine.ChangeState(enemyAI.enemyChaseState);
        };
    }

    //public override void Update()
    //{
    //    base.Update();
    //    if (enemyAI.playerDetect.IsPlayer == false)
    //    {
    //        stateMachine.ChangeState(enemyAI.enemyChaseState);
    //    }
    //}

    public override void Exit()
    {
        base.Exit();      
        enemyAI.attack.StopAttack();
        
    }

   
}