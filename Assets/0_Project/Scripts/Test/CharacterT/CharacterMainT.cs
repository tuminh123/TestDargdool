using UnityEngine;

public class CharacterMainT : IState
{
    protected CharacterT characterT;
    protected StateMachine stateMachine;
    protected float x;

    public CharacterMainT(CharacterT characterT, StateMachine stateMachine)
    {
        this.characterT = characterT;
        this.stateMachine = stateMachine;
    }

    public virtual void Enter()
    {
        SwipeManager.OnTap += HandleTap;
    }

    public virtual void Exit()
    {
        SwipeManager.OnTap -= HandleTap;
    }
    private void HandleTap(Vector2 pos)
    {

        characterT.SetAttackDirection(pos);

        stateMachine.ChangeState(characterT.attackT);

    }
    public virtual void Update()
    {
        x = SwipeManagerTest.MoveDirection;
    }

    public virtual void UpdatePhysic()
    {
        
    }
}
