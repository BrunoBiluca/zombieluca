using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new_player_settings", menuName = "Zombieluca/Player Settngs")]
public class ZombilucaPlayerSettings : ScriptableObject
{
    [Header("Player")]

    public GameObject PlayerPrefab;
    public GameObject PlayerFullModel;

    public float StartHealth = 20f;
}
