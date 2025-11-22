using System.Collections;
using UnityEngine;

public class MainAttackState : MainCharacterState
{
    private AttackBase currentAttack;
    public MainAttackState(StateMachine stateMachine, CharacterCtrl characterCtrl) : base(stateMachine, characterCtrl)
    {
       
    }
    public override void Enter()
    {
        base.Enter();
        //bool isRight = characterCtrl.attackDir.x > 0;
        //currentAttack = characterCtrl.attack.GetRandomAttack(isRight);
        //currentAttack.AttackHandle(characterCtrl.attackDir);

        //characterCtrl.attack.HandleAttack(characterCtrl.attackDir);

    }
    public override void Update()
    {
        base.Update();
        characterCtrl.attack.HandleAttack(characterCtrl.attackDir);
        if (characterCtrl.attack.isAttacking == false && characterCtrl.attack.currentAttackData != null)
        {
            stateMachine.ChangeState(characterCtrl.idelState);
        }
    }
    public override void Exit()
    {
        base.Exit();
        characterCtrl.attack.StopAttack();
        //currentAttack.AttackEnd();
    }
    private void Attacking()
    {
        Debug.Log("Attack");
        
    }
    private void EndAttack()
    {
        stateMachine.ChangeState(characterCtrl.idelState);
    }
    
}