
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCtrl : CharacterParent
{
    public static CharacterCtrl Instance { get; private set; }
    
    public Jump jump {  get; private set; } 

    #region  State
    public MainMoveState moveState { get; private set; }
    public MainAttackState attackState { get; private set; }
    public MainIdelState idelState { get; private set; }
    public MainJumpState jumpState { get; private set; }
    private StateMachine stateMachine;

    #endregion

    [SerializeField] GameObject gameOverPanel;

    protected override void Awake()
    {
        base.Awake();
        Instance = this;
        gameOverPanel.SetActive(false);

        jump = GetComponentInChildren<Jump>();
        
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

    private void FixedUpdate()
    {
        stateMachine.UpdateState();
        AttackDirHandle();
    }


    private void AttackDirHandle()
    {
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0;
        attackDir = (mouseWorld - bodyParent.transform.position).normalized;
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