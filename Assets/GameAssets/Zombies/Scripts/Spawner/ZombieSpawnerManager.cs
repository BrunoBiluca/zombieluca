using UnityEngine;
using Zenject;

namespace Assets.GameAssets.Zombies
{
    public class ZombieSpawnerManager : IInitializable
    {
        private readonly GameObject player;
        public ZombieController[] Zombies { get; }

        public ZombieSpawnerManager(ZombieController[] zombies, GameObject player)
        {
            Zombies = zombies;
            this.player = player;
        }

        public void Initialize()
        {
            foreach(var zombie in Zombies)
                zombie.SetPlayerRef(player);
        }
    }
}