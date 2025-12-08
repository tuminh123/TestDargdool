/*using UnityEngine;

public enum WeaponType
{
    NONE = 0,
    MELE = 1,
    RANGE = 2,
}

public abstract class WeaponBase : MonoBehaviour,IObjectPool
{
    [SerializeField] protected WeaponType type;
    public WeaponDeSpawn weaponDeSpawn {  get; protected set; }
    public WeaponDamage weaponDamage { get; protected set; }
    public Rigidbody2D rb { get; protected set; }

    // EquipWeapon System
    protected Transform handTarget;
    protected Vector3 equipOffset = Vector3.zero;
    protected float flipDirection = 1;
    [SerializeField]protected float rotationOffset = 0f;
    [SerializeField]protected bool isEquip;

    //get
    public WeaponType Type => type;
    public bool IsEquip => isEquip;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        weaponDeSpawn = GetComponentInChildren<WeaponDeSpawn>();
        weaponDamage = GetComponentInChildren<WeaponDamage>();
    }
    private void FixedUpdate()
    {
        if (!isEquip || handTarget == null) return;

        // Move towards hand
        Vector3 targetPos = handTarget.position + equipOffset;

        rb.MovePosition(targetPos);
    }
    public void EquipWeapon(Transform hand, float flip,Vector3 offset)
    {
        handTarget = hand;
        equipOffset = offset;
        flipDirection = flip;
        isEquip = true;

        // Flip scale
        transform.localScale = new Vector3(flip, 1, 1);

        // Disable ground despawn
        weaponDeSpawn.gameObject.SetActive(false);
    }

    public void UnEquipWeapon()
    {
        isEquip = false;
        weaponDeSpawn.gameObject.SetActive(true);
        SingletonManager.Instance.weaponPoolManager.DeSpawn(this);
    }
    public void RotateByDirection(Vector2 attackDirection)
    {
        if (!isEquip) return;

        float angle = Mathf.Atan2(attackDirection.y, attackDirection.x) * Mathf.Rad2Deg;

        // Flip theo tay trái/phải
        angle += flipDirection == 1 ? rotationOffset : -rotationOffset;

        rb.MoveRotation(angle);
    }
    public abstract string GetObjectName();
}*/
using UnityEngine;

public enum WeaponType
{
    NONE = 0,
    MELE = 1,
    RANGE = 2,
}

public abstract class WeaponBase : MonoBehaviour, IObjectPool
{
    [SerializeField] protected WeaponType type;
    [SerializeField] Collider2D col;
    [SerializeField] private CircleCollider2D circle;
    public WeaponDeSpawn weaponDeSpawn { get; protected set; }
    public WeaponDamage weaponDamage { get; protected set; }
    public Rigidbody2D rb { get; protected set; }
    public HingeJoint2D joint2D { get; private set; }

    // EquipWeapon System
    protected Transform handTarget;
    protected Vector3 equipOffset = Vector3.zero;
    protected float flipDirection = 1;
    [SerializeField] protected float rotationOffset = 0f;
    [SerializeField] protected bool isEquip;

    //get
    public WeaponType Type => type;
    public bool IsEquip => isEquip;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        weaponDeSpawn = GetComponentInChildren<WeaponDeSpawn>();
        weaponDamage = GetComponentInChildren<WeaponDamage>();
        joint2D = GetComponent<HingeJoint2D>();
    }
    private void OnEnable()
    {
        ResetWeapon();
    }
  /*  private void FixedUpdate()
    {
        if (handTarget == null) return;

        // Move towards hand
        Vector3 targetPos = handTarget.position + equipOffset;

        rb.MovePosition(targetPos);
    }*/
    public void EquipWeapon(Rigidbody2D hand, Transform handTranform, float flip, Vector3 offset)
    {
        EquipWeapon(handTranform, flip, offset);

        joint2D.enabled = true;
        circle.isTrigger = false;
        joint2D.connectedBody = hand;
        gameObject.layer = LayerMask.NameToLayer("Default");
       
    }
    public void EquipWeapon(Transform hand, float flip, Vector3 offset)
    {
        /* handTarget = hand;
         equipOffset = offset;
         flipDirection = flip;*/
        //isEquip = true;

        //transform.localScale = new Vector3(flip, 1, 1);

        transform.position = hand.transform.position;
        rb.linearVelocity = Vector2.zero;
        rb.totalTorque = 0;
        col.enabled = false;
        weaponDeSpawn.gameObject.SetActive(false);
    }
    public void UnEquipWeapon()
    {
        //isEquip = false;
        ResetWeapon();
        weaponDeSpawn.gameObject.SetActive(true);
        SingletonManager.Instance.weaponPoolManager.DeSpawn(this);
    }
    public void ResetWeapon()
    {
        joint2D.connectedBody = null;
        joint2D.enabled = false;
        col.enabled = true;
        circle.isTrigger = true;
        gameObject.layer= LayerMask.NameToLayer("Weapon");
    }
    public abstract string GetObjectName();
}
