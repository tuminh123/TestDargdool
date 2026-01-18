
using UnityEngine;

public abstract class EquipmentBase : MonoBehaviour
{
    public event System.Action OnDrop;
    [SerializeField] protected GameObject leftHand;
    [SerializeField] protected GameObject rightHand;
    protected GameObject currentHand;
    protected WeaponBase currentWeapon;

    //get
    public WeaponBase CurrentWeapon => currentWeapon;

    public abstract void SetAbstractWeaponWhenEquip();

    public bool HasWeapon => currentWeapon != null;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out WeaponBase weapon)) return ;
        if (HasWeapon) return;
        if (weapon.isEquipping) return;

        currentWeapon = weapon;
        
        weapon.Equipping();

        SetAbstractWeaponWhenEquip();

        SetWeapon();
        
    }
    protected virtual void SetWeapon()
    {
        if (currentWeapon == null) return;
        currentHand = Random.value > 0.5 ? leftHand : rightHand;

        currentWeapon.transform.position = currentHand.transform.position;
        currentWeapon.rb.bodyType = RigidbodyType2D.Kinematic;
        currentWeapon.transform.parent = currentHand.transform;
        SetFlipWeaponByHand(currentWeapon);
    }

    public virtual void DropWeapon(Vector2 dir)
    {
        OnDrop?.Invoke();
        if (currentWeapon == null) return;

        currentWeapon?.DamageDealer?.SetFaction(Faction.None);
        currentWeapon.transform.parent = null;
        currentHand = null;

        currentWeapon.rb.bodyType = RigidbodyType2D.Dynamic;
        currentWeapon.UnEquipping();
        currentWeapon.WeaponFly(dir);

        currentWeapon = null;

    }

    #region Flip weapon system
    public void SetFlipWeaponByAttackDir(Vector3 attackDir)
    {
        if (currentWeapon == null) return;
        if (attackDir.x < 0) currentWeapon.transform.localScale = new Vector3(-1, 1, 1);
        if (attackDir.x > 0) currentWeapon.transform.localScale = new Vector3(1, 1, 1);
    }
    private void SetFlipWeaponByHand(WeaponBase weapon)
    {
        if (currentHand == leftHand) weapon.transform.localScale = new Vector3(-1, 1, 1);
        if (currentHand == rightHand) weapon.transform.localScale = new Vector3(1, 1, 1);
    }
    #endregion
}
