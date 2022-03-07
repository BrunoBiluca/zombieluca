using Assets.UnityFoundation.Systems.HealthSystem;
using Assets.UnityFoundation.UnityAdapter;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Assets.GameAssets.Zombies
{
    public class ZombieInstaller : MonoInstaller
    {
        [SerializeField] private Transform zombie;
        public override void InstallBindings()
        {
            Container.Bind<Transform>().FromInstance(zombie).AsSingle();

            Container.Bind<NavMeshAgent>()
                .FromComponentInHierarchy()
                .AsSingle();

            Container.Bind<INavMeshAgent>().To<NavMeshAgentDec>().AsSingle();

            Container.Bind<SimpleBrain.Settings>()
                .FromInstance(new SimpleBrain.Settings() { 
                    MinDistanceForChasePlayer = 10f,
                    WanderingDistance = 5f
                })
                .AsSingle();
            Container.Bind<IAIBrain>().To<SimpleBrain>().AsSingle();

            Container.Bind<Animator>().FromComponentInHierarchy().AsSingle();

            Container.Bind<IAnimator>().To<AnimatorController>().AsSingle();

            Container.BindInterfacesAndSelfTo<HealthSystem>()
                .FromComponentsInHierarchy()
                .AsSingle();

            Container.Bind<ZombieController.Settings>()
                .FromInstance(new ZombieController.Settings() { Speed = .45f })
                .AsSingle();

            Container.Bind<ZombieController>()
                .FromComponentInHierarchy()
                .AsSingle();
        }
    }
}