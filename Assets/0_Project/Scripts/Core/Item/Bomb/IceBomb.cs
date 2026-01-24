using System.Collections;
using UnityEngine;

public class IceBomb : ABomb
{
    [SerializeField] ParticleSystem _particleSystem;

    protected override void OnEnable()
    {
        _particleSystem?.Play();
        base.OnEnable();
    }
    protected override void OnDisable()
    {
        _particleSystem?.Stop();
        base.OnDisable();
    }

    public override void EffectSpawm()
    {
        VfxBase vfxIceBomb = null;
        ZenManager.Instance?.vfxPoolManager?.SpawnVfx(StringConst.ICEBOMBVFX, gameObject, out vfxIceBomb);
    }

    public override string GetObjectName()
    {
        return StringConst.BOMB_ICE;
    }
}