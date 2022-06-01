using Assets.GameAssets.Player;
using Assets.GameAssets.Zombies;
using Zenject;

namespace Assets.GameAssets.GameManager
{
    public class OnZombiesDied { }

    public class ZombieSpawnerManager : IInitializable
    {
        private readonly ZombilucaPlayer player;
        private readonly SignalBus signalBus;

        public ZombieController[] Zombies { get; }

        public int currentZombieDeadCount = 0;
        public int ZombieCount { get; private set; }

        public ZombieSpawnerManager(
            ZombieController[] zombies,
            ZombilucaPlayer player,
            SignalBus signalBus
        )
        {
            Zombies = zombies;
            ZombieCount = zombies.Length;
            this.player = player;
            this.signalBus = signalBus;
        }

        public void Initialize()
        {
            foreach(var zombie in Zombies)
            {
                zombie.SetPlayerRef(player.gameObject);
                zombie.Health.OnDied += (sender, args) => {
                    currentZombieDeadCount++;
                    if(currentZombieDeadCount == ZombieCount)
                        signalBus.Fire<OnZombiesDied>();
                };
            }

        }
    }
}