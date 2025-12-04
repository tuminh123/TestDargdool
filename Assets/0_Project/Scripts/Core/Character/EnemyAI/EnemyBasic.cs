public class EnemyBasic : EnemyAI
{
    #region State
    public EnemyDieState enemyDieState { get; private set; }
    public EnemyStunState enemyStunState { get; private set; }
    public EnemyChaseState enemyChaseState { get; private set; }
    public EnemyAttackState enemyAttackState { get; private set; }
    public EnemyCombatState enemyCombatState { get; private set; }
    public EnemyIdleState enemyIdleState { get; private set; }

    #endregion

    protected override void Awake()
    {
        base.Awake();
        enemyChaseState = new EnemyChaseState(stateMachine,this);
        enemyAttackState = new EnemyAttackState(stateMachine, this);
        enemyCombatState = new EnemyCombatState(stateMachine,this,attackDuration);
        enemyDieState = new EnemyDieState(stateMachine,this,dieDuration);
        enemyStunState = new EnemyStunState(stateMachine,this,stunnedDuration);
        enemyIdleState = new EnemyIdleState(stateMachine, this);
    }
    protected override void Start()
    {
        base.Start();
        stateMachine.InitState(enemyChaseState);
    }
}
