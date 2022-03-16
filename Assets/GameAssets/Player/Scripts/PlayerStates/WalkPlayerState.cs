using UnityFoundation.Code.TimeUtils;
using Assets.UnityFoundation.Systems.Character3D.Scripts;
using UnityEngine;
using Zenject;

namespace Assets.GameAssets.Player
{
    public class WalkPlayerState : BaseCharacterState3D
    {
        private readonly FirstPersonController controller;
        private readonly PlayerSettings playerSettings;
        private readonly FirstPersonAnimationController animController;
        private readonly FirstPersonInputs inputs;
        private readonly AudioSource walkStepAudio;
        private Timer walkStepTimer;

        public WalkPlayerState(
            FirstPersonController controller,
            PlayerSettings playerSettings,
            FirstPersonAnimationController animController,
            FirstPersonInputs inputs,
            [Inject(Id = AudioSources.PlayerMovement)] AudioSource audioSource
        )
        {
            this.controller = controller;
            this.playerSettings = playerSettings;

            this.animController = animController;
            this.inputs = inputs;
            walkStepAudio = audioSource;

            // TODO: Passar essa instanciação do timer para um factory do DI
            UpdateWalkingStepClip();
            walkStepTimer = new Timer(0.4f, UpdateWalkingStepClip).Loop();
        }

        private void UpdateWalkingStepClip()
        {
            var clipIdx = Random.Range(0, playerSettings.WalkingStepsSFX.Count - 1);
            walkStepAudio.clip = playerSettings.WalkingStepsSFX[clipIdx];
            walkStepAudio.Play();
            walkStepAudio.loop = true;
        }

        public override void EnterState()
        {
            animController.Walking(true);

            walkStepTimer.Start();
            walkStepAudio.Play();
        }

        public override void Update()
        {
            controller.Rotate();

            if(inputs.Move == Vector2.zero)
            {
                controller.TransitionToState(controller.IdlePlayerState);
                return;
            }

            controller.Move();
        }

        public override void ExitState()
        {
            walkStepTimer.Close();
            walkStepAudio.Stop();
        }
    }
}