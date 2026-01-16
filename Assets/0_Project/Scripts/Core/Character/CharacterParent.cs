using Core;
using Cysharp.Threading.Tasks;
using DamageNumbersPro;
using System.Collections;
using UnityEngine;
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
    #endregion
    [Space]
    [SerializeField] protected Stats stats;
    [Space]
    [SerializeField] private DamageNumber damageNumber;

    public StateMachine stateMachine { get; private set; }

    //damage
    protected Vector2 attackDir;
    protected bool isStunned =false;

    //Flip
    [Space]
    [SerializeField] protected float dirFace = 1;
    protected bool isFacingRight = true;

    // Replace the problematic auto-property with a standard property implementation

    public Vector2 AttackDir=> attackDir;
    public bool IsStunned => isStunned;
    public Stats Stats => stats;

    public GameObject OnjSend => transform.gameObject;

    public abstract void OnDead();
    public abstract Vector2 GetKnockDir();

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
      

        stateMachine = new StateMachine();

    }
    protected virtual void Start()
    {
        if (ragdollController != null)
        {
            ragdollController.InitLimbs(this);
            ragdollController.InitPhysicDamageDeal(this, attackContext);
        }
        else return;

            healthBase.OnTakeDamage += OnTakeDamage;
    }

    protected virtual void OnDestroy()
    {
        stateMachine.ExitState();
        if (healthBase == null) return;

        healthBase.OnTakeDamage -= OnTakeDamage;
    }

    #region Damage Event

    public virtual void OnTakeDamage(float damage)
    {
        if(healthBase.IsDead) return;

        string damageText = $" -{damage}";
        damageNumber.Spawn(transform.position, damageText);

        isStunned = true;
        if (ZenManager.Instance == null) return;
        VfxBase vfx = ZenManager.Instance.vfxPoolManager.Spawn(StringConst.HURTVFX, transform.position, Quaternion.identity);

        ZenManager.Instance.vfxPoolManager.SetParent(vfx, transform);

        if (SingletonManager.Instance == null || SingletonManager.Instance.soundManager == null) return;
        SingletonManager.Instance.soundManager.PlaySound(SoundType.Crunch);
    }
    public void SetIsStunned(bool isStunned)
    {
        this.isStunned = isStunned;
    }

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
    #endregion
   
    #region Reset character

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
        if (ragdollController == null || ragdollController?.Balances.Length <= 0) return;
        foreach (var item in ragdollController.Balances)
        {
            if (item == null) continue;
            Destroy(item.gameObject);
        }
    }

    private void ResetBalance()
    {
        if (ragdollController == null || ragdollController?.Balances.Length <= 0) return;
        foreach (var item in ragdollController.Balances)
        {
            if (item == null) continue;
            //item.hinge.enabled = false;
            item.ResetState();
        }
    }
    #endregion

    #region Utils
    public void FlipSystem(float x,Transform transform)
    {
        if (x > 0 && !isFacingRight) Flip(transform);
        else if (x < 0 && isFacingRight) Flip(transform);
        else return;
    }

    public void Flip(Transform transform)
    {
        dirFace *= -1;
        isFacingRight = !isFacingRight;
        transform.Rotate(0, 180, 0);
    }
    public void Buff(float healthMultiplier, float damageMultiplierr)
    {
        //Debug.Log("buff");
        float buffMaxHealth = stats.MaxHealth * healthMultiplier;
        float buffDamageBase = stats.DamageBase * damageMultiplierr;
        //Debug.Log(buffMaxHealth);
        //Debug.Log(buffDamageBase);
        stats.SetMaxHealth(buffMaxHealth);
        stats.SetDamageBase(buffDamageBase);

        healthBase.SetMaxHealth(stats.MaxHealth);
    }
    #endregion
}
