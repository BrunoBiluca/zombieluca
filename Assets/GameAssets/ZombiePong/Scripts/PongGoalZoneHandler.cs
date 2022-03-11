using UnityEngine;

namespace Assets.GameAssets.ZombiePong
{
    public class PongGoalZoneHandler : MonoBehaviour
    {
        public bool IsLeft = true;

        private void OnTriggerEnter(Collider other)
        {
            if(!other.gameObject.TryGetComponent(out BallHandler ball))
                return;

            ball.Deactivate();
            PongGameManager.Instance.AddScore(IsLeft ? 1 : 0);
        }
    }
}