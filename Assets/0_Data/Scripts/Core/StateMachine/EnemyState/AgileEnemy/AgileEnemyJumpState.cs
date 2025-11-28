public class AgileEnemyJumpState : AgileEnemyState
{
    private float jumpTime;
    private float jumpDuration;
    public AgileEnemyJumpState(AgileEnemy agileEnemy, StateMachine stateMachine) : base(agileEnemy, stateMachine)
    {
    }
    public override void Enter()
    {
        base.Enter();
        float x = -agileEnemy.AttackDir.x;
        agileEnemy.jump.JumpHandle(x);
    }
    public override void Update()
    {
        base.Update();
        
        if (isGround)
        {
            stateMachine.ChangeState(agileEnemy.agileEnemyShootState);
        }
    }
}