using UnityFoundation.Code;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.GameAssets.ZombiePong
{
    public class PongGameManager : Singleton<PongGameManager>
    {
        public bool SpawnBallOnStart = true;
        public GameObject BallPrefab;
        public Action<int, int> OnPlayerScores;

        private Transform ballInstance;
        private readonly List<int> scores = new List<int>();
        private SimplePongAI pongAI;

        protected override void OnAwake()
        {
            var playerCount = 2;
            for(int i = 0; i < playerCount; i++)
            {
                scores.Add(0);
            }
        }

        public void Start()
        {
            var paddles = FindObjectsOfType<PaddleHandler>();
            foreach(var paddle in paddles)
            {
                if(paddle.IsLeft)
                {
                    var paddleInputs = new PaddleInputs(new PongInputActions());
                    paddle.Setup(paddleInputs);
                }
                else
                {
                    pongAI = new SimplePongAI(paddle.transform, null);
                    paddle.Setup(pongAI);
                }
            }

            if(SpawnBallOnStart)
                StartCoroutine(nameof(SpawnBall));
        }

        public void AddScore(int playerIndex)
        {
            scores[playerIndex]++;

            OnPlayerScores?.Invoke(playerIndex, scores[playerIndex]);

            StartCoroutine(nameof(SpawnBall));
        }

        private IEnumerator SpawnBall()
        {
            yield return new WaitForSeconds(3);

            ballInstance = Instantiate(BallPrefab, new Vector3(0, .5f, 0f), Quaternion.identity)
                .transform;

            pongAI.SetBall(ballInstance);
        }

        public int GetScore(int playerIndex)
        {
            return scores[playerIndex];
        }
    }
}