using UnityEngine;

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

    //get
    public WeaponType Type => type;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        weaponDeSpawn = GetComponentInChildren<WeaponDeSpawn>();
        weaponDamage = GetComponentInChildren<WeaponDamage>();
    }
    public void ResetWeapon()
    {
        weaponDeSpawn.gameObject.SetActive(true);
        rb.bodyType = RigidbodyType2D.Dynamic;
    }
    public abstract string GetObjectName();
}
