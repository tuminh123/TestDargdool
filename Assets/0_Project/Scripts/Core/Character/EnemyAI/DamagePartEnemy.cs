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
    #endregion

    [SerializeField] Balance right_up_arm;
    [SerializeField] Balance right_down_arm;
    [SerializeField] Balance right_hand;

    [SerializeField] Balance left_up_arm;
    [SerializeField] Balance left_down_arm;
    [SerializeField] Balance left_hand;

    [SerializeField] private float defenseDuration = 3;

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

        damagePartToPlayer = GetComponentInChildren<DamagePartToPlayer>();
    }
    protected override void Start()
    {
        base.Start();
        //damagePartToPlayer.gameObject.SetActive(false);

        stateMachine.InitState(damagePartEnemyCombatState);
    }
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

}
