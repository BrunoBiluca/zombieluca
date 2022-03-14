using UnityEngine;
using Zenject;

namespace Assets.GameAssets.Zombies
{


    public class ZombieSpawnerInstaller : MonoInstaller
    {
        [SerializeField] private GameObject player;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ZombieSpawnerManager>()
                .AsSingle()
                .WithArguments(player);
        }
    }
}