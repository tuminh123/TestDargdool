
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
    public StateMachine stateMachine { get;private set; }

    #endregion

    [SerializeField] SwipeManager swipeManager;
    [SerializeField] GameObject gameOverPanel;

    // Input
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
        stateMachine = new StateMachine();
        moveState = new MainMoveState(stateMachine, this);
        idelState = new MainIdelState(stateMachine, this);
        attackState = new MainAttackState(stateMachine, this);
        jumpState = new MainJumpState(stateMachine, this);
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
        AttackDirHandle();
    }
    private void FixedUpdate()
    {
        stateMachine.UpdatePhysicState();
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        stateMachine.ExitState();

    }
    private void OnDestroy()
    {
        stateMachine.ExitState();
    }

    public void SetAttackDirection(Vector2 attackDir)
    {
        this.attackDir = attackDir;
    }
    private void AttackDirHandle()
    {
        // Nếu không có tap → không làm gì
        //if (!SwipeManager.tap) return;
        //Vector3 worldPos = Camera.main.ScreenToWorldPoint(SwipeManager.TapPosition);
        //worldPos.z = bodyParent.transform.position.z;
        //attackDir = (worldPos - bodyParent.transform.position).normalized;
    }

    public override void OnDead()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0;
    }

    protected override Vector2 GetKnockDir()
    {
      
        Vector2 dir;
        if(move.IsMovingRight) 
            dir = Vector2.left;
        else if(move.IsMovingLeft) 
            dir = Vector2.right;
        else 
            dir = Vector2.up;
        Debug.Log(dir);
        return dir;
    }
}