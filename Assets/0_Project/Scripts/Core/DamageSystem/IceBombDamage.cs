using System.Collections;
using UnityEngine;

public class IceBombDamage : BombDamageBase
{
    public override void VfxSpawm(GameObject obj)
    {
        VfxBase vfxIce = null;
        ZenManager.Instance?.vfxPoolManager?.SpawnVfx(StringConst.ICEHITVFX, obj, out vfxIce);
    }
}