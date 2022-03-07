using Assets.UnityFoundation.Code.PhysicsUtils;
using Assets.UnityFoundation.Systems.Character3D.Scripts;
using UnityEngine;
using Zenject;

public class FirstPersonController : BaseCharacter3D
{
    private FirstPersonInputs inputs;
    private FirstPersonAnimationController animController;
    private AudioSource audioSource;
    private CheckGroundHandler checkGroundHandler;
    private Rigidbody rigBody;

    public bool IsGrounded => checkGroundHandler.IsGrounded;

    [Inject] private readonly PlayerSettings settings;
    [Inject] public IdlePlayerState IdlePlayerState;
    [Inject] public WalkPlayerState WalkPlayerState;

    [Inject]
    public void Init(
        FirstPersonInputs inputs,
        FirstPersonAnimationController animController,
        [Inject(Id = AudioSources.PlayerWeapon)] AudioSource audioSource,
        CheckGroundHandler checkGroundHandler
    )
    {
        this.inputs = inputs;
        this.animController = animController;
        this.audioSource = audioSource;
        this.checkGroundHandler = checkGroundHandler.DebugMode(true);

        checkGroundHandler.OnLanded += OnLandedHandler;
    }

    private void OnLandedHandler()
    {
        audioSource.PlayOneShot(settings.LandAudioClip);
    }

    protected override void OnStart()
    {
        inputs.Enable();
        rigBody = GetComponent<Rigidbody>();

        TransitionToState(IdlePlayerState);
    }

    protected override void OnUpdate()
    {
        checkGroundHandler.CheckGround();
        TryJump();

        // TODO: separar as classes de movimento e mira já que 
        // são classes que resolvem problemas distintos
        TryReload();
        TryFire();
        TryAim();
    }

    private void TryReload()
    {
        if(!inputs.Reload) return;

        animController.Reload();
    }

    private void TryFire()
    {
        if(!inputs.Fire) return;

        animController.Fire();
    }

    private void TryAim()
    {
        if(!inputs.Aim) return;

        animController.Aim();
    }

    protected override void OnTriggerAnimationEvent(string name)
    {
        base.OnTriggerAnimationEvent(name);

        if(name == "fire")
            audioSource.PlayOneShot(settings.FireSFX);
    }

    private void TryJump()
    {
        if(!IsGrounded) return;

        if(!inputs.Jump) return;

        // TODO: corrigir para só chamar o add force uma única vez
        audioSource.PlayOneShot(settings.JumpAudioClip);
        rigBody.AddForce(Vector3.up * 5f, ForceMode.Impulse);
    }
}
