using Cinemachine;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<FirstPersonController>().FromComponentInHierarchy().AsCached();

        Container.Bind<Camera>().FromComponentInHierarchy().AsCached();

        Container.Bind<FirstPersonInputs>().FromNew().AsSingle();

        Container.BindInterfacesAndSelfTo<FirstPersonInputActions>().FromNew().AsSingle();
    }
}