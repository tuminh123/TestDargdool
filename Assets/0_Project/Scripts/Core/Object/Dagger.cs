using UnityEngine;
using Zenject;

public class Dagger : ObjInGameBase
{
    [InjectOptional] private ObjInGamePoolManager objInGamePoolManager;
    [SerializeField] private Transform model;
    private BombDamageBase weaponDamage;
    private bool hasHit = false;   // tránh gây damage 2 lần
    public BombDamageBase WeaponDamage=>weaponDamage;
    //get
    public bool HasHit => hasHit;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        weaponDamage = GetComponentInChildren<BombDamageBase>();
    }

    private void Update()
    {
        // nếu đã trúng mục tiêu → không kiểm tra nữa
        if (hasHit) return;
/*
        // kiểm tra damage
        if (weaponDamage.SenderDamageTo())
        {
            hasHit = true;
            ZenManager.Instance.objInGamePoolManager.DeSpawn(this);
        }*/
    }

    public void SetDaggerAction(Vector2 dir,float rot)
    {
        rb.linearVelocity = dir * 15;
        model.rotation = new Quaternion(0, rot, 0,0);
    }

    public override string GetObjectName()
    {
        return StringConst.DAGGER;
    }
}
