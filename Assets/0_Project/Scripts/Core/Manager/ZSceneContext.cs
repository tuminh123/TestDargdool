using UnityEngine;
using Zenject;

public class ZSceneContext : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<ItemPoolManager>().FromComponentInHierarchy().AsSingle();
        Container.Bind<VfxPoolManager>().FromComponentInHierarchy().AsSingle();
    }
}
