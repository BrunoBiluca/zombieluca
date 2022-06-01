using System;
using UnityEngine;

namespace Assets.GameAssets.Zombies
{
    public partial class ZombieController
    {
        [Serializable]
        public class Settings
        {
            public float WanderingSpeed;
            public float ChasingSpeed;
            public float ChasingTurnSpeed;

            public float AttackRange;
            public float AttackDamage;

            public bool DebugMode;
            public float BaseHealth;
            public GameObject RagdollPrefab;
            public AudioClip[] AttackSFX;

            public AudioClip WanderingSFX;
            public AudioClip[] ChasingSFX;
        }
    }
}