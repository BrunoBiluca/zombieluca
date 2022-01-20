using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<CursorLockHandler>().AsSingle();
        Container.BindInterfacesAndSelfTo<GameManager>().AsSingle().NonLazy();

        // TODO: criar subcontaine para o player, 
        // encapsulando assim todas as instancias de um player 
        // possibilitando um coop split/screen
        Container.Bind<Camera>().FromComponentInHierarchy().AsCached();

        Container.Bind<IdlePlayerState>().AsTransient();
        Container.Bind<WalkPlayerState>().AsTransient();

        Container.Bind<FirstPersonInputs>().FromNew().AsSingle();

        // TODO: FromComponentInHierarchy só funciona porque só existe 1 na cena
        // mudar isso para garantir que funcione com mais Animators
        // talvez será necessário criar um prefab para o player
        Container.Bind<FirstPersonController>().FromComponentInHierarchy().AsCached();
        Container.Bind<Animator>().FromComponentInHierarchy().AsCached();
        Container.Bind<AudioSource>().FromComponentInHierarchy().AsCached();

        Container.Bind<FirstPersonAnimationController>().AsSingle();
        Container.BindInterfacesAndSelfTo<FirstPersonInputActions>().AsSingle();
    }
}