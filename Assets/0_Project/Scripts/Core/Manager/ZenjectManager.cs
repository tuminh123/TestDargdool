using UnityEngine;
using Zenject;

public class ZenjectManager : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<UIManager>().FromComponentInHierarchy().AsSingle();
        Container.Bind<ObjInGamePoolManager>().FromComponentInHierarchy().AsSingle();
        Container.Bind<VfxPoolManager>().FromComponentInHierarchy().AsSingle();
        Container.Bind<ItemPoolManager>().FromComponentInHierarchy().AsSingle();
        Container.Bind<ProjectilePoolManager>().FromComponentInHierarchy().AsSingle();
        Container.Bind<TimeSlow>().FromComponentInHierarchy().AsSingle();
        Container.Bind<CameraShaker>().FromComponentInHierarchy().AsSingle();
    }
}
