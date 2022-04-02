using Assets.UnityFoundation.Systems.HealthSystem;
using Assets.UnityFoundation.UnityAdapter;
using Moq;
using System;
using UnityEngine;

namespace Assets.GameAssets.Zombies.Tests
{
    public class ZombieControllerTestBuilder
    {
        private ZombieController zombie;
        private ZombieController.Settings settings;
        private GameObject player;

        public Mock<IAnimator> Animator { get; private set; }
        public Mock<IAIBrain> Brain { get; private set; }
        public Mock<IHasHealth> HasHealth { get; private set; }
        public ZombieController.Settings Settings { get { return settings; } }

        public ZombieControllerTestBuilder()
        {
            Animator = new Mock<IAnimator>();
            Brain = new Mock<IAIBrain>();
            HasHealth = new Mock<IHasHealth>();
            settings = new ZombieController.Settings();
        }

        public ZombieControllerTestBuilder With(ZombieController.Settings settings)
        {
            this.settings = settings;
            return this;
        }

        public ZombieController Build()
        {
            zombie = new GameObject("zombie")
                .AddComponent<ZombieController>();

            zombie.Setup(
                settings,
                Animator.Object, 
                Brain.Object, 
                HasHealth.Object,
                new DummyNavMeshAgent(zombie.transform) 
            );

            return zombie;
        }
    }
}