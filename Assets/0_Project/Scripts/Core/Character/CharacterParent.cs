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

    #region Child component
    public Move move { get; private set; }
    public Attack attack { get; private set; }
    public Jump jump { get; private set; }
    public Idle idle { get; private set; }
    public HealthBase healthBase { get; private set; }
    public RagdollController ragdollController { get; private set; }
    public AttackContext attackContext { get; private set; }
    public GroundDetect groundDetect { get; private set; }
    public EquipmentBase weaponEquip { get; private set; }
    #endregion
    [Space]
    [SerializeField] protected Stats stats;
    [Space]
    [SerializeField] private DamageNumber damageNumber;
    [Space]
    [SerializeField] private GameObject leftHand;
    [SerializeField] private GameObject rightHand;

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
    public GameObject LeftHand => leftHand;
    public GameObject RightHand => rightHand;

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
        weaponEquip = GetComponentInChildren<EquipmentBase>();

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
        Vector2 dir = Random.value > 0.5f ? Vector2.right : Vector2.left;
        weaponEquip?.DropWeapon(dir);
        if (healthBase.IsDead) return;

        string damageText = $" -{damage}";
        damageNumber.Spawn(transform.position, damageText);

        isStunned = true;
        //if (ZenManager.Instance == null || ZenManager.Instance.vfxPoolManager) return;
        VfxBase vfx = null;
        ZenManager.Instance?.vfxPoolManager?.SpawnVfx(StringConst.HURTVFX, gameObject,out vfx);

        if (vfx == null) return;
        ZenManager.Instance?.vfxPoolManager?.SetParent(vfx, transform);

        if (SingletonManager.Instance == null || SingletonManager.Instance.soundManager == null) return;
        SingletonManager.Instance.soundManager.PlaySound(SoundType.Crunch);
    }
    public void SetIsStunned(bool isStunned)
    {
        this.isStunned = isStunned;
    }

    #region Damage Caculate
    public void SendDamageBase()
    {
        Global.Send(new SignalSendDamage
        {
            damaged = DamageCaculate()
        });
    }

    public void SendWeaponDamageBase()
    {
        if (weaponEquip == null || weaponEquip.CurrentWeapon == null) return;

        Global.Send(new SignalSendDamage
        {
            damaged = DamageCaculate() + weaponEquip.CurrentWeapon.Damage
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
    public void Buff(float healthMultiplier, float damageMultiplierr,float critC,float critM)
    {
        //Debug.Log("buff");
        float buffMaxHealth = stats.MaxHealth * healthMultiplier;
        float buffDamageBase = stats.DamageBase * damageMultiplierr;
        float buffCritC = stats.CritChane * critC;
        float buffCritM = stats.CritMultiplier * critM;
        //Debug.Log(buffMaxHealth);
        //Debug.Log(buffDamageBase);
        stats.SetMaxHealth(buffMaxHealth);
        stats.SetDamageBase(buffDamageBase);
        stats.SetCritChane(buffCritC);
        stats.SetCritMultiplier(buffCritM);

        healthBase.SetMaxHealth(stats.MaxHealth);
    }
    public void EffectSpawns(GameObject @object, out VfxBase vfx)
    {
        vfx = null;
        if (ZenManager.Instance != null && ZenManager.Instance.vfxPoolManager != null)
        {
            ZenManager.Instance.vfxPoolManager.SpawnVfx(StringConst.ATTACKVFX, @object, out vfx);
        }
    }
    public void EffectDeSpawns(VfxBase @object)
    {
        ZenManager.Instance?.vfxPoolManager?.DeSpawn(@object);
    }
    #endregion
}
