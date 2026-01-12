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
      /*  characterCtrl.attackContext.EnableAttack();
        characterCtrl.attackContext.EnableAttackPhysics(characterCtrl.AttackDir);

        string name = characterCtrl.AttackDir.x < 0 ? "LeftPunch" : "RightPunch";

        characterCtrl.attack.HandleAttack(characterCtrl.AttackDir,characterCtrl.ragdollController.actionBase,name).Forget();

        characterCtrl.SendDamageBase();*/

        characterCtrl?.attack?.EnterAttack(characterCtrl, "LeftPunch", "RightPunch");


        if (characterCtrl.attack.currentAttackData == null) return;
       // characterCtrl.attack.currentAttackData.OnAttacking += Attacking;
        characterCtrl.attack.currentAttackData.OnEndAttack += EndAttack;

    }
    public override void Exit()
    {
        base.Exit();
        /*
                characterCtrl.attackContext.DisableAttack();
                characterCtrl.attackContext.ResetPhysics();*/

        characterCtrl?.attack?.ExitAttack(characterCtrl);

        if (characterCtrl.attack.currentAttackData == null) return;
        //characterCtrl.attack.currentAttackData.OnAttacking -= Attacking;
        characterCtrl.attack.currentAttackData.OnEndAttack -= EndAttack;
    }

    private void EndAttack()
    {
        Debug.Log("End");
        stateMachine.ChangeState(characterCtrl.idelState);
    }

}