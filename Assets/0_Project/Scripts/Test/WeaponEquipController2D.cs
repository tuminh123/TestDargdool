using UnityEngine;
public enum EquipMode2D
{
    HardParent,    // idling
    StrongHinge,   // attack
    WeakHinge,     // hit / stun
    Free           // drop
}

public class WeaponEquipController2D : MonoBehaviour
{
    [Header("Hand")]
    public Rigidbody2D handRB;
    public Transform handGrip;

    [Header("Runtime")]
    public Weapon2D currentWeapon;
    public EquipMode2D currentMode;

    // ==============================
    // PUBLIC API (Player dùng)
    // ==============================

    public void EquipWeapon(Weapon2D weapon)
    {
        if (currentWeapon != null)
            DropWeapon();

        currentWeapon = weapon;
        weapon.ApplyData();

        weapon.transform.position = handGrip.position;
        weapon.transform.rotation = handGrip.rotation;

        SetMode(EquipMode2D.HardParent);
    }

    public void DropWeapon()
    {
        if (currentWeapon == null) return;

        CleanupJoint();

        currentWeapon.transform.parent = null;
        currentWeapon.rb.simulated = true;

        currentWeapon = null;
        currentMode = EquipMode2D.Free;
    }

    public void SetMode(EquipMode2D mode)
    {
        if (currentWeapon == null) return;
        if (currentMode == mode) return;

        CleanupJoint();
        currentMode = mode;

        switch (mode)
        {
            case EquipMode2D.HardParent:
                currentWeapon.rb.simulated = false;
                currentWeapon.transform.parent = handGrip;
                currentWeapon.transform.localPosition = Vector3.zero;
                currentWeapon.transform.localRotation = Quaternion.identity;
                break;

            case EquipMode2D.StrongHinge:
                AttachHinge(
                    currentWeapon.data.attackMotorSpeed,
                    currentWeapon.data.attackMotorTorque
                );
                break;

            case EquipMode2D.WeakHinge:
                AttachHinge(
                    currentWeapon.data.weakMotorSpeed,
                    currentWeapon.data.weakMotorTorque
                );
                break;

            case EquipMode2D.Free:
                currentWeapon.rb.simulated = true;
                break;
        }
    }

    // ==============================
    // INTERNAL
    // ==============================

    void AttachHinge(float speed, float torque)
    {
        currentWeapon.transform.parent = null;
        currentWeapon.rb.simulated = true;

        var joint = currentWeapon.gameObject.AddComponent<HingeJoint2D>();
        currentWeapon.joint = joint;

        joint.connectedBody = handRB;
        joint.autoConfigureConnectedAnchor = false;
        joint.anchor = Vector2.zero;
        joint.connectedAnchor =
            handRB.transform.InverseTransformPoint(handGrip.position);

        joint.useLimits = true;
        joint.limits = new JointAngleLimits2D
        {
            min = currentWeapon.data.minAngle,
            max = currentWeapon.data.maxAngle
        };

        joint.useMotor = true;
        joint.motor = new JointMotor2D
        {
            motorSpeed = speed,
            maxMotorTorque = torque
        };
    }

    void CleanupJoint()
    {
        if (currentWeapon != null && currentWeapon.joint != null)
            Destroy(currentWeapon.joint);
    }
}
