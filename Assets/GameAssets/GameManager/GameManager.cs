using Assets.GameAssets.Player;
using Cinemachine;
using UnityEngine;
using Zenject;

public class GameManager : IInitializable
{
    private readonly FirstPersonController player;
    private readonly CinemachineVirtualCamera vCamera;
    private readonly CursorLockHandler cursorLockHandler;

    public GameManager(
        FirstPersonController player,
        CinemachineVirtualCamera vCamera,
        CursorLockHandler cursorLockHandler
    ) {
        this.player = player;
        this.vCamera = vCamera;
        this.cursorLockHandler = cursorLockHandler;
    }

    public void Initialize()
    {
        cursorLockHandler.Enable();

        vCamera.Follow = player.transform;

        var spawnPoint = GameObject.Find("player_spawn_point");
        player.transform.position = spawnPoint.transform.position;
    }
}
