using Assets.GameAssets.Player;
using Assets.UnityFoundation.Systems.HealthSystem;
using Assets.UnityFoundation.UI.Menus.GameOverMenu;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class GameManager : IInitializable
{
    private readonly FirstPersonController player;
    private readonly CinemachineVirtualCamera vCamera;
    private readonly CursorLockHandler cursorLockHandler;
    private readonly GameOverMenu gameOverMenu;

    public GameManager(
        FirstPersonController player,
        CinemachineVirtualCamera vCamera,
        CursorLockHandler cursorLockHandler,
        GameOverMenu gameOverMenu
    )
    {
        this.player = player;
        this.vCamera = vCamera;
        this.cursorLockHandler = cursorLockHandler;
        this.gameOverMenu = gameOverMenu;
    }

    public void Initialize()
    {
        cursorLockHandler.Enable();

        vCamera.Follow = player.transform.Find("camera_reference");

        var spawnPoint = GameObject.Find("player_spawn_point");
        player.transform.position = spawnPoint.transform.position;

        player.GetComponent<IHealable>().OnDied += (args, sender) => {
            cursorLockHandler.Disable();
            gameOverMenu.Show("You Died");
        };

        gameOverMenu.Setup(
            "Retry",
            () => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex)
        );
    }
}
