using System.Collections;
using UnityEngine;

public class MainIdelState : MainCharacterState
{
    public MainIdelState(StateMachine stateMachine, CharacterCtrl characterCtrl) : base(stateMachine, characterCtrl)
    {
    }
    public override void Update()
    {
        base.Update();
        characterCtrl.idle.IdelHandle();
        
        if(x!=0 && isGround)stateMachine.ChangeState(characterCtrl.moveState);
    }

   
}