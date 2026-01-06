using UnityEngine;
public enum HandType
{
    None = 0,
    Left = 1,
    Right = 2,
}
public class HandController : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] FixedJoint2D joint;
    [SerializeField] HandType handType;

    WeaponBase weapon;

    public bool IsHolding => weapon != null;
    public WeaponBase CurrentWeapon => weapon;

    private void Awake()
    {
        joint.enabled = false;
    }

    #region EQUIP

    public void AttachWeapon_Physics(WeaponBase newWeapon)
    {
        if (newWeapon == null) return;
        weapon = newWeapon;

        weapon.transform.SetParent(transform);
        weapon.transform.position = transform.position;

        weapon.Equipping();
        ApplyHandFlip(weapon);


        joint.connectedBody = weapon.rb;
        joint.breakForce = Mathf.Infinity;
        joint.breakTorque = Mathf.Infinity;
        joint.enabled = true;
    }

    #endregion

    #region UNEQUIP

    public WeaponBase DetachWeapon_Physics(Vector2 dir)
    {
        if (!weapon) return null;

        joint.enabled = false;
        joint.connectedBody = null;

        weapon.transform.SetParent(null);
        weapon.UnEquipping();

        weapon.WeaponFly(dir);

        WeaponBase dropped = weapon;
        weapon = null;

        return dropped;
    }

    #endregion

    void ApplyHandFlip(WeaponBase weapon)
    {
        weapon.transform.localScale = handType == HandType.Left ? new Vector3(-1, 1, 1) : new Vector3(1,1,1);
    }

}
