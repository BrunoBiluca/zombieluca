using Assets.GameAssets.FirstPersonModeSystem;
using Assets.UnityFoundation.Systems.HealthSystem;
using UnityEngine;
using Zenject;

namespace Assets.GameAssets.Player
{
    public class ZombilucaPlayer : MonoBehaviour
    {
        public IHealable Health { get; private set; }

        [Inject] public ZombilucaPlayerSettings Settings { get; }
        [Inject] public FirstPersonMode FirstPersonMode { get; private set; }
        [Inject] public SignalBus SignalBus { get; private set; }

        protected void Start()
        {
            Health = GetComponent<IHealable>();
            Health.Setup(Settings.StartHealth);
            Health.OnDied += OnDied;

            FirstPersonMode.OnShotHit += () => SignalBus.Fire<HitShotSignal>();
        }

        private void OnDied(object sender, System.EventArgs e)
        {
            var model = Instantiate(
                Settings.PlayerFullModel, 
                new Vector3(
                    transform.position.x, 
                    Terrain.activeTerrain.SampleHeight(transform.position),
                    transform.position.z
                ),
                transform.rotation
            );

            model.GetComponent<Animator>().SetTrigger("Death");
            Destroy(gameObject);
        }

    }
}