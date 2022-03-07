using Assets.UnityFoundation.Code;
using Assets.UnityFoundation.Code.Common;
using UnityEngine;

namespace Assets.GameAssets.Zombies
{
    public class SimpleBrain : IAIBrain
    {
        private readonly Settings settings;
        private GameObject player;

        public bool IsAttacking { get; private set; }
        public bool IsRunning { get; private set; }
        public bool IsWalking { get; private set; }
        public bool IsWandering { get; private set; }
        public bool IsChasing { get; private set; }
        public Transform Body { get; }
        public Optional<Vector3> TargetPosition { get; private set; }

        public SimpleBrain(
            Settings settings,
            Transform body
        )
        {
            this.settings = settings;
            Body = body;
            TargetPosition = Optional<Vector3>.None();
        }

        public void SetPlayer(GameObject player)
        {
            this.player = player;
        }

        public void Update()
        {
            ResetStates();
            if(IsPlayerClose())
            {
                SetupChasing();
                return;
            }

            SetupWandering();
        }

        private void ResetStates()
        {
            IsAttacking = false;
            IsRunning = false;
            IsWalking = false;
            IsWandering = false;
            IsChasing = false;
        }

        private bool IsPlayerClose()
        {
            if(player == null)
                return false;

            var distance = Vector3.Distance(Body.transform.position, player.transform.position);
            return distance <= settings.MinDistanceForChasePlayer;
        }

        private void SetupWandering()
        {
            IsWandering = true;
            IsWalking = true;

            if(!TargetPosition.IsPresent)
                EvaluateTargetPosition();

            var distance = Vector3.Distance(Body.transform.position, TargetPosition.Get());
            if(distance.NearlyEqual(0f, 0.5f))
                TargetPosition = Optional<Vector3>.None();
        }

        private void EvaluateTargetPosition()
        {
            var posX = Random.Range(-settings.WanderingDistance, settings.WanderingDistance);
            var posZ = Random.Range(-settings.WanderingDistance, settings.WanderingDistance);

            var target = new Vector3(posX, 0f, posZ);

            if(Terrain.activeTerrain != null)
            {
                var posY = Terrain.activeTerrain.SampleHeight(target);
                target.y = posY;
            }

            TargetPosition = Optional<Vector3>.Some(target);
        }

        private void SetupChasing()
        {
            IsChasing = true;
            IsRunning = true;
            TargetPosition = Optional<Vector3>.Some(player.transform.position);
        }

        public class Settings
        {
            public float MinDistanceForChasePlayer;
            public float WanderingDistance;
        }
    }
}