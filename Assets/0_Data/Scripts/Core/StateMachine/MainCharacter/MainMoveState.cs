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
        
        if(x==0) stateMachine.ChangeState(characterCtrl.idelState);
    }
    public override void UpdatePhysic()
    {
        base.UpdatePhysic();
        characterCtrl.move.MoveHandle(x);
    }

    public override void Exit()
    {
        base.Exit();
        characterCtrl.move.StopMoveCoroutine();
    }
}