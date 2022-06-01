using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityFoundation.Code;

namespace Assets.GameAssets.UI.MainMenu
{
    public class GameScenes
    {
        public static string GamePlayDemo => "player_gameplay_demo";
        public static string MinigameZombiePong => "zombie_pong";
    }

    public class MainMenuScreen : MonoBehaviour
    {
        public void Awake()
        {
            transform
                .FindComponent<Button>(
                    "background",
                    "buttons_holder",
                    "gameplay_demo_button"
                )
                .onClick
                .AddListener(() => SceneManager.LoadScene(GameScenes.GamePlayDemo));

            transform
                .FindComponent<Button>(
                    "background",
                    "buttons_holder",
                    "minigame_button"
                )
                .onClick
                .AddListener(() => SceneManager.LoadScene(GameScenes.MinigameZombiePong));
        }
    }
}