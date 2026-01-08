using UnityEngine;

public class CharacterIdleT : CharacterMainT
{
    public CharacterIdleT(CharacterT characterT, StateMachine stateMachine) : base(characterT, stateMachine)
    {
    }
    public override void Enter()
    {
        base.Enter();
        characterT.idling.IdelHandle();
    }
    public override void Update()
    {
        base.Update();
        if(x != 0)
        {
            stateMachine.ChangeState(characterT.moveT);
        }
    }
}
