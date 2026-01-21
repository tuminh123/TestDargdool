
using UnityEngine;

public class VfxPoolManager : ObjectPoolManager<VfxBase>
{
    public void SpawnVfx(string name, GameObject obj, out VfxBase vfx)
    {
        SpawnVfx(name, obj.transform.position,Quaternion.identity, out vfx);
        SetParent(vfx, obj.transform);
    }
    public void SpawnVfx(string name,UnityEngine.Vector2 pos ,Quaternion rot,out VfxBase vfx)
    {
        vfx = Spawn(name,pos,rot);

        if (vfx == null) return;

        vfx.PlayVfx();
    }
    public void DeSpawnVfx(VfxBase vfx)
    {
        if (vfx == null) return;
        DeSpawn(vfx);
        vfx.StopVfx();
        SetParent(vfx);
    }
}
