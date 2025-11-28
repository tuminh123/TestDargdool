using UnityEngine;

public class DamagePartEnemyDefenseState : DamagePartEnemyState
{
    private float defenseTime;
    private float defenseDuration;

    public DamagePartEnemyDefenseState(DamagePartEnemy partEnemy, StateMachine stateMachine, float defenseDuration) : base(partEnemy, stateMachine)
    {
        this.defenseDuration = defenseDuration;
    }

    public override void Enter()
    {
        base.Enter();
        defenseTime = defenseDuration;

        partEnemy.BeginDefense();
    }
    public override void Update()
    {
        base.Update();
        defenseTime -= Time.deltaTime;

        if (defenseTime < 0)
        {
            stateMachine.ChangeState(partEnemy.damagePartEnemyCombatState);
        }
        
    }
    public override void Exit()
    {
        base.Exit();

        partEnemy.StopDefense();
    }
}