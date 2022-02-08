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
    private CapsuleCollider capsuleCollider;

    public bool IsGrounded => checkGroundHandler.IsGrounded;

    [Inject] private PlayerSettings settings;
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
        this.checkGroundHandler = checkGroundHandler;
    }

    private void Start()
    {
        inputs.Enable();
        rigBody = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();

        TransitionToState(IdlePlayerState);
    }

    protected override void OnUpdate()
    {
        checkGroundHandler.CheckGround();
        TryJump();

        // TODO: separar as classes de movimento e mira j� que 
        // s�o classes que resolvem problemas distintos
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

        // TODO: corrigir para s� chamar o add force uma �nica vez
        rigBody.AddForce(Vector3.up, ForceMode.Impulse);
    }

    public class Factory : PlaceholderFactory<FirstPersonController> {

    }
}
