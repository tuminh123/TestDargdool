
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
    [SerializeField] protected ParticleSystem dieParticle;

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
         dieParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
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
       /* GameState currentGameState = SingletonManager.Instance.gameManager.CurrentState;
        if (currentGameState != GameState.PLAY) return;*/
        stateMachine.UpdatePhysicState();
    }
    private void HandleProperties()
    {
        CharacterCtrl characterCtrl = CharacterCtrl.Instance;
        if (characterCtrl == null || characterCtrl.healthBase.IsDead) return;
        Transform player = characterCtrl.transform;

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
        CharacterCtrl characterCtrl = CharacterCtrl.Instance;
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