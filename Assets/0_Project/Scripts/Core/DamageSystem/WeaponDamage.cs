using UnityEngine;

public class WeaponDamage : DamageBase
{
    public void ResetLayer()
    {
        SetTargetLayer("Nothing");
    }
    public void SetTargetLayer(string layer)
    {
        this.layer = LayerMask.GetMask(layer);
    }
}
