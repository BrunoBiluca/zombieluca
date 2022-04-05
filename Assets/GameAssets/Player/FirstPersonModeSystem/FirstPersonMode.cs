using Assets.UnityFoundation.Systems.Character3D.Scripts;
using Assets.UnityFoundation.UnityAdapter;
using System;
using UnityEngine;
using UnityFoundation.Code.PhysicsUtils;
using Zenject;

namespace Assets.GameAssets.FirstPersonModeSystem
{
    public class FirstPersonMode : BaseCharacter3D
    {
        public FirstPersonModeSettings Settings { get; private set; }
        public FirstPersonInputs Inputs { get; private set; }
        public CheckGroundHandler CheckGroundHandler { get; private set; }
        public FirstPersonAnimationController AnimController { get; private set; }
        public IAudioSource AudioSource { get; private set; }
        public Rigidbody Rigidbody { get; private set; }
        public Transform WeaponShootPoint;

        public IdlePlayerState IdlePlayerState;
        public WalkPlayerState WalkPlayerState;
        public AimPlayerState AimState;

        public event Action OnShotHit;

        private Camera mainCamera;

        [Inject]
        public FirstPersonMode Setup(
            FirstPersonModeSettings settings,
            FirstPersonInputs inputs,
            CheckGroundHandler checkGroundHandler,
            FirstPersonAnimationController animController,
            IAudioSource audioSource,
            Camera camera
        )
        {
            Settings = settings;
            Inputs = inputs;
            CheckGroundHandler = checkGroundHandler;
            AnimController = animController;
            AudioSource = audioSource;
            mainCamera = camera;

            IdlePlayerState = new IdlePlayerState(this);
            WalkPlayerState = new WalkPlayerState(this);
            AimState = new AimPlayerState(this);

            CheckGroundHandler.OnLanded += OnLandedHandler;
            return this;
        }

        private void OnLandedHandler()
        {
            AudioSource.PlayOneShot(Settings.LandAudioClip);
        }

        protected override void OnStart()
        {
            Inputs.Enable();
            Rigidbody = GetComponent<Rigidbody>();

            TransitionToState(IdlePlayerState);
        }

        protected override void OnUpdate()
        {
            CheckGroundHandler.CheckGround();

            if(TryAim())
                return;

            TryJump();
        }

        private bool TryAim()
        {
            if(!Inputs.Aim) return false;

            if(CurrentState != AimState)
                TransitionToState(AimState);
            else
                AnimController.ToggleAim();
            return true;
        }

        public void ShootHit()
        {
            OnShotHit?.Invoke();
        }

        public void Rotate()
        {
            transform.rotation = Quaternion.Euler(
                mainCamera.transform.eulerAngles.x, mainCamera.transform.eulerAngles.y, 0f
            );
        }

        public void Move()
        {
            var targetDirection = new Vector3(Inputs.Move.x, 0f, Inputs.Move.y).normalized;

            AnimController.Walking(targetDirection.magnitude > 0f);

            var newPos = transform.forward * targetDirection.z
                + transform.right * targetDirection.x;
            transform.position += Settings.MoveSpeed * Time.deltaTime * newPos;
        }

        public void TryJump()
        {
            if(!CheckGroundHandler.IsGrounded) return;

            if(!Inputs.Jump) return;

            // TODO: corrigir para só chamar o add force uma única vez
            AudioSource.PlayOneShot(Settings.JumpAudioClip);
            Rigidbody.AddForce(Vector3.up * 5f, ForceMode.Impulse);
        }
    }
}