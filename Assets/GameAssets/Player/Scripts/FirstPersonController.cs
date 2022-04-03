using Assets.UnityFoundation.Systems.Character3D.Scripts;
using Assets.UnityFoundation.Systems.HealthSystem;
using UnityEngine;
using UnityFoundation.Code.PhysicsUtils;
using Zenject;

namespace Assets.GameAssets.Player
{
    public class FirstPersonController : BaseCharacter3D
    {
        public FirstPersonInputs Inputs { get; private set; }
        public FirstPersonAnimationController AnimController { get; private set; }
        public AudioSource AudioSource { get; private set; }
        public Rigidbody Rigidbody { get; private set; }
        public IHealable Health { get; private set; }

        public bool IsGrounded => checkGroundHandler.IsGrounded;

        [Inject] public PlayerSettings Settings { get; }
        [Inject] public IdlePlayerState IdlePlayerState;
        [Inject] public WalkPlayerState WalkPlayerState;
        [Inject] public AimPlayerState AimState;

        private CheckGroundHandler checkGroundHandler;
        private Camera mainCamera;

        public Transform WeaponShootPoint;

        [Inject]
        public void Init(
            FirstPersonInputs inputs,
            FirstPersonAnimationController animController,
            [Inject(Id = AudioSources.PlayerWeapon)] AudioSource audioSource,
            CheckGroundHandler checkGroundHandler,
            Camera mainCamera
        )
        {
            Inputs = inputs;
            AnimController = animController;
            AudioSource = audioSource;

            this.checkGroundHandler = checkGroundHandler.DebugMode(true);
            checkGroundHandler.OnLanded += OnLandedHandler;

            this.mainCamera = mainCamera;
        }

        private void OnLandedHandler()
        {
            AudioSource.PlayOneShot(Settings.LandAudioClip);
        }

        protected override void OnStart()
        {
            Inputs.Enable();
            Rigidbody = GetComponent<Rigidbody>();

            Health = GetComponent<IHealable>();
            Health.Setup(Settings.StartHealth);
            Health.OnDied += OnDied;

            TransitionToState(IdlePlayerState);
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

        protected override void OnUpdate()
        {
            checkGroundHandler.CheckGround();

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
            if(!IsGrounded) return;

            if(!Inputs.Jump) return;

            // TODO: corrigir para só chamar o add force uma única vez
            AudioSource.PlayOneShot(Settings.JumpAudioClip);
            Rigidbody.AddForce(Vector3.up * 5f, ForceMode.Impulse);
        }
    }
}