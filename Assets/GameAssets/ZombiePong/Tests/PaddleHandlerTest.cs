using Assets.UnityFoundation.TestUtility;
using NUnit.Framework;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace Assets.GameAssets.ZombiePong.Tests
{
    public class PaddleHandlerTest : InputTestFixture
    {
        [TestCaseSource(nameof(KeyBoardTestCases))]
        public void ShouldMoveInFourDirectionsAccordingPlayerInputs(
            string key, float paddleSpeed, Vector3 expected
        )
        {
            var paddleInputs = new PaddleInputs(new PongInputActions());
            paddleInputs.Enable();

            var paddle = new GameObject("paddle")
                .AddComponent<PaddleHandler>()
                .Setup(paddleInputs);

            paddle.PaddleSpeed = paddleSpeed;
            paddle.Awake();

            var keyboard = InputSystem.AddDevice<Keyboard>();
            Press(GetButtonControl(keyboard, key));
            paddle.Update();
            Release(GetButtonControl(keyboard, key));

            AssertHelper.AreEqual(expected * Time.deltaTime, paddle.transform.position);
        }

        private ButtonControl GetButtonControl(Keyboard keyBoard, string name)
        {
            return name switch {
                "wKey" => keyBoard.wKey,
                "sKey" => keyBoard.sKey,
                "aKey" => keyBoard.aKey,
                "dKey" => keyBoard.dKey,
                _ => throw new ArgumentException($"Key: {name} doesn't exist in keyboard"),
            };
        }

        private static IEnumerable KeyBoardTestCases()
        {
            var paddleSpeed = 1f;

            yield return new TestCaseData("wKey", paddleSpeed, new Vector3(0, 0, paddleSpeed))
                .SetName("Player inputs wKey");

            yield return new TestCaseData("sKey", paddleSpeed, new Vector3(0, 0, -paddleSpeed))
                .SetName("Player inputs sKey");

            yield return new TestCaseData("aKey", paddleSpeed, new Vector3(-paddleSpeed, 0, 0f))
                .SetName("Player inputs aKey");

            yield return new TestCaseData("dKey", paddleSpeed, new Vector3(paddleSpeed, 0, 0))
                .SetName("Player inputs dKey");

        }
    }
}