using Assets.GameAssets.Player;
using Assets.GameAssets.Zombies;
using Zenject;

public class ZombieSpawnerManager : IInitializable
{
    private readonly ZombilucaPlayer player;
    public ZombieController[] Zombies { get; }

    public ZombieSpawnerManager(ZombieController[] zombies, ZombilucaPlayer player)
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
