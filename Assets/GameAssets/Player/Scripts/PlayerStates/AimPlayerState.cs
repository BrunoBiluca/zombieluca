using Assets.UnityFoundation.Systems.Character3D.Scripts;

namespace Assets.GameAssets.Player
{
    public class AimPlayerState : BaseCharacterState3D
    {
        private readonly FirstPersonController player;

        public AimPlayerState(FirstPersonController player)
        {
            this.player = player;
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
                player.AudioSource.PlayOneShot(player.Settings.FireSFX);
        }

    }
}