
using UnityEngine;

public class MainJumpState :MainCharacterState
{
    public MainJumpState(StateMachine stateMachine, CharacterCtrl characterCtrl) : base(stateMachine, characterCtrl)
    {
    }
    public override void Enter()
    {
        base.Enter();
        characterCtrl.jump.JumpHandle(0);
    }

    public override void Update()
    {
        base.Update();
        if (isGround)
        {
            stateMachine.ChangeState(characterCtrl.idelState);
        }
    }


}