using System.Collections;
using UnityEngine;


public class MainMoveState : MainCharacterState
{
    public MainMoveState(StateMachine stateMachine, CharacterCtrl characterCtrl) : base(stateMachine, characterCtrl)
    {
    }
    public override void Update()
    {
        base.Update();

        characterCtrl.move.MoveHandle(x);
        
        if(x==0) stateMachine.ChangeState(characterCtrl.idelState);
    }

    public override void Exit()
    {
        base.Exit();
        characterCtrl.move.StopMoveCoroutine();
    }
}