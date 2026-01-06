using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;
using static HadesSDK.Ads.Core.AdService;

/*public enum EquipState
{
    Idle,      // Không cầm gì
    Pulling,   // Đang hút/kéo vũ khí về tay
    Holding    // Đã cầm vũ khí
}*/
public enum AttackMode
{
    BareHand,     // đánh tay
    WeaponPull,   // đánh khi vũ khí đang bay
    WeaponHold    // đánh khi đã cầm
}
public class PlayerWeaponEquip : MonoBehaviour
{
    public event System.Action<WeaponBase> OnEquip;
    public event System.Action OnDrop;
    public event System.Action<HandController> OnHand;

    [Header("Hands")]
    [SerializeField] HandController[] hands;

    [Header("Pull Physics")]
    [SerializeField] float grabForce = 2000f;
    [SerializeField] float grabDistance = 0.2f;

    //EquipState state = EquipState.Idle;
    HandController activeHand;
    TargetJoint2D pullJoint;

    WeaponBase pullingWeapon;   // đang kéo
    WeaponBase holdingWeapon;   // đang cầm

    //get
    public WeaponBase CurrentWeapon => holdingWeapon;
    public bool HasWeapon => holdingWeapon != null;
    //public 

    public AttackMode CurrentAttackMode
    {
        get
        {
            if (holdingWeapon != null)
                return AttackMode.WeaponHold;

            if (pullingWeapon != null)
                return AttackMode.WeaponPull;

            return AttackMode.BareHand;
        }
    }

    /*public bool HasWeapon { get;private set; }*/

    private void FixedUpdate()
    {
        /* if (state == EquipState.Pulling)
             UpdatePulling();*/
        if (pullingWeapon != null)
            UpdatePulling();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        /*if (state != EquipState.Idle) return;
        if (!collision.TryGetComponent(out WeaponBase weapon)) return;

        HasWeapon = true;

        BeginEquip(weapon);*/

        if (pullingWeapon || holdingWeapon) return;
        if (!collision.TryGetComponent(out WeaponBase weapon)) return;

        BeginEquip(weapon);
    }

    #region Equip
    void BeginEquip(WeaponBase weapon)
    {
        pullingWeapon = weapon;
        activeHand = ChooseHand();

        pullJoint = weapon.gameObject.AddComponent<TargetJoint2D>();
        pullJoint.autoConfigureTarget = false;
        pullJoint.target = activeHand.transform.position;
        pullJoint.maxForce = grabForce;
        pullJoint.frequency = 8f;
        pullJoint.dampingRatio = 1f;

        /*currentWeapon = weapon;
        activeHand = ChooseHand();
        state = EquipState.Pulling;

        pullJoint = weapon.gameObject.AddComponent<TargetJoint2D>();
        pullJoint.autoConfigureTarget = false;
        pullJoint.target = activeHand.transform.position;
        pullJoint.maxForce = grabForce;
        pullJoint.frequency = 8f;
        pullJoint.dampingRatio = 1f;*/
    }

    void UpdatePulling()
    {
        if (!pullingWeapon || !activeHand) return;

        pullJoint.target = activeHand.transform.position;

        float dist = Vector2.Distance(
            pullingWeapon.rb.position,
            activeHand.transform.position
        );

        if (dist <= grabDistance)
        {
            CompleteEquip();
            return;
        }

        /*if (!currentWeapon || !activeHand) return;

        pullJoint.target = activeHand.transform.position;

        float dist = Vector2.Distance(
            currentWeapon.rb.position,
            activeHand.transform.position
        );

        if (dist <= grabDistance) CompleteEquip();*/
    }

    void CompleteEquip()
    {
        Destroy(pullJoint);

        activeHand.AttachWeapon_Physics(pullingWeapon);
        holdingWeapon = pullingWeapon;
        pullingWeapon = null;

        /*Destroy(pullJoint);

        activeHand.AttachWeapon_Physics(currentWeapon);

        state = EquipState.Holding;*/
    }
    #endregion

    #region UnEquip
    public void DropWeapon(Vector2 dir)
    {
        if (!holdingWeapon) return;

        WeaponBase dropped = activeHand.DetachWeapon_Physics(dir);
        holdingWeapon = null;
        activeHand = null;

        OnDrop?.Invoke();
    }
    public void ThrowWeapon(Vector2 throwDir, float force = 30f)
    {
        if (!holdingWeapon) return;
        if (!activeHand || !activeHand.IsHolding) return;

        WeaponBase weapon = activeHand.DetachWeapon_Physics();
        if (!weapon) return;

        weapon.rb.linearVelocity = Vector2.zero;
        weapon.rb.angularVelocity = 0f;

        //weapon.rb.AddForce(throwDir.normalized * force, ForceMode2D.Impulse);
        weapon.rb.linearVelocity = throwDir.normalized * force;
        // Lực xoay
        float torque = Random.Range(-720f, 720f);
        weapon.rb.AddTorque(torque, ForceMode2D.Impulse);

        holdingWeapon = null;
        activeHand = null;

        OnDrop?.Invoke();
    }
    public void SetFaceWeaponAttack(Vector2 dir)
    {
        if(holdingWeapon == null) return;
        holdingWeapon.transform.localScale = dir.x < 0 ? new Vector3(-1, 1, 1) : new Vector3(1,1,1);
    }
    //public void BoostPull(float forceMultiplier = 1.5f)
    //{
    //    if (pullJoint != null)
    //        pullJoint.maxForce *= forceMultiplier;
    //}
    //public void Unequip()
    //{
    //    if (state != EquipState.Holding) return;
    //    if (!activeHand || !activeHand.IsHolding) return;

    //    WeaponBase dropped = activeHand.DetachWeapon_Physics();
    //    if (!dropped) return;

    //    ResetWeaponPhysics(dropped);

    //    state = EquipState.Idle;
    //    activeHand = null;

    //    HasWeapon = false;
    //}

    //void ResetWeaponPhysics(WeaponBase weapon)
    //{
    //    weapon.rb.linearVelocity = Vector2.zero;
    //    weapon.rb.angularVelocity = 0f;
    //}
    #endregion
    HandController ChooseHand()
    {
        if (hands.Length <= 0) return null;
        return hands[Random.Range(0,hands.Length)];
    }
}
