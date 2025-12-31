using Cysharp.Threading.Tasks;
using System;
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
    public ItemDeSpawn weaponDeSpawn {  get; protected set; }

    public bool IsAttacking => throw new NotImplementedException();

    public GameObject OnjSend => throw new NotImplementedException();

    public event Action OnAttackStart;

    [Header("Physics")]
    public Rigidbody2D rb;

    [Tooltip("Độ dễ xoay – kiếm cao, búa thấp")]
    public float spinMultiplier = 1.2f;

    [Tooltip("Pivot tại chuôi vũ khí (local space)")]
    public Vector2 handPivot;

    public float maxAngularVelocity = 720f; // độ/giây

    protected override void Awake()
    {
        base.Awake();
        //weaponDeSpawn = GetComponentInChildren<ItemDeSpawn>();
    }

    public void DisableAttack()
    {
        throw new NotImplementedException();
    }

    public void EnableAttack()
    {
        throw new NotImplementedException();
    }
    public void FaceDirection(float direction)
    {
        // direction: 1 = phải, -1 = trái
        float angle = direction > 0 ? 0f : 180f;
        rb.MoveRotation(angle);
    }
    public void AttachToHand(HandController hand)
    {
        // Lấy rotation của tay
        /*float targetRotation =
            hand.transform.eulerAngles.z +
            hand.WeaponRotationOffset;

        rb.MoveRotation(targetRotation);*/
    }

}
