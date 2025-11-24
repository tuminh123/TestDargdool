using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[System.Serializable]
public class Stats
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float damageBase;

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
    public DamageDetect[] damageDetect { get; private set; }
    #endregion
    [SerializeField] protected Stats stats;

    //Balance
    protected Balance bodyParent;
    protected Balance[] childBalance;

    [SerializeField] protected float stunTime;
    [SerializeField] protected float knockBackForce;
    protected Transform target;

    protected Vector2 attackDir;
    protected Coroutine damageCoroutine;
    protected bool isStunned =false;
    //get
    public Vector2 AttackDir=> attackDir;
    public bool IsStunned => isStunned;
    public Stats Stats => stats;

    protected virtual void Awake()
    {

        idle = GetComponentInChildren<Idle>();
        move = GetComponentInChildren<Move>();
        attack = GetComponentInChildren<Attack>();
        groundDetect = GetComponentInChildren<GroundDetect>();

        healthBase = GetComponentInChildren<HealthBase>();
        damageDetect = GetComponentsInChildren<DamageDetect>();

        bodyParent = transform.GetComponent<Balance>();
        childBalance = transform.GetComponentsInChildren<Balance>();

        //healthBase.SetMaxHealth(stats.MaxHealth);
        //foreach (var item in damageDetect)
        //{
        //    if (item == null) continue;
        //    item.SetDamageBase(stats.DamageBase);
        //}
    }
    private void Start()
    {
      
    }
    public abstract void OnDead();
    protected abstract Vector2 GetKnockDir();

    #region Health event
    protected virtual void OnEnable()
    {
        if (healthBase == null) return;
        healthBase.OnDead += OnDead;
        healthBase.OnTakeDamage += OnTakeDamage;
    }

    protected virtual void OnDisable()
    {
        if (healthBase == null) return;
        healthBase.OnDead -= OnDead;
        healthBase.OnTakeDamage += OnTakeDamage;

        StopDamageCoroutine();
    }
    private void OnDestroy()
    {
        StopDamageCoroutine();
    }
    
    private void OnTakeDamage()
    {
        SetKnockBackBalance();

        damageCoroutine = StartCoroutine(SetBalanceTrigger());

    }
    private IEnumerator SetBalanceTrigger()
    {
        isStunned = true;

        SetTriggerBalance(false);

        yield return new WaitForSeconds(stunTime);

        SetTriggerBalance(true);

        isStunned = false;
    }

    public void SetTriggerBalance(bool value)
    {
        foreach (var item in childBalance)
        {
            item.SetIsTrigger(value);
        }
        bodyParent.SetIsTrigger(value);
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

    public void StopDamageCoroutine()
    {
        if (damageCoroutine != null)
        {
            StopCoroutine(damageCoroutine);
            damageCoroutine = null;
        }
    }
    #endregion

    public void SendDamage()
    {
        foreach (var item in damageDetect)
        {
            if (item == null) continue;
            item.SenderDamageTo();
        }
    }

    public void Buff(float healthMultiplier, float damageMultiplierr)
    {
        Debug.Log("buff");
        float buffMaxHealth = stats.MaxHealth*healthMultiplier;
        float buffDamageBase = stats.DamageBase * damageMultiplierr;
        Debug.Log(buffMaxHealth);
        Debug.Log(buffDamageBase);
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
