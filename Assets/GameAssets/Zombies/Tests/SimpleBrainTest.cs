using Assets.UnityFoundation.TestUtility;
using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

namespace Assets.GameAssets.Zombies.Tests
{
    public class SimpleBrainTest
    {

        private SimpleBrain.Settings defaultSettings = new SimpleBrain.Settings() {
            MinDistanceForChasePlayer = 10f,
            WanderingDistance = 10f,
            MinAttackDistance = 1f,
            MinNextAttackDelay = .5f
        };

        [TestCase(10f, 10f, 11f)]
        public void ShouldWanderWhenPlayerIsFarAway(
            float wanderingDistance,
            float minDistanceForChasePlayer,
            float playerStartPosition
        )
        {
            var player = new GameObject("player");
            player.transform.position = new Vector3(playerStartPosition, 0, 0);

            var aiBody = new GameObject("AI");
            aiBody.transform.position = new Vector3(0, 0, 0);

            var brainSettings = new SimpleBrain.Settings() {
                MinDistanceForChasePlayer = minDistanceForChasePlayer,
                WanderingDistance = wanderingDistance,
                MinAttackDistance = 1f
            };
            var simpleBrain = new SimpleBrain(brainSettings, aiBody.transform);
            simpleBrain.Enabled();

            simpleBrain.SetPlayer(player);

            simpleBrain.Update();

            Assert.IsTrue(simpleBrain.IsWandering);
            AssertHelper.Between(
                -wanderingDistance, wanderingDistance, simpleBrain.TargetPosition.Get().x);
            AssertHelper.Between(
                -wanderingDistance, wanderingDistance, simpleBrain.TargetPosition.Get().z);
        }

        [Test]
        public void ShouldChaseWhenPlayerIsNear()
        {
            var player = new GameObject("player");
            var playerStartPosition = new Vector3(5, 0, 0);
            player.transform.position = playerStartPosition;

            var aiBody = new GameObject("AI");
            aiBody.transform.position = new Vector3(0, 0, 0);

            var simpleBrain = new SimpleBrain(defaultSettings, aiBody.transform);
            simpleBrain.Enabled();

            simpleBrain.SetPlayer(player);

            simpleBrain.Update();

            Assert.IsTrue(simpleBrain.IsChasing);
            Assert.AreEqual(playerStartPosition, simpleBrain.TargetPosition.Get());
        }

        [Test]
        public void ShouldForgetWhenPlayerIsFar()
        {
            var player = new GameObject("player");
            var playerStartPosition = new Vector3(5, 0, 0);
            player.transform.position = playerStartPosition;

            var aiBody = new GameObject("AI");
            aiBody.transform.position = new Vector3(0, 0, 0);

            var simpleBrain = new SimpleBrain(defaultSettings, aiBody.transform);
            simpleBrain.Enabled();

            simpleBrain.SetPlayer(player);
            simpleBrain.Update();

            Assert.IsTrue(simpleBrain.IsChasing);
            Assert.IsTrue(simpleBrain.TargetPosition.IsPresent);
            Assert.AreEqual(playerStartPosition, simpleBrain.TargetPosition.Get());

            player.transform.position = new Vector3(11f, 0, 0);

            simpleBrain.Update();

            Assert.IsFalse(simpleBrain.IsChasing);
            Assert.IsTrue(simpleBrain.IsWandering);
            Assert.IsTrue(simpleBrain.TargetPosition.IsPresent);

            var distance = defaultSettings.WanderingDistance;
            AssertHelper.Between(-distance, distance, simpleBrain.TargetPosition.Get().x);
            AssertHelper.Between(-distance, distance, simpleBrain.TargetPosition.Get().z);
        }

        [Test]
        public void ShouldAttackWhenPlayerIsInRange()
        {
            var player = new GameObject("player");
            var playerStartPosition = new Vector3(1, 0, 0);
            player.transform.position = playerStartPosition;

            var aiBody = new GameObject("AI");
            aiBody.transform.position = new Vector3(0, 0, 0);

            var simpleBrain = new SimpleBrain(defaultSettings, aiBody.transform);
            simpleBrain.Enabled();

            simpleBrain.SetPlayer(player);
            simpleBrain.Update();

            Assert.IsTrue(simpleBrain.IsAttacking);
            Assert.AreEqual(playerStartPosition, simpleBrain.TargetPosition.Get());
        }

        [UnityTest]
        [RequiresPlayMode]
        public IEnumerator ShouldWaitForXSecondsForSecondAttack()
        {
            var player = new GameObject("player");
            var playerStartPosition = new Vector3(1, 0, 0);
            player.transform.position = playerStartPosition;

            var aiBody = new GameObject("AI");
            aiBody.transform.position = new Vector3(0, 0, 0);

            var simpleBrain = new SimpleBrain(defaultSettings, aiBody.transform);
            simpleBrain.Enabled();

            simpleBrain.SetPlayer(player);
            simpleBrain.Update();

            Assert.IsTrue(simpleBrain.IsAttacking);
            Assert.AreEqual(playerStartPosition, simpleBrain.TargetPosition.Get());

            simpleBrain.Update();

            Assert.IsFalse(simpleBrain.IsAttacking);

            yield return new WaitForSeconds(defaultSettings.MinNextAttackDelay);

            simpleBrain.Update();
            Assert.IsTrue(simpleBrain.IsAttacking);
            Assert.AreEqual(playerStartPosition, simpleBrain.TargetPosition.Get());               
        }

        [Test]
        public void ShouldNotHaveAnyStateWhenDisabled()
        {
            var player = new GameObject("player");
            var playerStartPosition = new Vector3(1, 0, 0);
            player.transform.position = playerStartPosition;

            var aiBody = new GameObject("AI");
            aiBody.transform.position = new Vector3(0, 0, 0);

            var simpleBrain = new SimpleBrain(defaultSettings, aiBody.transform);
            simpleBrain.Enabled();

            simpleBrain.SetPlayer(player);
            simpleBrain.Update();

            simpleBrain.Disabled();

            Assert.IsFalse(simpleBrain.IsChasing);
            Assert.IsFalse(simpleBrain.IsAttacking);
            Assert.IsFalse(simpleBrain.IsRunning);
            Assert.IsFalse(simpleBrain.IsWalking);
            Assert.IsFalse(simpleBrain.IsWandering);
            Assert.IsFalse(simpleBrain.IsEnabled);
        }
    }
}