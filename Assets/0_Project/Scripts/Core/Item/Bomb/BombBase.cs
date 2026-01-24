using UnityEngine;

public class BombBase : ABomb
{
    public override void EffectSpawm()
    {
        VfxBase vfxExplosion = null;
        ZenManager.Instance?.vfxPoolManager?.SpawnVfx(StringConst.EXPLOSIONVFX, gameObject, out vfxExplosion);
    }

    public override string GetObjectName()
    {
        return StringConst.BOMB_BASE;
    }
}
