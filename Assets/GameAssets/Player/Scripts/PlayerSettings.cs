using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new_player_settings", menuName = "Player/ Player Settings")]
public class PlayerSettings : ScriptableObject
{
    [Header("Player")]

    public GameObject PlayerPrefab;

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
