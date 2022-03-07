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

            zombie.Animator.Verify(a => a.SetBool(ZombiesAnimParams.Walking, false));
        }

        [Test]
        public void ShouldPlayWalkingAnimationWhenZombieMove()
        {
            var zombieTest = new ZombieControllerTestBuilder();
            zombieTest.Brain.Setup(b => b.IsWandering).Returns(true);
            zombieTest.Brain.Setup(b => b.IsWalking).Returns(true);

            var zombie = zombieTest.Build();
            zombie.Update();

            zombieTest.Animator.Verify(a => a.SetBool(ZombiesAnimParams.Walking, true));
        }

        [Test]
        public void ShouldPlayRunningAnimationWhenZombieRun()
        {
            var zombieTest = new ZombieControllerTestBuilder();
            zombieTest.Brain.Setup(b => b.IsWandering).Returns(true);
            zombieTest.Brain.Setup(b => b.IsRunning).Returns(true);

            var zombie = zombieTest.Build();

            zombie.Update();

            zombieTest.Animator.Verify(a => a.SetBool(ZombiesAnimParams.Running, true));
        }

        [Test]
        public void ShouldPlayAttackAnimationWhenZombieAttack()
        {
            var zombieTest = new ZombieControllerTestBuilder();
            zombieTest.Brain.Setup(b => b.IsAttacking).Returns(true);

            var zombie = zombieTest.Build();

            zombie.Update();

            zombieTest.Animator.Verify(a => a.SetTrigger(ZombiesAnimParams.Attack));
        }

        [Test]
        public void ShouldPlayDeathAnimationWhenZombieDies()
        {
            var zombie = new ZombieControllerTestBuilder();
            zombie.Build();

            zombie.Animator.Verify(a => a.SetTrigger(ZombiesAnimParams.Dead), Times.Never);

            zombie.HasHealth.Raise(health => health.OnDied += null, EventArgs.Empty);

            zombie.Animator.Verify(a => a.SetTrigger(ZombiesAnimParams.Dead), Times.Once);
        }
    }
}