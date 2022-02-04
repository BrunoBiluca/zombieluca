using Assets.GameAssets.Items;
using Cinemachine;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [Inject]
    private PlayerSettings playerSettings;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<CursorLockHandler>().AsSingle();
        Container.BindInterfacesAndSelfTo<GameManager>().AsSingle().NonLazy();

        // TODO: criar subcontaine para o player, 
        // encapsulando assim todas as instancias de um player 
        // possibilitando um coop split/screen
        Container.Bind<Camera>().FromComponentInHierarchy().AsCached();
        Container.Bind<CinemachineVirtualCamera>().FromComponentInHierarchy().AsCached();

        Container.Bind<FirstPersonInputs>().AsSingle();
        Container.BindInterfacesAndSelfTo<FirstPersonInputActions>().AsSingle();

        Container.Bind<FirstPersonController>()
            .FromSubContainerResolve()
            .ByNewPrefabInstaller<PlayerInstaller>(playerSettings.PlayerPrefab)
            .AsCached();

        Container.Bind<HealItem>().AsTransient().WithArguments(10f);

        Container.Bind<AmmoItem>().AsTransient().WithArguments(5);
    }
}