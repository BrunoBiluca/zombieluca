using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.GameAssets.ZombiePong.Tests
{
    public class SimplePongAITest
    {
        [Test]
        [TestCase(0f, 5f, 1f)]
        [TestCase(0f, -5f, -1f)]
        [TestCase(5f, 0f, -1f)]
        [TestCase(-5f, 0f, 1f)]
        public void ShouldMoveRelatedToCurrentBallPosition(
            float paddlePosZ, float ballPosZ, float inputExpected
        )
        {
            var testBuilder = new SimplePongAITestBuilder()
                .WithBall(ballPosZ)
                .WithPaddle(paddlePosZ);
            var pongAI = testBuilder.Build();

            Assert.That(pongAI.Move, Is.EqualTo(new Vector2(0f, inputExpected)));
        }

        [Test]
        public void ShouldNotMoveWhenBallIsDestroyed()
        {
            var testBuilder = new SimplePongAITestBuilder().WithBall(5f).WithPaddle(0f);
            var pongAI = testBuilder.Build();

            Object.DestroyImmediate(testBuilder.Ball.gameObject);

            Assert.That(pongAI.Move, Is.EqualTo(Vector2.zero));
        }

        [Test]
        public void ShouldMoveRelatedToNewBallPositionWhenBallChange()
        {
            var testBuilder = new SimplePongAITestBuilder().WithBall(5f).WithPaddle(0f);
            var pongAI = testBuilder.Build();

            Assert.That(pongAI.Move, Is.EqualTo(new Vector2(0f, 1f)));

            Object.DestroyImmediate(testBuilder.Ball.gameObject);

            Assert.That(pongAI.Move, Is.EqualTo(Vector2.zero));

            var ball = new GameObject("ball_2").AddComponent<BallHandler>();
            ball.transform.position = new Vector3(0f, 0f, -5f);

            pongAI.SetBall(ball.transform);

            Assert.That(pongAI.Move, Is.EqualTo(new Vector2(0f, -1f)));
        }

        public class SimplePongAITestBuilder
        {
            public BallHandler Ball { get; private set; }
            public PaddleHandler Paddle { get; private set; }
            public SimplePongAI AI { get; private set; }

            private float ballPosZ;
            private float paddlePosZ;

            public SimplePongAITestBuilder WithBall(float positionZ)
            {
                ballPosZ = positionZ;
                return this;
            }

            public SimplePongAITestBuilder WithPaddle(float positionZ)
            {
                paddlePosZ = positionZ;
                return this;
            }

            public SimplePongAI Build()
            {
                Ball = new GameObject("ball").AddComponent<BallHandler>();
                Ball.transform.position = new Vector3(0f, 0f, ballPosZ);

                Paddle = new GameObject("paddle").AddComponent<PaddleHandler>();
                Paddle.transform.position = new Vector3(0f, 0f, paddlePosZ);

                AI = new SimplePongAI(Paddle.transform, Ball.transform);

                Paddle.Setup(AI);

                return AI;
            }
        }
    }
}