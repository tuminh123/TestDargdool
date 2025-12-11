using UnityEngine;
using Zenject;

public enum WeaponType
{
    NONE = 0,
    MELE = 1,
    RANGE = 2,
}

public abstract class WeaponBase : ItemBase
{
    [InjectOptional] private ItemPoolManager itemPoolManager;
    [SerializeField] protected WeaponType type;
    public ItemDeSpawn weaponDeSpawn {  get; protected set; }
    public WeaponDamage weaponDamage { get; protected set; }
    public FixedJoint2D joint { get; protected set; }
    //get
    public WeaponType Type => type;
    private void OnEnable()
    {
        ResetWeapon();
    }
    protected override void Awake()
    {
        base.Awake();
        weaponDeSpawn = GetComponentInChildren<ItemDeSpawn>();
        weaponDamage = GetComponentInChildren<WeaponDamage>();
        joint = GetComponentInChildren<FixedJoint2D>();
    }
    public void Equip(Rigidbody2D hand)
    {
        joint.enabled = true;
        joint.connectedBody = hand;
        weaponDeSpawn.gameObject.SetActive(false);
    }
    public void UnEquip()
    {
        ZenManager.Instance.itemPoolManager.DeSpawn(this);
        ResetWeapon();
    }
    public void ResetWeapon()
    {
        joint.enabled = false;
        joint.connectedBody = null;
        weaponDeSpawn.gameObject.SetActive(true);
    }
    
}
