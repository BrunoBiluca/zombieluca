using Assets.GameAssets.AmmoStorageSystem;
using Assets.UnityFoundation.Systems.HealthSystem;
using Moq;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.TestTools;

namespace Assets.GameAssets.Items.Tests
{
    [TestFixture]
    public class PickUpItemsTest
    {
        public Mock<IHealable> GetHealable(
            float baseHealth, 
            float current, 
            Action<float> healCallback
        )
        {
            var healthSystem = new Mock<IHealable>();
            healthSystem.Setup(hs => hs.CurrentHealth).Returns(current);
            healthSystem.Setup(hs => hs.BaseHealth).Returns(baseHealth);

            healthSystem.Setup(hs => hs.Heal(It.IsAny<float>()))
                .Callback<float>(amount => healCallback(amount));

            return healthSystem;
        }


        [Test]
        [TestCase(0f)]
        [TestCase(10f)]
        public void ShouldRecoverXHealth_WhenUseHealItem(float healAmount)
        {
            var healItem = new HealItem(healAmount);
            var currentHealthExpected = 0f;
            var healthSystem = GetHealable(10f, 1f, (amount) => currentHealthExpected += amount);

            healItem.SetHealable(healthSystem.Object).Use();

            healthSystem.Verify(hs => hs.Heal(It.IsAny<float>()), Times.Once);
            Assert.That(currentHealthExpected, Is.EqualTo(healAmount));
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void HealItemsShouldBeUsedOnlyOnce(int healUses)
        {
            var currentHealthExpected = 0f;
            var healthSystem = GetHealable(10f, 1f, (amount) => currentHealthExpected += amount);

            var healItem = new HealItem(10f);
            for(int use = 0; use < healUses; use++)
            {
                healItem.SetHealable(healthSystem.Object).Use();
            }

            healthSystem.Verify(hs => hs.Heal(It.IsAny<float>()), Times.Once);
            Assert.That(currentHealthExpected, Is.EqualTo(10f));
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
                ammoStorage => ammoStorage.Recover(It.IsAny<uint>()),
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
                ammoStorage => ammoStorage.Recover(It.IsAny<uint>()),
                Times.Once
            );
        }

        [UnityTest]
        [RequiresPlayMode]
        [TestCaseSource(nameof(MockConsumableItems))]
        public IEnumerator ItemXBehaviour_WhenCollideWithY(
            IConsumableItemFactory itemFactory,
            IItemUserFactory itemUserFactory,
            bool expectedConsume
        )
        {
            var collision = new Collision();
            var field = typeof(Collision).GetField(
                "m_Body", BindingFlags.Instance | BindingFlags.NonPublic
            );
            field.SetValue(collision, itemUserFactory.Create()); // Set non-public field

            var item = itemFactory.Create();
            item.OnCollisionEnter(collision);

            Assert.AreEqual(expectedConsume, itemFactory.GetConsumableItem().WasConsumed);

            itemFactory.Destroy();
            itemUserFactory.Destroy();

            yield return null;
        }

        public static IEnumerable<TestCaseData> MockConsumableItems()
        {
            yield return new TestCaseData(new AmmoItemFactory(), new StorageFactory(), true)
                .SetName("Ammo Item should be used when collide with Ammo Storage")
                .Returns(null);

            yield return new TestCaseData(
                    new AmmoItemFactory(), new StorageFactory().Full(), false
                )
                .SetName("Ammo Item should not be used when collide with Ammo Storage maxed")
                .Returns(null);

            yield return new TestCaseData(new AmmoItemFactory(), new HealthSystemFactory(), false)
                .SetName("Ammo Item should not be used when collide with Health System")
                .Returns(null);

            yield return new TestCaseData(new HealItemFactory(), new StorageFactory(), false)
                .SetName("Heal item should not be used when collide with Ammo Storage")
                .Returns(null);

            yield return new TestCaseData(new HealItemFactory(), new HealthSystemFactory(), true)
                .SetName("Heal item should be used when collide with Healable")
                .Returns(null);

            yield return new TestCaseData(
                    new HealItemFactory(), new HealthSystemFactory().Full(), false
                )
                .SetName("Heal item should not be used when collide with Healable full")
                .Returns(null);
        }
    }
}