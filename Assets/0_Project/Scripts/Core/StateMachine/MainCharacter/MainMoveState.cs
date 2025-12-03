using System.Collections;
using UnityEngine;


public class MainMoveState : MainCharacterState
{
    public MainMoveState(StateMachine stateMachine, CharacterCtrl characterCtrl) : base(stateMachine, characterCtrl)
    {
    }
    public override void Enter()
    {
        base.Enter();
        //characterCtrl.SetLayerBalance("None");
        //characterCtrl.SetBalanceColSkip(true);
        characterCtrl.DetectCol.enabled = false;
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
        characterCtrl.DetectCol.enabled = true;
        //characterCtrl.SetLayerBalance("Player");
        // characterCtrl.SetBalanceColSkip(false);
    }
}