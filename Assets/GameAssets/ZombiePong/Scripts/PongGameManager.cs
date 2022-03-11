using UnityFoundation.Code;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.GameAssets.ZombiePong
{
    public class PongGameManager : Singleton<PongGameManager>
    {
        public GameObject BallPrefab;

        private readonly List<int> scores = new List<int>();

        public Action<int, int> OnPlayerScores;

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
                var paddleInputs = new PaddleInputs(new PongInputActions());
                paddle.Setup(paddleInputs);
            }
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

            Instantiate(BallPrefab, new Vector3(0, .5f, 0f), Quaternion.identity);
        }

        public int GetScore(int playerIndex)
        {
            return scores[playerIndex];
        }
    }
}