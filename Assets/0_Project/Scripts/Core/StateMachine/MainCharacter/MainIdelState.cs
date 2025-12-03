using System.Collections;
using UnityEngine;

public class MainIdelState : MainCharacterState
{
    public MainIdelState(StateMachine stateMachine, CharacterCtrl characterCtrl) : base(stateMachine, characterCtrl)
    {
    }
    public override void Enter()
    {
        base.Enter();
        characterCtrl.idle.IdelHandle();
    }
    public override void Update()
    {
        base.Update();
       
        
        if(x!=0 && isGround )stateMachine.ChangeState(characterCtrl.moveState);
    }

   
}