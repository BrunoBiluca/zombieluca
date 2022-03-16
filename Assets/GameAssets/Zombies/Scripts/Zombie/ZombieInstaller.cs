using Assets.UnityFoundation.Systems.HealthSystem;
using Assets.UnityFoundation.UnityAdapter;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Assets.GameAssets.Zombies
{
    public class ZombieInstaller : MonoInstaller<ZombieInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<Transform>().FromComponentOnRoot().AsSingle();
            
            Container.Bind<NavMeshAgent>().FromComponentOnRoot().AsSingle();
            Container.Bind<INavMeshAgent>().To<NavMeshAgentDec>().AsSingle();
            Container.Bind<IAIBrain>().To<SimpleBrain>().AsSingle();

            Container.Bind<Animator>().FromComponentOnRoot().AsSingle();
            Container.Bind<IAnimator>().To<AnimatorController>().AsSingle();

            Container.BindInterfacesAndSelfTo<HealthSystem>().FromComponentOnRoot().AsSingle();

            Container.Bind<ZombieController>().FromComponentOnRoot().AsSingle();
        }
    }
}