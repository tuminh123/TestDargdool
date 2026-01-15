using UnityEngine;

public class EnemyBreakBoxState : EnemyBaseState
{
    private IPostAction postAction;
    public EnemyBreakBoxState(StateMachine stateMachine, EnemyBasic enemyBasic) : base(stateMachine, enemyBasic)
    {
        postAction = new SmoothPostAction(enemyBasic?.ragdollController?.ActionsDataSO, enemyBasic?.ragdollController?.Balances, enemyBasic?.attack?.ConfigSO);
    }
    public override void Enter()
    {
        base.Enter();
        string name = enemyBasic.AttackDir.x < 0 ? "LeftKick" : "RightKick";
        enemyBasic.attackContext.EnableAttack();
        enemyBasic?.ragdollController?.postContext.SetPostAction(postAction);

        enemyBasic?.attack?.attackContext?.ExecuteAttack(enemyBasic.AttackDir, enemyBasic.ragdollController.actionBase, name, enemyBasic.gameObject);
        enemyBasic.SendDamageBase();

        enemyBasic.attack.currentAttack.OnAttackEnd += EndAttack;
    }
    public override void Exit()
    {
        base.Exit();

        enemyBasic.attackContext.DisableAttack();
        enemyBasic.attack.currentAttack.OnAttackEnd -= EndAttack;
    }

    private void EndAttack()
    {
        /*if (enemyBasic.IsBoxDetect)
        {
            stateMachine.ChangeState(enemyBasic.enemyIdleState);
        }*/

        stateMachine.ChangeState(enemyBasic.enemyIdleState);
    }

}
