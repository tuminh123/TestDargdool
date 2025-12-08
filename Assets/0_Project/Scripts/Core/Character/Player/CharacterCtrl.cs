
using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCtrl : CharacterParent
{
    public static CharacterCtrl Instance { get; private set; }
    
    public Jump jump {  get; private set; } 
    public DetectionZone zone { get; private set; }
    public PlayerWeaponEquip weaponEquip { get; private set; }

    #region  State
    public MainMoveState moveState { get; private set; }
    public MainAttackState attackState { get; private set; }
    public MainIdelState idelState { get; private set; }
    public MainJumpState jumpState { get; private set; }
    public MainStunState stunnedState { get;private set; }
    public MainWeaponAttackState weaponAttackState { get; private set; }

    #endregion

    
    protected override void Awake()
    {
        base.Awake();
        Instance = this;

        InitPlayerData();

        jump = GetComponentInChildren<Jump>();
        zone = GetComponentInChildren<DetectionZone>();
        weaponEquip = GetComponentInChildren<PlayerWeaponEquip>();
        //state init
        //stateMachine = new StateMachine();
        moveState = new MainMoveState(stateMachine, this);
        idelState = new MainIdelState(stateMachine, this);
        attackState = new MainAttackState(stateMachine, this);
        jumpState = new MainJumpState(stateMachine, this);
        stunnedState = new MainStunState(stateMachine, this);
        weaponAttackState = new MainWeaponAttackState(stateMachine, this);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        healthBase.OnDead += OnDead;
    }

    private void Start()
    {
        stateMachine.InitState(idelState);
        //Debug.Log(stats.MaxHealth);
        //Debug.Log(stats.Speed);
        //Debug.Log(stats.DamageBase);
    }
    private void Update()
    {
        stateMachine.UpdateState();
    }
    private void FixedUpdate()
    {
        stateMachine.UpdatePhysicState();
    }
    protected override void OnDisable()
    {
        base.OnDisable();

        healthBase.OnDead -= OnDead;
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        healthBase.OnDead -= OnDead;
    }
    #region Stats setup
    private void InitPlayerData()
    {
        PlayerData data = SingletonManager.Instance.dataManager.Data;
        float maxHP = data.maxHp;
        float finalDamage = data.CalculateDamage();
        stats.SetMaxHealth(maxHP);
        stats.SetDamageBase(finalDamage);
    }
    #endregion

    public void SetAttackDirection(Vector2 pos)
    {
        Vector3 tapWorldPos = Camera.main.ScreenToWorldPoint(pos);
        tapWorldPos.z = 0;
        attackDir = (tapWorldPos - transform.position).normalized;
    }
    public override void OnDead()
    {
        SingletonManager.Instance.gameManager.SetState(GameState.LOSE);
        SetKnockBackBalance();
        SetTriggerBalance(false);
        //OnDeadWait().Forget();
    }

    protected override Vector2 GetKnockDir()
    {
        if (zone == null) return Vector2.up;
        EnemyAI enemy = zone.GetNearestEnemy(transform);
        if(enemy == null) return Vector2.up;
        return (transform.position - enemy.transform.position).normalized;
    }
}