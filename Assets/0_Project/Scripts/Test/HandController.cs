using UnityEngine;

public class HandController : MonoBehaviour
{
    /*[Header("Hand")]
    [SerializeField] FixedJoint2D handJoint;
    [SerializeField] Rigidbody2D armRb;

    [Header("Weapon Attach")]
    [SerializeField] float weaponRotationOffset;
    WeaponBase currentWeapon;

    public float WeaponRotationOffset => weaponRotationOffset;

    void Awake()
    {
        handJoint.enabled = false;
    }

    void FixedUpdate()
    {
        if (currentWeapon)
        {
            currentWeapon.rb.MoveRotation(
                armRb.rotation + weaponRotationOffset
            );
        }
    }

    #region Equip

    public void AttachWeapon(WeaponBase weapon)
    {
        currentWeapon = weapon;

        handJoint.connectedBody = weapon.rb;
        handJoint.breakForce = Mathf.Infinity;
        handJoint.breakTorque = Mathf.Infinity;
        handJoint.enabled = true;
    }

    public void DetachWeapon()
    {
        handJoint.connectedBody = null;
        handJoint.enabled = false;
        currentWeapon = null;
    }

    public bool IsHolding => currentWeapon != null;

    #endregion

    #region Combat

    public void Swing(float direction, float force)
    {
        // direction: -1 hoặc 1
        armRb.AddTorque(-direction * force, ForceMode2D.Impulse);

        if (IsHolding)
        {
            currentWeapon.rb.AddTorque(
                -direction * force * 0.5f,
                ForceMode2D.Impulse
            );
        }
    }

    public float GetSwingSpeed()
    {
        return Mathf.Abs(armRb.angularVelocity);
    }

    #endregion*/

    [Header("Hand Physics")]
    [SerializeField] Rigidbody2D armRb;
    [SerializeField] HingeJoint2D weaponJoint;

    WeaponBase currentWeapon;

    public bool IsHolding => currentWeapon != null;

    private void Awake()
    {
        weaponJoint.enabled = false;
    }
    void FixedUpdate()
    {
        if (currentWeapon)
        {
            var rb = currentWeapon.rb;
            rb.angularVelocity = Mathf.Clamp(
                rb.angularVelocity,
                -currentWeapon.maxAngularVelocity,
                currentWeapon.maxAngularVelocity
            );
        }
    }

    #region Equip

    public void AttachWeapon(WeaponBase weapon)
    {
        currentWeapon = weapon;

        weaponJoint.connectedBody = weapon.rb;
        weaponJoint.autoConfigureConnectedAnchor = false;

        weaponJoint.anchor = Vector2.zero;
        weaponJoint.connectedAnchor = weapon.handPivot;

        weaponJoint.useLimits = false;
        weaponJoint.useMotor = false;

        weaponJoint.enabled = true;
    }

    public void DetachWeapon()
    {
        weaponJoint.connectedBody = null;
        weaponJoint.enabled = false;
        currentWeapon = null;
    }

    #endregion

    #region Combat

    public void Swing(float direction, float force)
    {
        // 1️⃣ Tay vung trước
        armRb.AddTorque(-direction * force, ForceMode2D.Impulse);

        // 2️⃣ Truyền lực cho vũ khí → tạo quán tính
        if (currentWeapon)
        {
            currentWeapon.rb.AddTorque(
                -direction * force * currentWeapon.spinMultiplier,
                ForceMode2D.Impulse
            );
        }
    }

    #endregion
}
