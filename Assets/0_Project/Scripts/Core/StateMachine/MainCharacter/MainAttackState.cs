using Cysharp.Threading.Tasks;
using System.Collections;
using UnityEngine;

public class MainAttackState : MainCharacterState
{
    protected string name;
    public MainAttackState(StateMachine stateMachine, CharacterCtrl characterCtrl) : base(stateMachine, characterCtrl)
    {
       
    }
    public override void Enter()
    {
        base.Enter();

        string name = characterCtrl.AttackDir.x > 0 ? "RightPunch" : "LeftPunch";
        this.name = name;

        //characterCtrl.ragdollController.actionBase.InitBalancesOfActionDataSO(name);
        characterCtrl.attackContext.EnableAttack();
        

        //string name = characterCtrl.AttackDir.x > 0 ? "PhysicRightPunch" : "PhysicLeftPunch";
        // Init
        /* characterCtrl?.ragdollController?.actionBase?.DisableBalance(name);
         characterCtrl.InitAttack(name);*/
        characterCtrl?.attack?.Init(characterCtrl.ragdollController.actionBase, name, characterCtrl.gameObject);
        // Action
        characterCtrl?.attack?.ExecuteAttack(characterCtrl.AttackDir);
        characterCtrl.SendDamageBase();

        //characterCtrl.attack.attackSystem.OnAttackEnd += EndAttack;
        characterCtrl.attack.currentAttackData.OnEndAttack += EndAttack;

    }
    public override void Exit()
    {
        base.Exit();

        characterCtrl.attackContext.DisableAttack();
       //characterCtrl?.ragdollController?.actionBase?.EnableBalance(name);
        //characterCtrl.attack.attackSystem.OnAttackEnd -= EndAttack;
        characterCtrl.attack.currentAttackData.OnEndAttack -= EndAttack;

    }

    private void EndAttack()
    {
        Debug.Log("End");

        //ragdollController.actionBase.ClearBalancesOfActionDataSO();
        stateMachine.ChangeState(characterCtrl.idelState);
    }

}