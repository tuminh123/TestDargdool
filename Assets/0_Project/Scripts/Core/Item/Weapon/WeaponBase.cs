using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using Zenject;

public enum WeaponType
{
    NONE = 0,
    MELE = 1,
    RANGE = 2,
}

public abstract class WeaponBase : ItemBase,IAttackContext,IObjSendDamage
{
    [InjectOptional] private ItemPoolManager itemPoolManager;
    [SerializeField] protected WeaponType type = WeaponType.MELE;
    [SerializeField] protected float velocityFly = 20f;
    public Transform model {  get; private set; }
    public ItemDeSpawn weaponDeSpawn {  get; protected set; }
    public PhysicsDamageDealer  weaponDamage { get; protected set; }
    public FixedJoint2D joint { get; protected set; }

    //get
    public WeaponType Type => type;

    public bool IsAttacking { get; protected set; }

    public GameObject OnjSend => transform.gameObject;

    private void OnEnable()
    {
        ResetWeapon();
    }

    protected override void Awake()
    {
        base.Awake();
        weaponDeSpawn = GetComponentInChildren<ItemDeSpawn>();
        weaponDamage = GetComponentInChildren<PhysicsDamageDealer>();
        joint = GetComponentInChildren<FixedJoint2D>();
        model = transform.Find(StringConst.MODEL);
    }
    private void Start()
    {
        weaponDamage.Init(this, this);
    }
    public void Equip(Rigidbody2D hand)
    {
        joint.enabled = true;
        joint.connectedBody = hand;
        weaponDeSpawn.gameObject.SetActive(false);
    }
    public async UniTask UnEquip(CancellationToken token)
    {
        WeaponUnequipAction();
        await UniTask.Delay(1500,cancellationToken : token);
        ZenManager.Instance.itemPoolManager.DeSpawn(this);
       
    }
    public void ResetWeapon()
    {
        joint.enabled = false;
        joint.connectedBody = null;
        weaponDeSpawn.gameObject.SetActive(true);
        //weaponDamage.ResetLayer();
        //weaponDamage.ResetDamage();
    }
    private void WeaponUnequipAction()
    {
        ResetWeapon();

        if (ZenManager.Instance == null || ZenManager.Instance.itemPoolManager == null) return;
        Transform holder = ZenManager.Instance.itemPoolManager.Holder;
        ZenManager.Instance.itemPoolManager.SetParent(this, holder);

        transform.rotation = Quaternion.Euler(0, 0, 45); // xéo 45 độ
        rb.linearVelocity = transform.up * velocityFly;
    }

    public void EnableAttack()
    {
        IsAttacking = true;
    }

    public void DisableAttack()
    {
        IsAttacking = false;
    }
}
