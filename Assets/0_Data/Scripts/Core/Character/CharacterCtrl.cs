
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCtrl : MonoBehaviour
{
    #region Child Component Action

    public Move move { get; private set; }
    public Attack attack { get; private set; }
    public Jump jump { get; private set; }
    public Idle idle { get; private set; }
    public GroundDetect groundDetect { get; private set; }

    #endregion

    #region  State
    public MainMoveState moveState { get; private set; }
    public MainAttackState attackState { get; private set; }
    public MainIdelState idelState { get; private set; }
    public MainJumpState jumpState { get; private set; }
    private StateMachine stateMachine;

    #endregion
    
    private void Awake()
    {
        idle = GetComponentInChildren<Idle>();
        move = GetComponentInChildren<Move>();
        attack = GetComponentInChildren<Attack>();
        jump = GetComponentInChildren<Jump>();
        groundDetect = GetComponentInChildren<GroundDetect>();
        
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
    }

    private void FixedUpdate()
    {
        stateMachine.UpdateState();
    }

    
}