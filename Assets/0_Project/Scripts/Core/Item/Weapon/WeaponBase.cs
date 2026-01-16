using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Threading;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public enum WeaponType
{
    NONE = 0,
    MELE = 1,
    RANGE = 2,
}

public abstract class WeaponBase : ItemBase,IAttackContext,IObjSendDamage
{
    [SerializeField] protected WeaponType weaponType = WeaponType.MELE;
    //[SerializeField] protected FixedJoint2D fixedJoint2D;
    [SerializeField] protected float damage = 100;
    [SerializeField] protected TrailRenderer[] trails;

    #region Component
    [Inject] public WeaponPhysicDamageDealer DamageDealer { get; private set; }
    [SerializeField] protected ItemDeSpawn weaponDeSpawn;

    /* [Inject]
     void Init(WeaponPhysicDamageDealer damageDealer)
     {
         this.damageDealer = damageDealer;
     }*/

    #endregion

    public bool IsAttacking { get;private set; }

    public GameObject OnjSend => gameObject;

    public event Action OnAttackStart;
    //Coroutine moveCoroutine;
    
    //get
    public float Damage => damage;

    private void Start()
    {
       /* if (damageDealer == null)
        {
            damageDealer = GetComponentInChildren<WeaponPhysicDamageDealer>();
        }*/
        DamageDealer.Init(this, this);
    }

    public void DisableAttack()
    {
        IsAttacking = false;
    }

    public void EnableAttack()
    {
        IsAttacking = true;
        OnAttackStart?.Invoke();
    }

    public void WeaponFly(Vector2 hitDirection)
    {
        rb.simulated = true;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        // Lực văng ra
        float throwForce = 30f;
        Vector2 force = hitDirection.normalized * throwForce + Vector2.up * 50f; // thêm lực lên để tạo vòng cung

        rb.AddForce(force, ForceMode2D.Impulse);

        // Lực xoay
        float torque = Random.Range(-15f, 15f);
        rb.AddTorque(torque, ForceMode2D.Impulse);
    }

    public void EnableEffect(bool enable)
    {
        if (trails.Length <= 0) return;
        foreach (var item in trails)
        {
            if (item == null) continue;
            item.emitting = enable;
        }
    }
}
