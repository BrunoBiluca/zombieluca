using UnityFoundation.Code.PhysicsUtils;
using Assets.UnityFoundation.Systems.HealthSystem;
using Assets.UnityFoundation.UnityAdapter;
using UnityEngine;
using Zenject;
using Assets.GameAssets.FirstPersonModeSystem;
using Assets.GameAssets.AmmoStorageSystem;

namespace Assets.GameAssets.Player
{
    public class PlayerInstaller : Installer<PlayerInstaller>
    {
        public override void InstallBindings()
        {
            // TODO: instalar os componentes no prefab pelo installer, não ter no prefab

            Container.Bind<Transform>().FromComponentOnRoot().AsSingle();
            Container.Bind<ZombilucaPlayer>().FromComponentOnRoot().AsSingle();

            Container.BindInterfacesAndSelfTo<CheckGroundHandler>()
                .AsCached()
                .WithArguments(0.5f);

            Container.Bind<FirstPersonMode>().FromComponentOnRoot().AsSingle();

            Container.Bind<AudioSource>()
                .WithId(AudioSources.PlayerWeapon)
                .FromComponentOnRoot()
                .AsCached();

            Container.Bind<AudioSource>()
                .WithId(AudioSources.PlayerMovement)
                .FromComponentOnRoot()
                .AsCached();

            Container.Bind<Animator>()
                .FromComponentInHierarchy()
                .AsCached();

            Container.BindInterfacesAndSelfTo<HealthSystem>().AsSingle();

            Container.BindInterfacesAndSelfTo<AmmoStorage>()
                .AsSingle()
                .WithArguments(10u);

            Container.Bind<IAnimator>().To<AnimatorController>().AsCached();

            Container.Bind<FirstPersonAnimationController>().AsSingle();
        }
    }
}