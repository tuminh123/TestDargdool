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
    [SerializeField] FixedJoint2D handJoint;
    [SerializeField] HandType handType;

    WeaponBase currentWeapon;
    //get
    public Rigidbody2D Rb => rb;

    public HandType HandType => handType;

    public void EquipWeapon(WeaponBase weapon)
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

}
