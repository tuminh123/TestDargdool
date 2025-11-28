using UnityEngine;

public class Dagger : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private Transform model;
    private Rigidbody2D rb;
    private WeaponDamage weaponDamage;
    private bool hasHit = false;   // tránh gây damage 2 lần
    public WeaponDamage WeaponDamage=>weaponDamage;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        weaponDamage = GetComponentInChildren<WeaponDamage>();
    }

    private void Update()
    {
        // nếu đã trúng mục tiêu → không kiểm tra nữa
        if (hasHit) return;

        // kiểm tra damage
        if (weaponDamage.SenderDamageTo())
        {
            hasHit = true;
            Destroy(gameObject);
        }
    }

    public void SetDaggerAction(Vector2 dir,float rot)
    {
        rb.linearVelocity = dir * speed;
        model.rotation = new Quaternion(0, rot, 0,0);
    }

}
