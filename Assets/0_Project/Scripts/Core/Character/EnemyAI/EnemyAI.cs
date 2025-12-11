
using System;
using System.Collections;
using UnityEngine;
using Zenject;

public abstract class EnemyAI : CharacterParent
{
    [InjectOptional]
    private ItemPoolManager itemPoolManager;

    [SerializeField] protected float maxAttackDistance = 5;
    //Time change state
    [SerializeField] protected float attackDuration = 3;
    [SerializeField] protected float stunnedDuration = 4;
    [SerializeField] protected float dieDuration = 3;
    [SerializeField] GameObject parent;
    [SerializeField] protected ParticleSystem dieParticle;

    public CharacterCtrl characterCtrl { get;private set; }
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
        characterCtrl = CharacterCtrl.Instance;
        dieParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
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
        if (characterCtrl == null || characterCtrl.healthBase.IsDead) return;
        Transform player = characterCtrl.transform;

        attackDir = (player.position - bodyParent.transform.position).normalized;
        disBetweenEnemyAndPlayer = Vector2.Distance(bodyParent.transform.position, player.position);
    }
    public override void OnDead()
    {
        Destroy(parent);
        //Destroy(bodyParent.gameObject);

        ZenManager.Instance. itemPoolManager.SpawnRandomItem(transform.position);

    }
    protected override Vector2 GetKnockDir()
    {
        if (characterCtrl == null || characterCtrl.healthBase.IsDead) return Vector2.zero;
        Transform player = characterCtrl.transform;

        return transform.position - player.position;
    }  
    public void EnemyDieHandle()
    {
        SetTriggerBalance(false);
        SetKnockBackBalance();
        StartCoroutine(SetDieParticle());
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
}