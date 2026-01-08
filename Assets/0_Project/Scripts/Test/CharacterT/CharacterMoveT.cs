using UnityEngine;

public class CharacterMoveT : CharacterMainT
{
    public CharacterMoveT(CharacterT characterT, StateMachine stateMachine) : base(characterT, stateMachine)
    {
    }
    public override void Update()
    {
        base.Update();
        if(x == 0)
        {
            stateMachine.ChangeState(characterT.idleT);
        }
    }
    public override void UpdatePhysic()
    {
        base.UpdatePhysic();
        characterT.moving.MoveHandle(x);
    }
    public override void Exit()
    {
        base.Exit();
        characterT.moving.StopMoveCoroutine();
    }
}
