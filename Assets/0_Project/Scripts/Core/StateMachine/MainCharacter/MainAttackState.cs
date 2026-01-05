using Cysharp.Threading.Tasks;
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
        characterCtrl.attackContext.EnableAttack();
        characterCtrl.attackContext.EnableAttackPhysics(characterCtrl.AttackDir);
        characterCtrl.attack.HandleAttack(characterCtrl.AttackDir).Forget();

        characterCtrl.SendDamageBase();

        if (characterCtrl.attack.currentAttackData == null) return;

        characterCtrl.attack.currentAttackData.OnAttacking += Attacking;
        characterCtrl.attack.currentAttackData.OnEndAttack += EndAttack;

        /*  foreach (var pose in characterCtrl.PoseMotors)
              pose.Disable();

          characterCtrl.attack
          .HandleAttack(characterCtrl.AttackDir);
          *//*.Forget();*//*

          characterCtrl.attack.OnAttackEnd += EndAttack;*/

    }
    public override void Exit()
    {
        base.Exit();

        characterCtrl.attackContext.DisableAttack();
        characterCtrl.attackContext.ResetPhysics();

        if (characterCtrl.attack.currentAttackData == null) return;

        characterCtrl.attack.currentAttackData.OnAttacking -= Attacking;
        characterCtrl.attack.currentAttackData.OnEndAttack -= EndAttack;
        /* foreach (var pose in characterCtrl.PoseMotors)
             pose.Enable();

         //characterCtrl.attack.CancelAttack();
         characterCtrl.attack.OnAttackEnd -= EndAttack;*/
    }
    private void Attacking()
    {
        Debug.Log("Attack");
        characterCtrl.SendDamage();
    }
    private void EndAttack()
    {
        //characterCtrl.SendDamage();
        //stateMachine.ChangeState(characterCtrl.idelState);

        //characterCtrl.attack.CancelAttack();
        stateMachine.ChangeState(characterCtrl.idelState);
    }

}