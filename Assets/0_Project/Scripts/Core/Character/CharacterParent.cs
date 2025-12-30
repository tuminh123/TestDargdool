using Core;
using DamageNumbersPro;
using System.Collections;
using UnityEngine;
using UnityEngine.U2D.IK;
using Zenject;
#region Stats Character

[System.Serializable]
public class Stats
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float damageBase;
    [SerializeField] private float critChane;
    [SerializeField] private float critMultiplier;

    //get
    public float MaxHealth => maxHealth;
    public float DamageBase => damageBase;
    public float CritChane => critChane;
    public float CritMultiplier => critMultiplier;
    public void SetMaxHealth(float maxHealth)
    {
        this.maxHealth = maxHealth;
    }
    public void SetDamageBase(float damageBase)
    {
        this.damageBase = damageBase;
    }
    public void SetCritChane(float critChane)
    {
        this.critChane = critChane;
    }
    public void SetCritMultiplier(float critMultiplier)
    {
        this.critMultiplier = critMultiplier;
    }
}

#endregion
public abstract class CharacterParent : MonoBehaviour,IResettable,IObjSendDamage
{
    [InjectOptional]
    private VfxPoolManager vfxPoolManager;

    #region Child component
    public Move move { get; private set; }
    public Attack attack { get; private set; }
    public Jump jump { get; private set; }
    public Idle idle { get; private set; }
    public HealthBase healthBase { get; private set; }
    public RagdollController ragdollController { get; private set; }
    public AttackContext attackContext { get; private set; }
    public GroundDetect groundDetect { get; private set; }
    public CharacterDamage[] damageDetect { get; private set; }
    #endregion
    [SerializeField] protected Stats stats;

    public StateMachine stateMachine { get; private set; }

    //Balance
    protected Balance[] childBalance;
    protected LimbHitBox[] limbHitBoxs;
    protected PhysicsDamageDealer[] physicsCharacterDamageDealers; 

    [SerializeField] protected float knockBackForce;

    //damage
    protected Vector2 attackDir;
    protected Coroutine damageCoroutine;
    protected bool isStunned =false;
    protected bool balanceColSkip;

    //get
    public Vector2 AttackDir=> attackDir;
    public bool IsStunned => isStunned;
    public Stats Stats => stats;

    public GameObject OnjSend => transform.gameObject;

    public abstract void OnDead();
    protected abstract Vector2 GetKnockDir();

    protected virtual void Awake()
    {
        
        idle = GetComponentInChildren<Idle>();
        move = GetComponentInChildren<Move>();
        jump = GetComponentInChildren<Jump>();
        attack = GetComponentInChildren<Attack>();
        groundDetect = GetComponentInChildren<GroundDetect>();

        healthBase = GetComponent<HealthBase>();
        ragdollController = GetComponent<RagdollController>();
        attackContext = GetComponent<AttackContext>();

        damageDetect = GetComponentsInChildren<CharacterDamage>();

        //bodyParent = transform.GetComponent<Balance>();
        childBalance = transform.GetComponentsInChildren<Balance>();
        limbHitBoxs = transform.GetComponentsInChildren<LimbHitBox>();
        physicsCharacterDamageDealers = transform.GetComponentsInChildren<PhysicsDamageDealer>();

        stateMachine = new StateMachine();

        foreach (var item in damageDetect)
        {
            if (item == null) continue;
            item.SetDamageBase(stats.DamageBase);
        }
    }
    protected virtual void Start()
    {
        InitLimbs();
        InitPhysicDamageDeal();
    }
    protected virtual void OnEnable()
    {
        if (healthBase == null) return;

        healthBase.OnTakeDamage += OnTakeDamage;
    }
    protected virtual void OnDisable()
    {
        stateMachine.ExitState();
        if (healthBase == null) return;
       
        healthBase.OnTakeDamage -= OnTakeDamage;

    }
    protected virtual void OnDestroy()
    {
        stateMachine.ExitState();
        if (healthBase == null) return;

        healthBase.OnTakeDamage -= OnTakeDamage;
    }

