using Assets.UnityFoundation.Code;
using Assets.UnityFoundation.Systems.Character3D.Scripts;
using UnityEngine;
using Zenject;

// TODO: FirstPersonController não é o player, é só um modo de controle do jogo.
// o player pode alterar entre first person e outro modo, 
// logo temos que separar a classe de controle da classe de player
public class FirstPersonController : BaseCharacter3D
{
    private FirstPersonInputs inputs;
    private FirstPersonAnimationController animController;
    private AudioSource audioSource;
    private Rigidbody rigBody;
    private CapsuleCollider capsuleCollider;

    public bool IsGrounded { get; private set; }

    [Inject] private PlayerSettings settings;
    [Inject] public IdlePlayerState IdlePlayerState;
    [Inject] public WalkPlayerState WalkPlayerState;

    [Inject]
    public void Init(
        FirstPersonInputs inputs,
        FirstPersonAnimationController animController,
        [Inject(Id = AudioSources.PlayerWeapon)] AudioSource audioSource
    )
    {
        this.inputs = inputs;
        this.animController = animController;
        this.audioSource = audioSource;
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
        CheckGround();
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

    private bool CheckGround()
    {
        IsGrounded = Physics.Raycast(
            capsuleCollider.bounds.center,
            transform.Down(),
            capsuleCollider.height + 0.05f
        );

        Debug.DrawRay(
            capsuleCollider.bounds.center,
            transform.Down() * (capsuleCollider.height + 0.05f),
            IsGrounded ? Color.green : Color.red
        );

        return IsGrounded;
    }

    private void TryJump()
    {
        if(!IsGrounded) return;

        if(!inputs.Jump) return;

        rigBody.AddForce(Vector3.up * 2f, ForceMode.Impulse);
    }

    public class Factory : PlaceholderFactory<FirstPersonController> {

    }
}
