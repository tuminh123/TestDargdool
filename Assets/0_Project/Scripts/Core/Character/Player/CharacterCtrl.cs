
using Cysharp.Threading.Tasks;
using HadesSDK.Ads.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Zenject;

public class CharacterCtrl : CharacterParent
{    
    public static CharacterCtrl Instance { get; private set; }

    [SerializeField] private LevelStatModifier levelStatModifier;

    [SerializeField] private float stunTime = 4;
    public DetectionZone zone { get; private set; }
    public PlayerWeaponEquip weaponEquip { get; private set; }

    public WeaponBase currentWeaponBase { get; private set; }
    public Vector3 LastPositionBeforeDead { get; private set; }

    #region  State
    public MainMoveState moveState { get; private set; }
    public MainAttackState attackState { get; private set; }
    public MainIdelState idelState { get; private set; }
    public MainJumpState jumpState { get; private set; }
    public MainStunState stunnedState { get;private set; }
    public MainWeaponAttackState weaponAttackState { get; private set; }

    #endregion

    private CancellationTokenSource unWeapon;

    protected override void Awake()
    {
        base.Awake();
        Instance = this;

        zone = GetComponentInChildren<DetectionZone>();
        weaponEquip = GetComponentInChildren<PlayerWeaponEquip>();
        //state init
        //stateMachine = new StateMachine();
        moveState = new MainMoveState(stateMachine, this);
        idelState = new MainIdelState(stateMachine, this);
        attackState = new MainAttackState(stateMachine, this);
        jumpState = new MainJumpState(stateMachine, this);
        stunnedState = new MainStunState(stateMachine, this,stunTime);
        weaponAttackState = new MainWeaponAttackState(stateMachine, this);
    }
    
    protected override void OnEnable()
    {
        base.OnEnable();

        GameEventBus.OnPlayerSpawn += GameEventBus_OnPlayerSpawn;
        GameEventBus.OnLevelUp += ApplyLevel;

        healthBase.OnDead += OnDead;
        weaponEquip.OnEquip += WeaponEquip_OnEquip;

        if (currentWeaponBase == null || currentWeaponBase.weaponDamage == null) return;
        currentWeaponBase.weaponDamage.OnDealDamage += WeaponDamage_OnDealDamage;

    }
    protected override void OnDisable()
    {
        base.OnDisable();

        GameEventBus.OnPlayerSpawn -= GameEventBus_OnPlayerSpawn;
        GameEventBus.OnLevelUp -= ApplyLevel;

        healthBase.OnDead -= OnDead;
        weaponEquip.OnEquip -= WeaponEquip_OnEquip;

        unWeapon?.Cancel();
        unWeapon?.Dispose();

        if (currentWeaponBase == null || currentWeaponBase.weaponDamage == null) return;
        currentWeaponBase.weaponDamage.OnDealDamage -= WeaponDamage_OnDealDamage;

    }

   

    protected override void OnDestroy()
    {
        base.OnDestroy();
        GameEventBus.OnPlayerSpawn -= GameEventBus_OnPlayerSpawn;
        GameEventBus.OnLevelUp -= ApplyLevel;

        healthBase.OnDead -= OnDead;
        weaponEquip.OnEquip -= WeaponEquip_OnEquip;

        unWeapon?.Cancel();
        unWeapon?.Dispose();

        if (currentWeaponBase == null || currentWeaponBase.weaponDamage == null) return;
        currentWeaponBase.weaponDamage.OnDealDamage -= WeaponDamage_OnDealDamage;
    }

    private void Start()
    {
        InitPlayerData();
        stateMachine.InitState(idelState);
    }
    private void Update()
    {
        stateMachine.UpdateState();
    }
    private void FixedUpdate()
    {
        stateMachine.UpdatePhysicState();
    }

    private void GameEventBus_OnPlayerSpawn(CharacterCtrl obj)
    {
        obj = this;
    }

    // Weapon equipment event
    private void WeaponEquip_OnEquip(WeaponBase obj)
    {
        if (obj == null) return;

        //obj.weaponDamage?.SetWeaponDamage(stats.DamageBase);

        currentWeaponBase = obj;

    }
    private void WeaponDamage_OnDealDamage(AttackContext obj)
    {
        obj = attackContext;
    }
    public override void OnTakeDamage()
    {
        base.OnTakeDamage();

        if(healthBase.CurrentHealth < healthBase.MaxHealth * 0.3f)
        {
            if (ZenManager.Instance == null || ZenManager.Instance.cameraShaker == null) return;
            ZenManager.Instance.cameraShaker.ShakeCam();
        }
    }
    #region Stats setup
    private void InitPlayerData()
    {
        if (DataManager.Instance == null) return;
        PlayerData data = DataManager.Instance.PlayerData;
        float maxHP = data.Health;
        float finalDamage = data.CalculateDamage();
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
    public override void OnDead()
    {
        FirebaseService.Instance.LogEvent("player dead", new EventParameter("time", "2025"));
        LastPositionBeforeDead = transform.position;

        SetKnockBackBalance();
        DisableBalance();

        RemoveWeapon(currentWeaponBase);
        weaponEquip.Equipping();

        GameEventBus.RaisePlayerLose(this);
        //OnDeadWait().Forget();
    }

    protected override Vector2 GetKnockDir()
    {
        if (zone == null) return Vector2.up;
        EnemyAI enemy = zone.GetNearestEnemy(transform);
        if(enemy == null) return Vector2.up;
        return (transform.position - enemy.transform.position).normalized;
    }
    private void RemoveWeapon(WeaponBase weapon)
    {
        if (weapon == null)
        {
            Debug.LogWarning("RemoveWeapon called but weapon is NULL");
            return;
        }
        // Cancel task cũ nếu có
        unWeapon?.Cancel();
        unWeapon?.Dispose();

        unWeapon = new CancellationTokenSource();

        // Link với lifecycle Character
        var linkedToken = CancellationTokenSource.CreateLinkedTokenSource(
            unWeapon.Token,
            this.GetCancellationTokenOnDestroy()
        ).Token;

        weaponEquip.UnEquipping();

        UniTaskSafe.Forget(
            weapon.UnEquip,
            linkedToken,
            "Remove Weapon"
        );
    }
    public void ApplyLevel(int level)
    {
        if (DataManager.Instance == null) return;

        float oldMax = healthBase.MaxHealth;
        float oldPercent = healthBase.CurrentHealth / oldMax;

        StatsCalculator.ApplyLevelStats(
            stats,
            DataManager.Instance.PlayerData,
            level,
            levelStatModifier
        );

        healthBase.SetMaxHealth(stats.MaxHealth);
        healthBase.SetCurrentHealth(stats.MaxHealth * oldPercent);

        foreach (var dmg in damageDetect)
            dmg.SetDamageBase(stats.DamageBase);
    }

}