using Assets.GameAssets.AmmoStorageSystem;
using Assets.GameAssets.FirstPersonModeSystem;
using Assets.UnityFoundation.Systems.HealthSystem;
using Assets.UnityFoundation.UnityAdapter;
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
        [Inject] public IHealthBar HealthBar { get; private set; }
        [Inject] public IAmmoStorage AmmoStorage { get; private set; }

        protected void Start()
        {
            var healthSystem = GetComponent<HealthSystem>();
            healthSystem.SetHealthBar(HealthBar);

            Health = healthSystem;
            Health.Setup(Settings.StartHealth);
            Health.OnDied += OnDied;

            FirstPersonMode.Rigidbody = new RidigbodyDecorator(GetComponent<Rigidbody>());
            FirstPersonMode.OnShotHit += (point) => {
                var blood = Instantiate(Settings.BloodVFX, point, Quaternion.identity);
                blood.transform.LookAt(transform.position);
                Destroy(blood, 0.5f);
                SignalBus.Fire<PlayerHitShotSignal>();
            };
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