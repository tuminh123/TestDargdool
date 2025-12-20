using UnityEngine;

public class WeaponDamage : DamageBase
{
    [SerializeField] private float damageInit;

    private void Awake()
    {
        ResetDamage();
    }
    public void ResetLayer()
    {
        SetTargetLayer("Nothing");
    }
    public void SetTargetLayer(string layer)
    {
        this.layer = LayerMask.GetMask(layer);
    }
    public void SetWeaponDamage(float damage)
    {
        this.damageBase = this.damageBase + damage;
    }
    public void ResetDamage()
    {
        this.damageBase = damageInit;
    }
}
