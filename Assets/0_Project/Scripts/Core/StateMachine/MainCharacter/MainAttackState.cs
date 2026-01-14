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

        string name = characterCtrl.AttackDir.x > 0 ? StringConst.RIGHT_PUNCH : StringConst.LEFT_PUNCH;

        characterCtrl.attackContext.EnableAttack();
        // Action
        characterCtrl?.attack?.ExecuteAttack(characterCtrl.AttackDir,characterCtrl?.ragdollController?.actionBase,name,characterCtrl.gameObject);
        characterCtrl.SendDamageBase();

        characterCtrl.attack.currentAttack.OnAttackEnd += EndAttack;

    }
    public override void Exit()
    {
        base.Exit();

        characterCtrl.attackContext.DisableAttack();

        characterCtrl?.attack?.CancelAttack();
        characterCtrl.attack.currentAttack.OnAttackEnd -= EndAttack;

    }

    private void EndAttack()
    {
        //Debug.Log("End");
        stateMachine.ChangeState(characterCtrl.idelState);
    }

}