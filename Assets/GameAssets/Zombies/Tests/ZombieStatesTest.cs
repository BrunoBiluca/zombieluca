using UnityFoundation.Code;
using Assets.UnityFoundation.TestUtility;
using NUnit.Framework;
using UnityEngine;

namespace Assets.GameAssets.Zombies.Tests
{
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