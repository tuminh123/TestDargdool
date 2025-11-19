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
        Debug.Log("begin");
        characterCtrl.attack.HandleAttack();
        characterCtrl.attack.currentAttackData.OnAttackEnd += () =>
        {
            Debug.Log("exit");
            stateMachine.ChangeState(characterCtrl.idelState);
        };

    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log("end");
        characterCtrl.attack.StopAttack();
        
    }
    
}