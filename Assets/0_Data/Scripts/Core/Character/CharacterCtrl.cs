
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCtrl : MonoBehaviour
{
    public static CharacterCtrl Instance { get; private set; }

    #region Child Component Action

    public Move move { get; private set; }
    public Attack attack { get; private set; }
    public Jump jump { get; private set; }
    public Idle idle { get; private set; }
    public GroundDetect groundDetect { get; private set; }
    public DamageDetect[] damageDetect { get; private set; }

    #endregion

    #region  State
    public MainMoveState moveState { get; private set; }
    public MainAttackState attackState { get; private set; }
    public MainIdelState idelState { get; private set; }
    public MainJumpState jumpState { get; private set; }
    private StateMachine stateMachine;

    #endregion

    [SerializeField] Balance body;
    [SerializeField] GameObject gameOverPanel;
    public HealthBase healthBase { get; private set; }
    public Vector2 attackDir {  get; private set; }
    public Balance Body => body;

    private void Awake()
    {
        Instance = this;

        gameOverPanel.SetActive(false);
        

        idle = GetComponentInChildren<Idle>();
        move = GetComponentInChildren<Move>();
        attack = GetComponentInChildren<Attack>();
        jump = GetComponentInChildren<Jump>();
        groundDetect = GetComponentInChildren<GroundDetect>();
        healthBase = GetComponent<HealthBase>();
        damageDetect = GetComponentsInChildren<DamageDetect>();
        
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
        AttackDirHandle();
    }
    private void OnEnable()
    {
        if (healthBase == null) return;
        healthBase.OnDead += OnCharacterDead;
    }
    private void OnDisable()
    {
        if (healthBase == null) return;
        healthBase.OnDead -= OnCharacterDead;
    }

    private void OnCharacterDead()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0;
    }


    private void AttackDirHandle()
    {
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0;
        attackDir = (mouseWorld - body.transform.position).normalized;
    }

    public void SendDamage()
    {
        foreach (var item in damageDetect)
        {
            if (item == null) continue;
            item.SenderDamageTo();
        }
    }
}