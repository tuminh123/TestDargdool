using UnityEngine;

public class HandController : MonoBehaviour
{
    [Header("Hand")]
    [SerializeField] FixedJoint2D handJoint;
    [SerializeField] Rigidbody2D armRb;

    WeaponBase currentWeapon;

    void Awake()
    {
        handJoint.enabled = false;
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
    }

    public float GetSwingSpeed()
    {
        return Mathf.Abs(armRb.angularVelocity);
    }

    #endregion
}
