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

            Container.Bind<SimpleBrain.Settings>()
                .FromInstance(new SimpleBrain.Settings() { 
                    MinDistanceForChasePlayer = 20f,
                    WanderingDistance = 5f,
                    MinAttackDistance = 1f
                })
                .AsSingle();
            Container.Bind<IAIBrain>().To<SimpleBrain>().AsSingle();

            Container.Bind<Animator>().FromComponentOnRoot().AsSingle();
            Container.Bind<IAnimator>().To<AnimatorController>().AsSingle();

            Container.BindInterfacesAndSelfTo<HealthSystem>().FromComponentOnRoot().AsSingle();

            Container.Bind<ZombieController.Settings>()
                .FromInstance(new ZombieController.Settings() {
                    WanderingSpeed = .45f,
                    ChasingSpeed = 4f,
                    ChasingTurnSpeed = 10f
                })
                .AsSingle();

            Container.Bind<ZombieController>().FromComponentOnRoot().AsSingle();
        }
    }
}