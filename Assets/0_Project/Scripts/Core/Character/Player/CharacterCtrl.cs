
using Core;
using Cysharp.Threading.Tasks;
using HadesSDK.Ads.Core;
using Lofelt.NiceVibrations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Profiling;
using Zenject;

public class CharacterCtrl : CharacterParent
{    
    public static CharacterCtrl Instance { get; set; }

    [Space]
    [Header("Character component")]
    [SerializeField] private RegenerationDamageArea damageArea;
    public RegenerationDamageArea DamageArea => damageArea;

    [SerializeField] private Transform head;
    [SerializeField] private LevelStatModifier levelStatModifier;

    [SerializeField] private float stunTime = 4;
    public DetectionZone zone { get; private set; }
    public Vector3 LastPositionBeforeDead { get; private set; }

    #region  State
    public MainMoveState moveState { get; private set; }
    public MainAttackState attackState { get; private set; }
    public MainIdelState idelState { get; private set; }
    public MainJumpState jumpState { get; private set; }
    public MainStunState stunnedState { get;private set; }
    public MainWeaponAttackState weaponAttackState { get; private set; }

    #endregion

    public Transform Head => head;
    public bool IsSendDamage { get; private set; } = false;
    protected override void Awake()
    {
        base.Awake();
        if (Instance != null && Instance != this)
        {
            Debug.LogError("Multiple CharacterCtrl detected!");
            Destroy(gameObject);
            return;
        }
        Instance = this;

        zone = GetComponentInChildren<DetectionZone>();
        damageArea.gameObject.SetActive(false);
        //state init
        //stateMachine = new StateMachine();
        moveState = new MainMoveState(stateMachine, this);
        idelState = new MainIdelState(stateMachine, this);
        attackState = new MainAttackState(stateMachine, this);
        jumpState = new MainJumpState(stateMachine, this);
        stunnedState = new MainStunState(stateMachine, this,stunTime);
        weaponAttackState = new MainWeaponAttackState(stateMachine, this);

    }

    protected override void Start()
    {
        base.Start();

        InitPlayerData();

        if (LevelManager.Instance != null)
        {
            ApplyLevel(LevelManager.Instance.Level);
        }

        stateMachine.InitState(idelState);

        GameEventBus.OnGameRestart += GameEventBus_OnGameRestart;
        GameEventBus.OnPlayerSpawn += GameEventBus_OnPlayerSpawn;
        GameEventBus.OnLevelUp += ApplyLevel;
        healthBase.OnDead += OnDead;
    }

 

    protected override void OnDestroy()
    {
        base.OnDestroy();
        GameEventBus.OnGameRestart -= GameEventBus_OnGameRestart;
        GameEventBus.OnPlayerSpawn -= GameEventBus_OnPlayerSpawn;
        GameEventBus.OnLevelUp -= ApplyLevel;
        healthBase.OnDead -= OnDead;
    }

    private void Update()
    {
        stateMachine.UpdateState();

    }
    private void FixedUpdate()
    {
        //Profiler.BeginSample("Limit moving");
        move?.LimitMoving
        (
            ragdollController?.actionBase?.GetBalance(BalanceType.body_up).Rb,
            ragdollController?.actionBase?.GetBalance(BalanceType.right_leg).Rb,
            ragdollController?.actionBase?.GetBalance(BalanceType.left_leg).Rb
        );
        //Profiler.EndSample();

        stateMachine.UpdatePhysicState();
    }

    #region GameEventBus event
   
    private void GameEventBus_OnGameRestart()
    {
        InitPlayerData();
    }
    private void GameEventBus_OnPlayerSpawn(CharacterCtrl obj)
    {
        obj = this;
    }

    public override void OnTakeDamage(float damage)
    {
        base.OnTakeDamage(damage);

        if(healthBase.CurrentHealth < healthBase.MaxHealth * 0.3f)
        {
            if (ZenManager.Instance == null || ZenManager.Instance.cameraShaker == null) return;
            ZenManager.Instance.cameraShaker.ShakeCam();

            HapticPatterns.PlayPreset(HapticPatterns.PresetType.SoftImpact);
        }
    }
    public override void OnDead()
    {
        FirebaseService.Instance.LogEvent("player dead", new EventParameter("time", "2025"));
        LastPositionBeforeDead = transform.position;

        //SetKnockBackBalance();
        VfxBase vfx = null;
        ZenManager.Instance?.vfxPoolManager?.SpawnVfx(StringConst.DIEVFX, gameObject, out vfx);

        if (ragdollController != null)
        {
            ragdollController.DisableRagdoll();
            ragdollController.KnockBackCharacter(GetKnockDir());
        }
        else return;

        GameEventBus.RaisePlayerLose(this);
    }

    public void ApplyLevel(int level)
    {

        /* float oldMax = healthBase.MaxHealth;
         float oldPercent = healthBase.CurrentHealth / oldMax;*/

        StatsCalculator.ApplyLevelStats(
            stats,
            level,
            levelStatModifier
        );

        healthBase.SetMaxHealth(stats.MaxHealth);
        //healthBase.SetCurrentHealth(stats.MaxHealth * oldPercent);
        healthBase.InitHealth();
    }
    #endregion

    #region Stats setup
    private void InitPlayerData()
    {
        if (DataManager.Instance == null) return;
        PlayerData data = DataManager.Instance.PlayerData;
        float maxHP = data.Health;
        float finalDamage = data.Damage;
        float critChane = data.CritChance;
        float critMultiplier = data.CritMultiplier;

        stats.SetMaxHealth(maxHP);
        stats.SetDamageBase(finalDamage);
        stats.SetCritChane(critChane);
        stats.SetCritMultiplier(critMultiplier);

        healthBase.SetMaxHealth(stats.MaxHealth, true);
    }
    #endregion

    public void SetAttackDirection(Vector2 pos)
    {
        Vector3 tapWorldPos = Camera.main.ScreenToWorldPoint(pos);
        tapWorldPos.z = 0;
        attackDir = (tapWorldPos - transform.position).normalized;
    }
   

    public override Vector2 GetKnockDir()
    {
        if (zone == null) return Vector2.up;
        EnemyAI enemy = zone.GetNearestEnemy(transform);
        if(enemy == null) return Vector2.up;
        return (transform.position - enemy.transform.position).normalized;
    }


}