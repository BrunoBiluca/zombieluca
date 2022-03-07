using Assets.GameAssets.Player.Tests;
using Assets.UnityFoundation.Code.Common;
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
        public void ShouldBeWanderingWhenPlayerIsFarAway()
        {
            const int distanceFromTarget = 5;
            Vector3 targetPosition = new Vector3(distanceFromTarget, 0, 0);

            var zombieTest = new ZombieControllerTestBuilder()
                .With(new ZombieController.Settings() { Speed = 1f });
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
        public void ShouldBeChasingPlayerIfPlayerIsClose()
        {
            var zombieTest = new ZombieControllerTestBuilder()
                .With(new ZombieController.Settings() { Speed = 1f });

            zombieTest.Brain.Setup(b => b.IsChasing).Returns(true);

            var zombie = zombieTest.Build();
            zombie.Update();

            Assert.AreEqual(zombie.ChaseState, zombie.CurrentState);
        }
    }
}