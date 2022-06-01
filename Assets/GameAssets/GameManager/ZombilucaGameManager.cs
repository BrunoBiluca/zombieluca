using Assets.GameAssets.Player;
using Assets.UnityFoundation.Systems.HealthSystem;
using Cinemachine;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityFoundation.Code;
using UnityFoundation.Compass;
using UnityFoundation.FirstPersonModeSystem;
using UnityFoundation.Radar;
using UnityFoundation.UI.Menus.GameOverMenu;
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
        private readonly GameFinishPoint finishPoint;
        private readonly SignalBus signalBus;

        public ZombilucaGameManager(
            ZombilucaPlayer player,
            CinemachineVirtualCamera vCamera,
            CursorLockHandler cursorLockHandler,
            GameOverMenu gameOverMenu,
            RadarView radar,
            CompassView compass,
            GameFinishPoint finishPoint,
            SignalBus signalBus
        )
        {
            this.player = player;
            this.vCamera = vCamera;
            this.cursorLockHandler = cursorLockHandler;
            this.gameOverMenu = gameOverMenu;
            this.radar = radar;
            this.compass = compass;
            this.finishPoint = finishPoint;
            this.signalBus = signalBus;
        }

        public void Initialize()
        {
            cursorLockHandler.Enable();

            vCamera.Follow = player.transform.Find("camera_reference");

            var spawnPoint = GameObject.Find("player_spawn_point");
            player.transform.position = spawnPoint.transform.position;

            player.GetComponent<IHealable>().OnDied
                += (args, sender) => {
                    AsyncProcessor.Instance.StartCoroutine(GameOverAsync());
                };

            gameOverMenu.Setup(
                "Retry",
                () => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex)
            );

            radar.Setup(player.transform);

            compass.Setup(player.transform);

            finishPoint.Hide();

            signalBus.Subscribe<OnGameFinished>(VictoryGameOver);
        }

        private void VictoryGameOver()
        {
            AsyncProcessor.Instance.StartCoroutine(VictoryGameOverAsync());
        }

        private IEnumerator VictoryGameOverAsync()
        {
            yield return new WaitForSeconds(6f);
            cursorLockHandler.Disable();
            gameOverMenu.Show("Victory");
        }


        private IEnumerator GameOverAsync()
        {
            yield return new WaitForSeconds(6f);
            cursorLockHandler.Disable();
            gameOverMenu.Show("You Died");
        }
    }
}