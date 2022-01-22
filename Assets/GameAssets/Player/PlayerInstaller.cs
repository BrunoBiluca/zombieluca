using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerInstaller : Installer<PlayerInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<FirstPersonController>().FromComponentOnRoot().AsSingle();

        Container.Bind<IdlePlayerState>().AsTransient();
        Container.Bind<WalkPlayerState>().AsTransient();

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

        Container.Bind<FirstPersonAnimationController>().AsSingle();
    }
}
