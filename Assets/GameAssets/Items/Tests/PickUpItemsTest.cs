using Assets.GameAssets.AmmoStorage;
using Assets.UnityFoundation.Systems.HealthSystem;
using Moq;
using NUnit.Framework;

namespace Assets.GameAssets.Items.Tests
{
    public class PickUpItemsTest
    {
        [Test]
        [TestCase(0f)]
        [TestCase(10f)]
        public void ShouldRecoverXHealth_WhenUseHealItem(float healAmount)
        {
            var healItem = new HealItem(healAmount);

            var healthSystem = new Mock<IHealable>();

            healthSystem.SetupGet(hs => hs.CurrentHealth).Returns(healAmount);

            healItem.SetHealable(healthSystem.Object).Use();

            healthSystem.Verify(hs => hs.Heal(It.IsAny<float>()), Times.Once);
            Assert.That(healthSystem.Object.CurrentHealth, Is.EqualTo(healAmount));
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void HealItemsShouldBeUsedOnlyOnce(int healUses)
        {
            var healthSystem = new Mock<IHealable>();

            var healItem = new HealItem(10f);

            for(int use = 0; use < healUses; use++)
            {
                healItem.SetHealable(healthSystem.Object).Use();
            }

            healthSystem.Verify(hs => hs.Heal(It.IsAny<float>()), Times.Once);
        }

        [Test]
        public void ShouldRecoverXAmmo_WhenUseAmmoItem()
        {
            const int refillAmount = 5;
            var ammoItem = new AmmoItem(refillAmount);

            var ammoStorage = new Mock<IAmmoStorage>();

            ammoStorage
                .SetupGet(ammoStorage => ammoStorage.CurrentAmount)
                .Returns(refillAmount);

            ammoItem.SetAmmoStorage(ammoStorage.Object).Use();

            ammoStorage.Verify(
                ammoStorage => ammoStorage.Recover(It.IsAny<int>()), 
                Times.Once
            );
            Assert.That(ammoStorage.Object.CurrentAmount, Is.EqualTo(refillAmount));
        }

        [Test]
        public void AmmoItemsShouldBeUsedOnlyOnce()
        {
            var ammoStorage = new Mock<IAmmoStorage>();

            const int refillAmount = 5;
            var ammoItem = new AmmoItem(refillAmount);

            ammoItem.SetAmmoStorage(ammoStorage.Object).Use();
            ammoItem.SetAmmoStorage(ammoStorage.Object).Use();

            ammoStorage.Verify(
                ammoStorage => ammoStorage.Recover(It.IsAny<int>()), 
                Times.Once
            );
        }
    }
}