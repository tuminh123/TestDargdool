using System.Collections;
using UnityEngine;

public class BomBaseDamage : BombDamageBase
{
    public override void VfxSpawm(GameObject obj)
    {
        VfxBase vfxFire = null;
        ZenManager.Instance?.vfxPoolManager?.SpawnVfx(StringConst.FIREVFX, obj, out vfxFire);
    }
}