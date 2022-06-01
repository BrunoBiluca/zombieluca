using Assets.GameAssets.Player;
using UnityEngine;
using Zenject;

namespace Assets.GameAssets.GameManager
{
    public class OnGameFinished { }

    public class GameFinishPoint : MonoBehaviour
    {
        [Inject] private readonly SignalBus signalBus;

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            signalBus.Subscribe<OnZombiesDied>(Show);
            gameObject.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.TryGetComponent(out ZombilucaPlayer player))
            {
                player.VictoryDance();
                signalBus.Fire<OnGameFinished>();
            }
            gameObject.SetActive(false);
        }
    }
}