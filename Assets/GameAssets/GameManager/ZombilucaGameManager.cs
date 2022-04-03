using Assets.GameAssets.Player;
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
        private readonly FirstPersonController player;
        private readonly CinemachineVirtualCamera vCamera;
        private readonly CursorLockHandler cursorLockHandler;
        private readonly GameOverMenu gameOverMenu;

        public ZombilucaGameManager(
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

            player.GetComponent<IHealable>().OnDied
                += (args, sender) => {
                    Camera.main.gameObject.SetActive(false);
                    AsyncProcessor.Instance.StartCoroutine(GameOverAsync());
                };

            gameOverMenu.Setup(
                "Retry",
                () => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex)
            );
        }

        private IEnumerator GameOverAsync()
        {
            yield return new WaitForSeconds(6f);
            cursorLockHandler.Disable();
            gameOverMenu.Show("You Died");
        }
    }
}