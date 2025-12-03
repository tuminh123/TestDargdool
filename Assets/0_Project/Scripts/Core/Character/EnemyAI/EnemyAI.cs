
using System;
using System.Collections;
using UnityEngine;

public abstract class EnemyAI : CharacterParent
{
    [SerializeField] protected float maxAttackDistance = 5;
    //Time change state
    [SerializeField] protected float attackDuration = 3;
    [SerializeField] protected float stunnedDuration = 4;
    [SerializeField] protected float dieDuration = 3;
    [SerializeField] GameObject parent;

    public PlayerDetect playerDetect { get; private set; }

    public float disBetweenEnemyAndPlayer { get; private set; }
    //get
    public float MaxAttackDistance => maxAttackDistance;
    protected override void Awake()
    {
        base.Awake();
        
        playerDetect = GetComponentInChildren<PlayerDetect>();
        //state init
    }

    protected virtual void Start()
    {
         
        //Debug.Log(stats.MaxHealth);
        //Debug.Log(stats.Speed);
        //Debug.Log(stats.DamageBase);
    }
    private void Update()
    {
        HandleProperties();
        stateMachine.UpdateState();
    }
    private void FixedUpdate()
    {
        stateMachine.UpdatePhysicState();
    }
    private void HandleProperties()
    {
        Transform player = CharacterCtrl.Instance.transform;

        attackDir = (player.position - bodyParent.transform.position).normalized;
        disBetweenEnemyAndPlayer = Vector2.Distance(bodyParent.transform.position, player.position);
    }
    public override void OnDead()
    {
        Destroy(parent);
        //Destroy(bodyParent.gameObject);
        Gold gold = SingletonManager.Instance.objInGamePoolManager.Spawn(StringConst.GOLD, transform.position,Quaternion.identity) as Gold;
        gold.SetVelocity();

    }
    protected override Vector2 GetKnockDir()
    {
        return transform.position - CharacterCtrl.Instance.transform.position;
    }  
    public void EnemyDieHandle()
    {
        SetTriggerBalance(false);
        SetKnockBackBalance();
        SingletonManager.Instance.vfxPoolManager.Spawn(StringConst.DIEVFX, transform.position, Quaternion.identity);
    }
}