using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.GameAssets.FirstPersonModeSystem
{
    [CreateAssetMenu(
        fileName = "new_first_person_settings",
        menuName = "First Person Mode/Settings"
    )]
    public class FirstPersonModeSettings : ScriptableObject
    {
        [Tooltip("Move speed of the character in m/s")]
        public float MoveSpeed = 2.0f;

        [Space(10)]
        [Tooltip("The height the player can jump")]
        public float JumpHeight = 1.2f;

        [Space(10)]
        [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
        public float JumpTimeout = 0.50f;

        public AudioClip FireSFX;
        public AudioClip FireMissSFX;

        public List<AudioClip> WalkingStepsSFX;

        public AudioClip JumpAudioClip;

        public AudioClip LandAudioClip;
    }
}