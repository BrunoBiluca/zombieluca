using NUnit.Framework;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.TestTools;

namespace Assets.GameAssets.ZombiePong.Tests
{
    public class PongGameManagerTest
    {
        [UnityTest]
        [RequiresPlayMode]
        public IEnumerator ShouldScoreGoalsOnProperPlayer()
        {
            var gameManager = new GameObject("pongo_game_manager")
                .AddComponent<PongGameManager>();

            yield return null;

            PongGameManager.Instance.AddScore(0);
            PongGameManager.Instance.AddScore(0);
            PongGameManager.Instance.AddScore(0);
            PongGameManager.Instance.AddScore(1);

            Assert.AreEqual(3, PongGameManager.Instance.GetScore(0));
            Assert.AreEqual(1, PongGameManager.Instance.GetScore(1));

            Object.Destroy(gameManager);
        }

        [UnityTest]
        [RequiresPlayMode]
        public IEnumerator ShouldUpdateUIWhenPlayerScores()
        {
            var gameManager = new GameObject("pong_game_manager")
                .AddComponent<PongGameManager>();

            var gameUI = new GameObject("pong_canvas");

            var left = new GameObject("paddle_left_score_text").AddComponent<TextMeshProUGUI>();
            left.transform.SetParent(gameUI.transform);
            var right = new GameObject("paddle_right_score_text").AddComponent<TextMeshProUGUI>();
            right.transform.SetParent(gameUI.transform);

            gameUI.AddComponent<PongUI>();

            yield return null;

            PongGameManager.Instance.AddScore(0);

            Assert.AreEqual("1", left.text);
            Assert.AreEqual("0", right.text);

            PongGameManager.Instance.AddScore(1);

            Assert.AreEqual("1", left.text);
            Assert.AreEqual("1", right.text);

            Object.Destroy(gameManager);
        }
    }
}