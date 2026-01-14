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
        //characterCtrl?.ragdollController?.actionBase.SetPostAction(new ActionPostNormal());

    }
    public override void Update()
    {
        base.Update();

        characterCtrl.FlipSystem(x,characterCtrl.Head);

        if(x==0) stateMachine.ChangeState(characterCtrl.idelState);
    }
    public override void UpdatePhysic()
    {
        base.UpdatePhysic();
        characterCtrl.move.MoveHandle(x,characterCtrl.ragdollController.actionBase);
    }

    public override void Exit()
    {
        base.Exit();
        characterCtrl.move.StopMoveCoroutine();
    }
}