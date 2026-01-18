public class EnemyBasic : EnemyAI
{
    #region State
    public EnemyDieState enemyDieState { get; private set; }
    public EnemyStunState enemyStunState { get; private set; }
    public EnemyChaseState enemyChaseState { get; private set; }
    public EnemyAttackState enemyAttackState { get; private set; }
    public EnemyCombatState enemyCombatState { get; private set; }
    public EnemyIdleState enemyIdleState { get; private set; }
    public EnemyJumpState enemyJumpState { get; private set; }
    public EnemyBreakBoxState enemyBreakBoxState { get; private set; }
    public EnemyBaseWeaponAttack enemyBaseWeaponAttack { get; private set; }
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
        enemyJumpState = new EnemyJumpState(stateMachine, this);
        enemyBreakBoxState  = new EnemyBreakBoxState(stateMachine, this);
        enemyBaseWeaponAttack = new EnemyBaseWeaponAttack(stateMachine, this);
    }
    protected override void Start()
    {
        base.Start();
        stateMachine.InitState(enemyIdleState);
    }
    protected override void EnemyAI_OnBoxDetect(Box obj)
    {
        base.EnemyAI_OnBoxDetect(obj);
        stateMachine.ChangeState(enemyBreakBoxState);
    }
}
