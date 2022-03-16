using UnityEngine;
using Zenject;

public class ZombieSpawnerInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<ZombieSpawnerManager>()
            .AsSingle();
    }
}