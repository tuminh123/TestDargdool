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
}