    void InitLimbs()
    {
        foreach (var limb in limbHitBoxs)
        {
            if(limb == null) continue;
            limb.Init(this);
        }
    }

    void InitPhysicDamageDeal()
    {
        Debug.Log($"[InitWeapons] attackContext = {attackContext}");
        foreach (var dealer in physicsCharacterDamageDealers)
        {
            if (dealer == null) continue;
            dealer.Init(this,attackContext);
        }
    }
    #region Damage Event
    public virtual void OnTakeDamage()
    {
        if(healthBase.IsDead) return;
        isStunned = true;
        if (ZenManager.Instance == null) return;
        VfxBase vfx = ZenManager.Instance.vfxPoolManager.Spawn(StringConst.HURTVFX, transform.position, Quaternion.identity);

        ZenManager.Instance.vfxPoolManager.SetParent(vfx, transform);

        if (SingletonManager.Instance == null || SingletonManager.Instance.soundManager == null) return;
        SingletonManager.Instance.soundManager.PlaySound(SoundType.Crunch);

    }
    public void EnableBalance()
    {
        foreach (var item in childBalance)
        {
            if(item == null) continue;
            item.EnablePose();
        }
    }
    public void DisableBalance()
    {
        foreach (var item in childBalance)
        {
            if (item == null) continue;
            item.DisablePose();
        }
    }

    public void SetKnockBackBalance( )
    {
        Vector2 knockBackDir = GetKnockDir();
        foreach (var item in childBalance)
        {
            if(item == null) continue ;
            item.Rb.linearVelocity = knockBackDir * knockBackForce; ;
            //item.Rb.AddForce(knockBackDir*knockBackForce,ForceMode2D.Impulse);
        }
    }
    public void SetIsStunned(bool isStunned)
    {
        this.isStunned = isStunned;
    }

    public virtual void SendDamage()
    {
        foreach (var item in damageDetect)
        {
            if (item == null) continue;
            item.SetDamageBase(stats.DamageBase);
            if (item.SenderDamageTo())
            {
                break;
            }
        }
    }
    #endregion


    public void Buff(float healthMultiplier, float damageMultiplierr)
    {
        //Debug.Log("buff");
        float buffMaxHealth = stats.MaxHealth*healthMultiplier;
        float buffDamageBase = stats.DamageBase * damageMultiplierr;
        //Debug.Log(buffMaxHealth);
        //Debug.Log(buffDamageBase);
        stats.SetMaxHealth(buffMaxHealth);
        stats.SetDamageBase(buffDamageBase);

        healthBase.SetMaxHealth(stats.MaxHealth);
        foreach (var item in damageDetect)
        {
            if (item == null) continue;
            item.SetDamageBase(stats.DamageBase);
        }
    }
    #region Reset character
    /* public void ResetBalance()
     {
         foreach (var item in childBalance)
         {
             if (item == null) continue;
             item.ResetState();
         }
         bodyParent.ResetState();
         attack.StopAttack();
     }*/
    public void ResetOnGameRestart()
    {
       StartCoroutine(ResetCharacter());
    }
    private IEnumerator ResetCharacter()
    {
        ResetBalance();

        yield return null;
        yield return new WaitForFixedUpdate();

        DestroyBalance();

        yield return new WaitForSecondsRealtime(0.1f);
        Destroy(gameObject);
    }

    private void DestroyBalance()
    {
        foreach (var item in childBalance)
        {
            if (item == null) continue;
            Destroy(item.gameObject);
        }
    }

    private void ResetBalance()
    {
        foreach (var item in childBalance)
        {
            if (item == null) continue;
            //item.hinge.enabled = false;
            item.ResetState();
        }
    }
    #endregion

    public void SendDamageBase()
    {
        Global.Send(new SignalSendDamage
        {
            damaged = DamageCaculate()
        });
    }
    private float DamageCaculate()
    {
        float baseDamage = stats.DamageBase;

        bool isCrit = UnityEngine.Random.value < stats.CritChane;

        if (isCrit)
        {
            baseDamage *= stats.CritMultiplier;
        }

        return baseDamage;
    }
}
