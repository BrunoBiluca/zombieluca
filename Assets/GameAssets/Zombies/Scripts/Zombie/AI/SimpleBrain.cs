using UnityFoundation.Code;
using Assets.UnityFoundation.DebugHelper;
using UnityEngine;
using System;

namespace Assets.GameAssets.Zombies
{
    public class SimpleBrain : IAIBrain
    {
        private readonly Settings settings;
        private GameObject player;
        private float nextAttackTime;

        public bool IsEnabled { get; private set; }
        public bool IsAttacking { get; private set; }
        public bool IsRunning { get; private set; }
        public bool IsWalking { get; private set; }
        public bool IsWandering { get; private set; }
        public bool IsChasing { get; private set; }
        public Transform Body { get; }
        public Optional<Vector3> TargetPosition { get; private set; }
        public bool DebugMode { get; set; }

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
            if(!IsEnabled) return;

            ResetStates();

            if(DebugMode)
                DrawDebug();

            if(IsPlayerInAttackRange())
            {
                if(CanAttack())
                {
                    SetupAttack();
                    return;
                }
                else
                    return;
            }

            if(IsPlayerInChasingRange())
            {
                SetupChasing();
                return;
            }

            SetupWandering();
        }

        private void SetupAttack()
        {
            nextAttackTime = Time.time + settings.MinNextAttackDelay;
            IsAttacking = true;
            TargetPosition = Optional<Vector3>.Some(player.transform.position);
        }

        private void DrawDebug()
        {
            DebugDraw.DrawSphere(Body.position, settings.MinAttackDistance, Color.red);
            DebugDraw.DrawSphere(Body.position, settings.MinDistanceForChasePlayer, Color.blue);
        }

        private void ResetStates()
        {
            IsAttacking = false;
            IsRunning = false;
            IsWalking = false;
            IsWandering = false;
            IsChasing = false;
            TargetPosition = Optional<Vector3>.None();
        }

        private bool IsPlayerInAttackRange()
        {
            var distance = Vector3.Distance(Body.position, player.transform.position);
            return distance <= settings.MinAttackDistance;
        }

        private bool CanAttack()
        {
            if(player == null)
                return false;

            if(Time.time < nextAttackTime)
                return false;

            return true;
        }

        private bool IsPlayerInChasingRange()
        {
            if(player == null)
                return false;

            var distance = Vector3.Distance(Body.position, player.transform.position);
            return distance <= settings.MinDistanceForChasePlayer;
        }

        private void SetupWandering()
        {
            IsWandering = true;
            IsWalking = true;

            if(!TargetPosition.IsPresent)
                EvaluateTargetPosition();

            var distance = Vector3.Distance(Body.position, TargetPosition.Get());
            if(distance.NearlyEqual(0f, 0.5f))
                TargetPosition = Optional<Vector3>.None();
        }

        private void EvaluateTargetPosition()
        {
            var posX = UnityEngine.Random.Range(
                -settings.WanderingDistance, settings.WanderingDistance);
            var posZ = UnityEngine.Random.Range(
                -settings.WanderingDistance, settings.WanderingDistance);

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

        public void Enabled()
        {
            IsEnabled = true;
        }

        public void Disabled()
        {
            IsEnabled = false;
            ResetStates();
            TargetPosition = Optional<Vector3>.None();
        }

        [Serializable]
        public class Settings
        {
            public float MinDistanceForChasePlayer;
            public float WanderingDistance;
            public float MinAttackDistance;
            public float MinNextAttackDelay;
        }
    }
}