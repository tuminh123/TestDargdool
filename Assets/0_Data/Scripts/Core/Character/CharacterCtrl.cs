
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCtrl : CharacterParent
{
    public static CharacterCtrl Instance { get; private set; }
    
    public Jump jump {  get; private set; } 
    public MoveVer2 moveVer2 { get; private set; }

    #region  State
    public MainMoveState moveState { get; private set; }
    public MainAttackState attackState { get; private set; }
    public MainIdelState idelState { get; private set; }
    public MainJumpState jumpState { get; private set; }
    public MainStunState stunnedState { get;private set; }

    #endregion

    [SerializeField] SwipeManager swipeManager;
    [SerializeField] GameObject gameOverPanel;

    public bool isAttackPress { get; private set; } = false;
    public bool isJumpPress { get; private set; } = false ;
    public bool isMoving { get; private set; } = false;
    public Vector2 moveDir { get; private set; }

    public void SetIsAttackingPress(bool isAttackingPress) => this.isAttackPress = isAttackPress;
    public void SetIsJumpPress(bool isJumpPress)=> this.isJumpPress = isJumpPress;
    public void SetIsMovingInput(bool isMoving)=> this.isMoving  = isMoving;
    public void SetMoveDir(Vector2 moveDir)=> this.moveDir = moveDir;

    protected override void Awake()
    {
        base.Awake();
        Instance = this;
        gameOverPanel.SetActive(false);

        jump = GetComponentInChildren<Jump>();
        moveVer2 = GetComponentInChildren<MoveVer2>();
        
        //state init
        //stateMachine = new StateMachine();
        moveState = new MainMoveState(stateMachine, this);
        idelState = new MainIdelState(stateMachine, this);
        attackState = new MainAttackState(stateMachine, this);
        jumpState = new MainJumpState(stateMachine, this);
        stunnedState = new MainStunState(stateMachine, this);
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

    public void SetAttackDirection(Vector2 pos)
    {
        Vector3 tapWorldPos = Camera.main.ScreenToWorldPoint(pos);
        tapWorldPos.z = 0;
        attackDir = (tapWorldPos - transform.position).normalized;
    }
    public override void OnDead()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0;
    }

    protected override Vector2 GetKnockDir()
    {
        return Vector2.up;
    }
}