using System.Collections;
using UnityEngine;

public class MainAttackState : MainCharacterState
{
    public MainAttackState(StateMachine stateMachine, CharacterCtrl characterCtrl) : base(stateMachine, characterCtrl)
    {
       
    }
    public override void Enter()
    {
        base.Enter();
        characterCtrl.attack.HandleAttack(characterCtrl.AttackDir);

        if (characterCtrl.attack.currentAttackData == null) return;

        characterCtrl.attack.currentAttackData.OnAttacking += Attacking;
        characterCtrl.attack.currentAttackData.OnAttackEnd += EndAttack;

        

    }
    public override void Exit()
    {
        base.Exit();

        characterCtrl.attack.StopAttack();

        if (characterCtrl.attack.currentAttackData == null) return;

        characterCtrl.attack.currentAttackData.OnAttacking -= Attacking;
        characterCtrl.attack.currentAttackData.OnAttackEnd -= EndAttack;

    }
    private void Attacking()
    {
        //Debug.Log("Attack");
        //characterCtrl.SendDamage();
        /* if (ZenManager.Instance.cameraShaker == null || ZenManager.Instance == null) return;
         ZenManager.Instance.cameraShaker.ShakeCam();*/
        characterCtrl.SendDamage();
        stateMachine.ChangeState(characterCtrl.idelState);
    }
    private void EndAttack()
    {
        //characterCtrl.SendDamage();
        //stateMachine.ChangeState(characterCtrl.idelState);
    }
    
}