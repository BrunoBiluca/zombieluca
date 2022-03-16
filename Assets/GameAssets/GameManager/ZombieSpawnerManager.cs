using Assets.GameAssets.Player;
using Assets.GameAssets.Zombies;
using Zenject;

public class ZombieSpawnerManager : IInitializable
{
    private readonly FirstPersonController player;
    public ZombieController[] Zombies { get; }

    public ZombieSpawnerManager(ZombieController[] zombies, FirstPersonController player)
    {
        Zombies = zombies;
        this.player = player;
    }

    public void Initialize()
    {
        foreach(var zombie in Zombies)
            zombie.SetPlayerRef(player.gameObject);
    }
}
