using Assets.GameAssets.Compass;
using Assets.GameAssets.FirstPersonModeSystem;
using Assets.GameAssets.Player;
using Assets.GameAssets.Radar;
using Assets.UnityFoundation.Systems.HealthSystem;
using Assets.UnityFoundation.UI.Menus.GameOverMenu;
using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityFoundation.Code;
using Zenject;

namespace Assets.GameAssets.GameManager
{
    public class ZombilucaGameManager : IInitializable
    {
        private readonly ZombilucaPlayer player;
        private readonly CinemachineVirtualCamera vCamera;
        private readonly CursorLockHandler cursorLockHandler;
        private readonly GameOverMenu gameOverMenu;
        private readonly RadarView radar;
        private readonly CompassView compass;

        public ZombilucaGameManager(
            ZombilucaPlayer player,
            CinemachineVirtualCamera vCamera,
            CursorLockHandler cursorLockHandler,
            GameOverMenu gameOverMenu,
            RadarView radar,
            CompassView compass
        )
        {
            this.player = player;
            this.vCamera = vCamera;
            this.cursorLockHandler = cursorLockHandler;
            this.gameOverMenu = gameOverMenu;
            this.radar = radar;
            this.compass = compass;
        }

        public void Initialize()
        {
            cursorLockHandler.Enable();

            vCamera.Follow = player.transform.Find("camera_reference");

            var spawnPoint = GameObject.Find("player_spawn_point");
            player.transform.position = spawnPoint.transform.position;

            player.GetComponent<IHealable>().OnDied
                += (args, sender) => {
                    Camera.main.gameObject.SetActive(false);
                    AsyncProcessor.Instance.StartCoroutine(GameOverAsync());
                };

            gameOverMenu.Setup(
                "Retry",
                () => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex)
            );

            radar.Setup(player.transform);

            compass.Setup(player.transform);
        }

        private IEnumerator GameOverAsync()
        {
            yield return new WaitForSeconds(6f);
            cursorLockHandler.Disable();
            gameOverMenu.Show("You Died");
        }
    }
}