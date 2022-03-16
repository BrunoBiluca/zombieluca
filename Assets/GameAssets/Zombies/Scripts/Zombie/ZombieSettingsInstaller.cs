using UnityEngine;
using Zenject;

namespace Assets.GameAssets.Zombies
{
    [CreateAssetMenu(fileName = "new_zombie_settings", menuName = "Zombies/Zombie settings")]
    public class ZombieSettingsInstaller : ScriptableObjectInstaller<ZombieSettingsInstaller>
    {
        public SimpleBrain.Settings zombieBrainSettings;
        public ZombieController.Settings zombieSettings;

        public override void InstallBindings()
        {
            Container.BindInstance(zombieBrainSettings);
            Container.BindInstance(zombieSettings);
        }
    }
}