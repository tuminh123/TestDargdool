
using Core;

using System.Collections;
using UnityEngine;
using UnityEngine.Profiling;
using Zenject;

public abstract class EnemyAI : CharacterParent
{


    [SerializeField] protected float maxAttackDistance = 3;
    //Time change state
    [SerializeField] protected float attackDuration = 1;
    [SerializeField] protected float stunnedDuration = 4;
    [SerializeField] protected float dieDuration = 3;

    [SerializeField] protected BoxDetect boxDetect;

    public CharacterCtrl characterCtrl { get;private set; }
    public PlayerDetect playerDetect { get; private set; }
    public float disBetweenEnemyAndPlayer { get; private set; }

    //get
    public float MaxAttackDistance => maxAttackDistance;
    protected override void Awake()
    {
        base.Awake();
        healthBase.SetMaxHealth(stats.MaxHealth);
        playerDetect = GetComponentInChildren<PlayerDetect>();
        //state init
    }

    protected override void Start()
    {
        base.Start();
        characterCtrl = CharacterCtrl.Instance;

        boxDetect.OnBoxDetect += EnemyAI_OnBoxDetect;
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();

        boxDetect.OnBoxDetect -= EnemyAI_OnBoxDetect;
    }

    protected virtual void EnemyAI_OnBoxDetect(Box obj)
    {
        if (obj == null) return;
        attackDir = obj.transform.position - transform.position;
    }

    private void Update()
    {
        HandleProperties();

        stateMachine.UpdateState();
    }
    private void FixedUpdate()
    {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        Profiler.BeginSample("Limit moving");
#endif
        move?.LimitMoving
        (
            ragdollController?.actionBase?.GetBalance(BalanceType.body_up).Rb,
            ragdollController?.actionBase?.GetBalance(BalanceType.right_leg).Rb,
            ragdollController?.actionBase?.GetBalance(BalanceType.left_leg).Rb
        );
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        Profiler.EndSample();
#endif
        stateMachine.UpdatePhysicState();
    }

    private void HandleProperties()
    {
        if (characterCtrl == null || characterCtrl.healthBase.IsDead) return;
        Transform player = characterCtrl.transform;

        attackDir = (player.position - transform.position).normalized;
        disBetweenEnemyAndPlayer = Vector2.Distance(transform.position, player.position);
    }
    public override void OnDead()
    {
        Destroy(gameObject);

        GameEventBus.RaiseEnemyDead();

        int goldCount = 3;
        if (SingletonManager.Instance == null || SingletonManager.Instance.goldManager == null) return;
        SingletonManager.Instance.goldManager.AddGold(goldCount);

        Global.Send(new SignalGoldReceived { receivedGoldCount = goldCount} );
        Global.Send(new SignalEnemyDie { enemyDieCount = 1} );

    }
    public override Vector2 GetKnockDir()
    {
        if (characterCtrl == null || characterCtrl.healthBase.IsDead) return Vector2.zero;
        Transform player = characterCtrl.transform;

        return transform.position - player.position;
    }  
    public void EnemyDieHandle()
    {
        VfxBase vfx = null;
        ZenManager.Instance?.vfxPoolManager?.SpawnVfx(StringConst.DIEVFX, gameObject, out vfx);

        Vector2 dir = Random.value > 0.5f ? Vector2.right : Vector2.left;
        weaponEquip?.DropWeapon(dir);
        if (ragdollController != null)
        {
            ragdollController.DisableRagdoll();
            ragdollController.KnockBackCharacter(GetKnockDir());
        }
        else return;
        //DisableBalance();
        
        //SetKnockBackBalance();

        ragdollController?.Explode();

        if (LevelManager.Instance == null) return;
        LevelManager.Instance.AddExp(10);
    }
}