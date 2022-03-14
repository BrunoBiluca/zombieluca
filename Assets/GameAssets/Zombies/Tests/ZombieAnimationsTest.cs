using Moq;
using NUnit.Framework;
using System;

namespace Assets.GameAssets.Zombies.Tests
{

    public class ZombieAnimationsTest
    {

        [Test]
        public void ShouldPlayIdleAnimationByDefault()
        {
            var zombie = new ZombieControllerTestBuilder();
            zombie.Build();

            zombie.Animator.Verify(a => a.SetBool(ZombieAnimParams.Walking, false));
        }

        [Test]
        public void ShouldPlayWalkingAnimationWhenZombieMove()
        {
            var zombieTest = new ZombieControllerTestBuilder();
            zombieTest.Brain.Setup(b => b.IsWandering).Returns(true);
            zombieTest.Brain.Setup(b => b.IsWalking).Returns(true);

            var zombie = zombieTest.Build();
            zombie.Update();

            zombieTest.Animator.Verify(a => a.SetBool(ZombieAnimParams.Walking, true));
        }

        [Test]
        public void ShouldPlayRunningAnimationWhenZombieRun()
        {
            var zombieTest = new ZombieControllerTestBuilder();
            zombieTest.Brain.Setup(b => b.IsWandering).Returns(true);
            zombieTest.Brain.Setup(b => b.IsRunning).Returns(true);

            var zombie = zombieTest.Build();

            zombie.Update();

            zombieTest.Animator.Verify(a => a.SetBool(ZombieAnimParams.Running, true));
        }

        [Test]
        public void ShouldPlayAttackAnimationWhenZombieAttack()
        {
            var zombieTest = new ZombieControllerTestBuilder();

            var zombie = zombieTest.Build();

            zombieTest.Brain.Setup(b => b.IsChasing).Returns(true);
            zombie.Update();

            zombieTest.Brain.Setup(b => b.IsAttacking).Returns(true);
            zombie.Update();

            zombieTest.Animator.Verify(a => a.SetBool(ZombieAnimParams.Attack, true));
        }

        [Test]
        public void ShouldPlayDeathAnimationWhenZombieDies()
        {
            var zombie = new ZombieControllerTestBuilder();
            zombie.Build();

            zombie.Animator.Verify(a => a.SetTrigger(ZombieAnimParams.Dead), Times.Never);

            zombie.HasHealth.Raise(health => health.OnDied += null, EventArgs.Empty);

            zombie.Animator.Verify(a => a.SetTrigger(ZombieAnimParams.Dead), Times.Once);
        }
    }
}