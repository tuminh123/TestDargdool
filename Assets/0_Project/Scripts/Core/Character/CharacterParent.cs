using DamageNumbersPro;
using System.Collections;
using UnityEngine;

[System.Serializable]
public class Stats
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float damageBase;
    //[SerializeField] private float scritDamage;

    //get
    public float MaxHealth => maxHealth;
    public float DamageBase => damageBase;

    public void SetMaxHealth(float maxHealth)
    {
        this.maxHealth = maxHealth;
    }
    public void SetDamageBase(float damageBase)
    {
        this.damageBase = damageBase;
    }
}
public abstract class CharacterParent : MonoBehaviour
{
    #region Child component
    public Move move { get; private set; }
    public Attack attack { get; private set; }
    public Idle idle { get; private set; }
    public HealthBase healthBase { get; private set; }
    public GroundDetect groundDetect { get; private set; }
    public CharacterDamage[] damageDetect { get; private set; }
    #endregion
    [SerializeField] protected Stats stats;
    [SerializeField] protected Collider2D detectCol;

    public StateMachine stateMachine { get; private set; }

    //Balance
    [SerializeField]protected Balance bodyParent;
    protected Balance[] childBalance;

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
    public Collider2D DetectCol => detectCol;

    protected virtual void Awake()
    {

        idle = GetComponentInChildren<Idle>();
        move = GetComponentInChildren<Move>();
        attack = GetComponentInChildren<Attack>();
        groundDetect = GetComponentInChildren<GroundDetect>();

        healthBase = GetComponentInChildren<HealthBase>();
        damageDetect = GetComponentsInChildren<CharacterDamage>();

        //bodyParent = transform.GetComponent<Balance>();
        childBalance = transform.GetComponentsInChildren<Balance>();

        stateMachine = new StateMachine();

        healthBase.SetMaxHealth(stats.MaxHealth);
        foreach (var item in damageDetect)
        {
            if (item == null) continue;
            item.SetDamageBase(stats.DamageBase);
        }
    }
    private void Start()
    {
        
    }
    public abstract void OnDead();
    protected abstract Vector2 GetKnockDir();

   
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

    #region Damage Event
    public void OnTakeDamage()
    {
        isStunned = true;
        VfxBase vfx = SingletonManager.Instance.vfxPoolManager.Spawn(StringConst.HURTVFX, transform.position, Quaternion.identity);

        SingletonManager.Instance.vfxPoolManager.SetParent(vfx, bodyParent.transform);
        
    }
    public void SetTriggerBalance(bool value)
    {
        foreach (var item in childBalance)
        {
            if(item == null) continue;
            item.SetIsTrigger(value);
        }
        bodyParent.SetIsTrigger(value);
        detectCol.enabled = value;
    }
    public void SetKnockBackBalance( )
    {
        Vector2 knockBackDir = GetKnockDir();
        foreach (var item in childBalance)
        {
            item.Rb.linearVelocity = knockBackDir * knockBackForce; ;
        }
        bodyParent.Rb.linearVelocity = knockBackDir * knockBackForce; ;
    }
    public void SetIsStunned(bool isStunned)
    {
        this.isStunned = isStunned;
    }

    public void SendDamage()
    {
        foreach (var item in damageDetect)
        {
            if (item == null) continue;
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

}
