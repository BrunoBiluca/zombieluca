using Assets.GameAssets.AmmoStorageSystem;
using Assets.UnityFoundation.Systems.Character3D.Scripts;
using Assets.UnityFoundation.Systems.HealthSystem;
using UnityEngine;
using Zenject;

namespace Assets.GameAssets.Player
{
    public class AimPlayerState : BaseCharacterState3D
    {
        private readonly FirstPersonController player;
        private readonly SignalBus signalBus;
        private readonly IAmmoStorage ammoStorage;

        public AimPlayerState(
            FirstPersonController player,
            SignalBus signalBus
        )
        {
            this.player = player;
            this.signalBus = signalBus;
            ammoStorage = player.GetComponent<AmmoStorageMonoBehaviour>();
        }

        public override void EnterState()
        {
            player.AnimController.Aim();
        }

        public override void Update()
        {
            player.Rotate();
            TryReload();
            TryFire();
            player.Move();
        }

        private void TryReload()
        {
            if(!player.Inputs.Reload) return;

            player.AnimController.Reload();
        }

        private void TryFire()
        {
            if(!player.Inputs.Fire) return;

            player.AnimController.Fire();
        }

        public override void TriggerAnimationEvent(string name)
        {
            if(name == "fire")
                Fire();
        }

        private void Fire()
        {
            var ammo = ammoStorage.GetAmmo(1);
            if(ammo == 0)
            {
                player.AudioSource.PlayOneShot(player.Settings.FireMissSFX);
                return;
            }
            
            player.AudioSource.PlayOneShot(player.Settings.FireSFX);
            var ray = new Ray(
                player.WeaponShootPoint.position, player.WeaponShootPoint.forward
            );
            if(Physics.Raycast(ray, out RaycastHit hit, float.MaxValue))
            {
                var damageable = hit.transform.GetComponent<IDamageable>();

                if(damageable != null)
                {
                    damageable.Damage(5f);
                    signalBus.Fire<HitShotSignal>();
                }
            }
        }
    }
}