using UnityEngine;
using Zenject;

public class ZenjectManager : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<GameManager>().FromComponentInHierarchy().AsSingle();
        Container.Bind<UIManager>().FromComponentInHierarchy().AsSingle();
        Container.Bind<GoldManager>().FromComponentInHierarchy().AsSingle();
    }
}
