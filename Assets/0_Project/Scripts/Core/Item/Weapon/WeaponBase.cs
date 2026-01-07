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
    [SerializeField] protected ItemDeSpawn weaponDeSpawn;
    [SerializeField] protected float damage = 100;
    [SerializeField] protected WeaponPhysicDamageDealer damageDealer;
    [SerializeField] protected TrailRenderer[] trails; 
    public bool IsAttacking { get;private set; }

    public GameObject OnjSend => gameObject;
    public WeaponPhysicDamageDealer DamageDealer => damageDealer;

    public event Action OnAttackStart;
    //Coroutine moveCoroutine;
    
    //get
    public float Damage => damage;

    private void Start()
    {
        if (damageDealer == null)
        {
            damageDealer = GetComponentInChildren<WeaponPhysicDamageDealer>();
        }
        damageDealer.Init(this, this);
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
    public void Equipping()
    {
        if (rb == null) return;

        SetRb2d(RigidbodyType2D.Kinematic);

        if (weaponDeSpawn == null) return;
        weaponDeSpawn.gameObject.SetActive(false);
    }

   

    public void UnEquipping()
    {
        if (rb == null) return;

        SetRb2d(RigidbodyType2D.Dynamic);

        if (weaponDeSpawn == null) return;
        weaponDeSpawn.gameObject.SetActive(true);
    }

    private void SetRb2d(RigidbodyType2D type2D)
    {
        rb.bodyType = type2D;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
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

    #region Old System
    /*   public void MoveToHand(HandController hand)
       {
           if (moveCoroutine != null)
               StopCoroutine(moveCoroutine);

           moveCoroutine = StartCoroutine(MoveRoutine(hand));
       }

       IEnumerator MoveRoutine(HandController hand)
       {
           rb.simulated = false;

           while (Vector2.Distance(transform.position, hand.transform.position) > 0.05f)
           {
               Vector3 targetPos = hand.transform.position - transform.position;

               transform.position = Vector3.Lerp(
                   transform.position,
                   targetPos,
                   Time.deltaTime * 20
               );

               yield return null;
           }
       }

       public void ResetWeapon()
       {

           if (fixedJoint2D == null || weaponDeSpawn == null) return;
           fixedJoint2D.enabled = false;
           weaponDeSpawn.gameObject.SetActive(true);

           fixedJoint2D.connectedBody = null;
       }
       public void Equipping(Rigidbody2D rb)
       {
           transform.localPosition = Vector3.zero;
           transform.localRotation = Quaternion.identity;

           if (fixedJoint2D == null || weaponDeSpawn == null) return;
           fixedJoint2D.enabled = true;
           weaponDeSpawn.gameObject.SetActive(false);

           fixedJoint2D.connectedBody = rb;
       }*/
    #endregion

}
