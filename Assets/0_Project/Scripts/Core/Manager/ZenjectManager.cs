using UnityEngine;
using Zenject;

public class ZenjectManager : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<UIManager>().FromComponentInHierarchy().AsSingle();
    }
}
