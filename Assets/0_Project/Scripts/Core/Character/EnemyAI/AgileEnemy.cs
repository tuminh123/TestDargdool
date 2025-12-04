using System.Collections;
using UnityEngine;

public class AgileEnemy : EnemyAI
{
    #region State
    public AgileEnemyChaseState agileEnemyChaseState { get; private set; }
    public AgileEnemyAttackState agileEnemyAttackState { get; private set; }
    public AgileEnemyCombatState agileEnemyCombatState { get; private set; }
    public AgileEnemyJumpState agileEnemyJumpState { get; private set ; }
    public AgileEnemyStunnedState agileEnemyStunnedState { get;private set; }
    public AgileEnemyShootState agileEnemyShootState { get; private set; }
    public AgileEnemyDeadState agileEnemyDeadState { get; private set; }
    public AgileEnemyIdleState agileEnemyIdleState { get; private set; }
    #endregion
    public GameObject dagger;
    [SerializeField] private Transform shootPoint;

    [SerializeField] private Balance right_up_arm;
    [SerializeField] private Balance right_down_arm;
    [SerializeField] private Balance right_hand;
    public Jump jump { get; private set; }

    [SerializeField] float timeComebackCombat = 5;

    protected override void Awake()
    {
        base.Awake();
        agileEnemyAttackState = new AgileEnemyAttackState(this, stateMachine);
        agileEnemyChaseState = new AgileEnemyChaseState(this, stateMachine);
        agileEnemyCombatState = new AgileEnemyCombatState(this, stateMachine,attackDuration);
        agileEnemyJumpState = new AgileEnemyJumpState(this, stateMachine);
        agileEnemyStunnedState = new AgileEnemyStunnedState(this, stateMachine,stunnedDuration);
        agileEnemyAttackState = new AgileEnemyAttackState(this, stateMachine);
        agileEnemyShootState = new AgileEnemyShootState(this, stateMachine,timeComebackCombat);
        agileEnemyDeadState = new AgileEnemyDeadState(this,stateMachine,dieDuration);
        agileEnemyIdleState = new AgileEnemyIdleState(this, stateMachine);

        jump = GetComponentInChildren<Jump>();
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.InitState(agileEnemyChaseState);
    }

    public void BeginShoot()
    {
        StartCoroutine(ShootBullet());
    }

    public void StopShoot()
    {
        StopCoroutine(ShootBullet());
    }



    private IEnumerator ShootBullet()
    {
        if (attackDir.x > 0)
        {
            right_up_arm.SetRotation(115);
            right_down_arm.SetRotation(110);
            right_hand.SetRotation(105);
        }
        else if (attackDir.x < 0)
        {
            right_up_arm.SetRotation(-115);
            right_down_arm.SetRotation(-110);
            right_hand.SetRotation(-105);
        }
        yield return new WaitForSeconds(1.4f);

        DaggerHandle();

        Debug.Log("Shoot");

        yield return new WaitForSeconds(3f);

        right_up_arm.ResetData();
        right_down_arm.ResetData();
        right_hand.ResetData();

    }

    private void DaggerHandle()
    {
        Dagger daggerClone = SingletonManager.Instance.objInGamePoolManager.Spawn(StringConst.DAGGER, shootPoint.position, Quaternion.identity) as Dagger;
        if (daggerClone == null) return;

        if(attackDir.x > 0) 
            daggerClone.SetDaggerAction(Vector2.right,0);

        else if(attackDir.x < 0)
            daggerClone.SetDaggerAction(Vector2.left,180);
    }
}
