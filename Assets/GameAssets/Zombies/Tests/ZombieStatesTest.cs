using UnityFoundation.Code;
using Assets.UnityFoundation.TestUtility;
using NUnit.Framework;
using UnityEngine;
using System;
using Assets.UnityFoundation.Systems.HealthSystem;
using Moq;

namespace Assets.GameAssets.Zombies.Tests
{

    public class DamageableMono : MonoBehaviour, IDamageable
    {
        IDamageable damageable;

        public DamageableLayer Layer => throw new NotImplementedException();

        public float BaseHealth => damageable.BaseHealth;

        public float CurrentHealth => damageable.CurrentHealth;

        public bool IsDead => damageable.IsDead;

        public bool DestroyOnDied { 
            get => damageable.DestroyOnDied; 
            set => damageable.DestroyOnDied = value; 
        }

        #pragma warning disable CS0067 // Rethrow to preserve stack details
        public event EventHandler OnTakeDamage;
        public event EventHandler OnFullyHeal;
        public event EventHandler OnDied;
        #pragma warning restore CS0067 // Rethrow to preserve stack details

        public void Damage(float amount, DamageableLayer layer = null)
        {
            damageable.Damage(amount, layer);
        }

        public void Setup(IDamageable damageable)
        {
            this.damageable = damageable;
        }

        public void Setup(float baseHealth)
        {
            damageable.Setup(baseHealth);
        }
    }

    public class ZombieStatesTest
    {
        [Test]
        public void ShouldBeIdleStateByDefault()
        {
            var zombie = new ZombieControllerTestBuilder().Build();

            Assert.AreEqual(zombie.IdleState, zombie.CurrentState);
        }

        [Test]
        public void ShouldBeWanderingWhenBrainIsWandering()
        {
            const int distanceFromTarget = 5;
            Vector3 targetPosition = new Vector3(distanceFromTarget, 0, 0);

            var zombieTest = new ZombieControllerTestBuilder()
                .With(new ZombieController.Settings() { WanderingSpeed = 1f });
            zombieTest.Brain.Setup(b => b.IsWandering).Returns(true);
            zombieTest.Brain.Setup(b => b.TargetPosition)
                .Returns(Optional<Vector3>.Some(targetPosition));

            var zombie = zombieTest.Build();

            zombie.Update();

            Assert.AreEqual(zombie.WanderState, zombie.CurrentState);

            for(int i = 0; i < distanceFromTarget; i++)
                zombie.Agent.Update();

            AssertHelper.AreEqual(targetPosition, zombie.transform.position);
        }

        [Test]
        public void ShouldBeChasingPlayerIfBrainIsChasing()
        {
            var zombieTest = new ZombieControllerTestBuilder()
                .With(new ZombieController.Settings() { WanderingSpeed = 1f });

            zombieTest.Brain.Setup(b => b.IsChasing).Returns(true);

            var zombie = zombieTest.Build();
            zombie.Update();

            Assert.AreEqual(zombie.ChaseState, zombie.CurrentState);
        }

        [Test]
        public void ShouldBeNotBeChasingPlayerIfBrainIsNotChasing()
        {
            var zombieTest = new ZombieControllerTestBuilder()
                .With(new ZombieController.Settings() { WanderingSpeed = 1f });
            zombieTest.Brain.Setup(b => b.TargetPosition)
                .Returns(Optional<Vector3>.Some(new Vector3()));

            zombieTest.Brain.Setup(b => b.IsChasing).Returns(true);

            var zombie = zombieTest.Build();
            zombie.Update();

            Assert.AreEqual(zombie.ChaseState, zombie.CurrentState);

            zombieTest.Brain.Setup(b => b.IsChasing).Returns(false);
            zombie.Update();

            Assert.AreNotEqual(zombie.ChaseState, zombie.CurrentState);
        }

        [Test]
        public void ShouldBeAttackingWhenBrainIsAttackingAfterChasePlayer()
        {
            var zombieTest = new ZombieControllerTestBuilder()
                .With(new ZombieController.Settings() { WanderingSpeed = 1f });
            zombieTest.Brain.Setup(b => b.TargetPosition)
                .Returns(Optional<Vector3>.Some(new Vector3()));

            zombieTest.Brain.Setup(b => b.IsChasing).Returns(true);
            var zombie = zombieTest.Build();
            zombie.Update();

            Assert.AreEqual(zombie.ChaseState, zombie.CurrentState);

            zombieTest.Brain.Setup(b => b.IsAttacking).Returns(true);
            zombie.Update();

            Assert.AreEqual(zombie.AttackState, zombie.CurrentState);
        }

        [Test]
        [Description("Damage behaviour when zombie is attacking")]
        [TestCase(1f, true, Description = "Should inflict damage")]
        [TestCase(1.5f, false, Description = "Should not inflict damage")]
        public void DamageBehaviourWhenTargetInAttack(float playerPos, bool isDamaging)
        {
            var player = new GameObject("player");
            player.transform.position = new Vector3(playerPos, 0, 0);

            var mockDamageable = new Mock<IDamageable>();
            player.AddComponent<DamageableMono>().Setup(mockDamageable.Object);

            var zombieTest = new ZombieControllerTestBuilder()
                .With(new ZombieController.Settings() { 
                    AttackRange = 1f,
                    AttackDamage = 1f
                });

            zombieTest.Brain.Setup(b => b.Target)
                .Returns(Optional<Transform>.Some(player.transform));

            zombieTest.Brain.Setup(b => b.IsAttacking).Returns(true);

            var zombie = zombieTest.Build();
            zombie.TransitionToState(zombie.AttackState);

            zombie.TriggerAnimationEvent("attack");

            if(isDamaging)
                mockDamageable.Verify(d => 
                    d.Damage(zombieTest.Settings.AttackDamage, null), Times.Once()
                );
            else
                mockDamageable.Verify(d => 
                    d.Damage(It.IsAny<float>(), null), Times.Never()
                );

            UnityEngine.Object.DestroyImmediate(player);
            UnityEngine.Object.DestroyImmediate(zombie);
        }

        [Test]
        public void ShouldNotBeAttackingOnlyWhenAttacksFinished()
        {
            var zombieTest = new ZombieControllerTestBuilder()
                .With(new ZombieController.Settings() { WanderingSpeed = 1f });
            zombieTest.Brain.Setup(b => b.TargetPosition)
                .Returns(Optional<Vector3>.Some(new Vector3()));

            zombieTest.Brain.Setup(b => b.IsChasing).Returns(true);
            var zombie = zombieTest.Build();
            zombie.Update();

            Assert.AreEqual(zombie.ChaseState, zombie.CurrentState);

            zombieTest.Brain.Setup(b => b.IsAttacking).Returns(true);
            zombie.Update();

            Assert.AreEqual(zombie.AttackState, zombie.CurrentState);

            zombie.Update();
            zombie.Update();
            zombie.Update();

            Assert.AreEqual(zombie.AttackState, zombie.CurrentState);

            zombie.TriggerAnimationEvent(AttackZombieState.TriggerEvents.AttackFinished);

            Assert.AreNotEqual(zombie.AttackState, zombie.CurrentState);
        }
    }
}