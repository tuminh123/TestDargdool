
using Core;
using System;
using System.Collections;
using UnityEngine;
using Zenject;

public abstract class EnemyAI : CharacterParent
{
    [InjectOptional]
    private ItemPoolManager itemPoolManager;

    [SerializeField] protected float maxAttackDistance = 3;
    //Time change state
    [SerializeField] protected float attackDuration = 1;
    [SerializeField] protected float stunnedDuration = 4;
    [SerializeField] protected float dieDuration = 3;

    [SerializeField] protected ParticleSystem dieParticle;

    [SerializeField] private Transform headModel;

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
        dieParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }
    private void Update()
    {
        HandleProperties();
        HeadRotation();
        stateMachine.UpdateState();
    }
    private void FixedUpdate()
    {
        stateMachine.UpdatePhysicState();
    }
    private void HeadRotation()
    {
        if (characterCtrl == null || characterCtrl.healthBase.IsDead) return;
        Transform player = characterCtrl.transform;

        if(player == null) return;
        Vector2 dir = -transform.position + player.position;

        if (headModel == null) return;
        if (dir.x > 0) headModel.transform.localScale = new Vector3(1, 1, 1);
        else if (dir.x < 0) headModel.transform.localScale = new Vector3(-1, 1, 1);
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
        //Destroy(bodyParent.gameObject);
        GameEventBus.RaiseEnemyDead();

        int goldCount = 3;
        if (SingletonManager.Instance == null || SingletonManager.Instance.goldManager == null) return;
        SingletonManager.Instance.goldManager.AddGold(goldCount);

        Global.Send(new SignalGoldReceived { receivedGoldCount = goldCount} );
        Global.Send(new SignalEnemyDie { enemyDieCount = 1} );

        /* for (int i = 0; i < 3; i++)
         {
             ZenManager.Instance.itemPoolManager.SpawnGoldItem(transform.position);
         }*/

    }
    protected override Vector2 GetKnockDir()
    {
        if (characterCtrl == null || characterCtrl.healthBase.IsDead) return Vector2.zero;
        Transform player = characterCtrl.transform;

        return transform.position - player.position;
    }  
    public void EnemyDieHandle()
    {
        DisableBalance();
        
        SetKnockBackBalance();

        ragdollController?.Explode();

        StartCoroutine(SetDieParticle());

        if (LevelManager.Instance == null) return;
        LevelManager.Instance.AddExp(100);
    }
    private IEnumerator SetDieParticle()
    {
        dieParticle.Play();
        yield return new WaitForSeconds(3);
        dieParticle.Clear();
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        StopCoroutine(SetDieParticle());
    }

    /*public void SendDamageBase()
    {
        Global.Send(new SignalSendDamage
        {
            damaged = DamageCaculate()
        });
    }
    private float DamageCaculate()
    {
        float baseDamage = stats.DamageBase;

        bool isCrit = UnityEngine.Random.value < stats.CritChane;

        if (isCrit)
        {
            baseDamage *= stats.CritMultiplier;
        }

        return baseDamage;
    }*/
}