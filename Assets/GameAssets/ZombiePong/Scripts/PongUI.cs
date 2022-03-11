using UnityFoundation.Code;
using TMPro;

namespace Assets.GameAssets.ZombiePong
{
    public class PongUI : Singleton<PongUI>
    {
        private TextMeshProUGUI paddleLeftText;
        private TextMeshProUGUI paddleRightText;

        protected override void OnAwake()
        {
            paddleLeftText = transform.FindComponent<TextMeshProUGUI>("paddle_left_score_text");
            paddleRightText = transform.FindComponent<TextMeshProUGUI>("paddle_right_score_text");

            paddleLeftText.text = "0";
            paddleRightText.text = "0";
        }

        public void Start()
        {
            PongGameManager.Instance.OnPlayerScores += (playerIndex, playerScore) => {
                if(playerIndex == 0)
                    paddleLeftText.text = playerScore.ToString();
                else if(playerIndex == 1)
                    paddleRightText.text = playerScore.ToString();
            };
        }
    }
}