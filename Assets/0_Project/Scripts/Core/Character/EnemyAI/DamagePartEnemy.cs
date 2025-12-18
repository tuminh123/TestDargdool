using System.Collections;
using UnityEngine;

public class DamagePartEnemy : EnemyAI
{
    #region State
    public DamagePartEnemyCombatState damagePartEnemyCombatState { get; private set; }
    public DamagePartEnemyAttackState damagePartEnemyAttackState { get; private set; }
    public DamagePartEnemyChaseState damagePartEnemyChaseState { get;private set; }
    public DamagePartEnemyDefenseState damagePartEnemyDefenseState { get; private set; }
    public DamagePartEnemyStundState damagePartEnemyStund { get; private set; }
    public DamagePartEnemyDeadState damagePartEnemyDeadState { get;private set;}
    public DamagePartEnemyIdleState damagePartEnemyIdleState { get; private set; }
    public DamagePartEnemyShootState damagePartEnemyShootState { get; private set; }
    #endregion

    [SerializeField] Balance right_up_arm;
    [SerializeField] Balance right_down_arm;
    [SerializeField] Balance right_hand;

    [SerializeField] Balance left_up_arm;
    [SerializeField] Balance left_down_arm;
    [SerializeField] Balance left_hand;

    [SerializeField] private Transform shootPoint;

    [SerializeField] private float defenseDuration = 3;
    [SerializeField] private float shootDuration = 2;

    private DamagePartToPlayer damagePartToPlayer;

    protected override void Awake()
    {
        base.Awake();
        damagePartEnemyAttackState = new DamagePartEnemyAttackState(this, stateMachine);
        damagePartEnemyChaseState = new DamagePartEnemyChaseState(this, stateMachine);
        damagePartEnemyCombatState = new DamagePartEnemyCombatState(this, stateMachine,attackDuration);
        damagePartEnemyDefenseState = new DamagePartEnemyDefenseState(this, stateMachine,defenseDuration);
        damagePartEnemyStund = new DamagePartEnemyStundState(this, stateMachine, stunnedDuration);
        damagePartEnemyDeadState = new DamagePartEnemyDeadState(this, stateMachine, dieDuration);
        damagePartEnemyIdleState = new DamagePartEnemyIdleState(this, stateMachine);
        damagePartEnemyShootState = new DamagePartEnemyShootState(this, stateMachine, shootDuration);

        damagePartToPlayer = GetComponentInChildren<DamagePartToPlayer>();
    }
    protected override void Start()
    {
        base.Start();
        //damagePartToPlayer.gameObject.SetActive(false);

        stateMachine.InitState(damagePartEnemyIdleState);
    }

    #region Shoot
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
        Dagger daggerClone = ZenManager.Instance.objInGamePoolManager.Spawn(StringConst.DAGGER, shootPoint.position, Quaternion.identity) as Dagger;
        if (daggerClone == null) return;

        if (attackDir.x > 0)
            daggerClone.SetDaggerAction(Vector2.right, 0);

        else if (attackDir.x < 0)
            daggerClone.SetDaggerAction(Vector2.left, 180);
    }
    #endregion

    #region Defense
    public void BeginDefense()
    {

        StartCoroutine(DefenseHandle());
        //damagePartToPlayer.DamagePartHandle();
        
    }

    public void StopDefense()
    {
        StopCoroutine(DefenseHandle());
    }

    private IEnumerator DefenseHandle()
    {
        if (attackDir.x > 0)
        {
            right_up_arm.SetRotation(50);
            right_down_arm.SetRotation(200);
            right_hand.SetRotation(150);

            left_up_arm.SetRotation(50);
            left_down_arm.SetRotation(200);
            left_hand.SetRotation(150);
        }
        else if (attackDir.x < 0)
        {
            right_up_arm.SetRotation(-50);
            right_down_arm.SetRotation(-200);
            right_hand.SetRotation(-150);

            left_up_arm.SetRotation(-50);
            left_down_arm.SetRotation(-200);
            left_hand.SetRotation(-150);
        }

        //damagePartToPlayer.gameObject.SetActive(true);
        damagePartToPlayer.SetHealthLayer(CharacterCtrl.Instance.healthBase.gameObject);
        yield return new WaitForSeconds(3f);
        damagePartToPlayer.StopHealthLayer(CharacterCtrl.Instance.healthBase.gameObject);

        //damagePartToPlayer.gameObject.SetActive(false);

        right_up_arm.ResetData();
        right_down_arm.ResetData();
        right_hand.ResetData();
        left_up_arm.ResetData();
        left_down_arm.ResetData();
        left_hand.ResetData();

    }
    #endregion

}
