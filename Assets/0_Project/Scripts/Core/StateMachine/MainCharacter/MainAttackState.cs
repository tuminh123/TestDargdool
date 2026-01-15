using Cysharp.Threading.Tasks;
using System.Collections;
using UnityEngine;

public class MainAttackState : MainCharacterState
{
    private IPostAction postAction;

    public MainAttackState(StateMachine stateMachine, CharacterCtrl characterCtrl) : base(stateMachine, characterCtrl)
    {
        postAction = new SmoothPostAction(characterCtrl?.ragdollController?.ActionsDataSO, characterCtrl?.ragdollController?.Balances, characterCtrl?.attack?.ConfigSO);
    }

    public override void Enter()
    {

        base.Enter();

        string name = characterCtrl.AttackDir.x > 0 ? StringConst.RIGHT_PUNCH : StringConst.LEFT_PUNCH;
        characterCtrl.attackContext.EnableAttack();
        characterCtrl?.ragdollController?.postContext.SetPostAction(postAction);

        characterCtrl?.attack?.attackContext?.ExecuteAttack(characterCtrl.AttackDir,characterCtrl?.ragdollController?.actionBase,name,characterCtrl.gameObject);
        characterCtrl.SendDamageBase();

        characterCtrl.attack.currentAttack.OnAttackEnd += EndAttack;

    }
    public override void Exit()
    {
        base.Exit();

        characterCtrl.attackContext.DisableAttack();
        characterCtrl.attack.currentAttack.OnAttackEnd -= EndAttack;

    }

    private void EndAttack()
    {
        //Debug.Log("End");
        stateMachine.ChangeState(characterCtrl.idelState);
    }

}