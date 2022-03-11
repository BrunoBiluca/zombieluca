using UnityFoundation.Code.TimeUtils;
using Assets.UnityFoundation.Systems.Character3D.Scripts;
using UnityEngine;
using Zenject;

public class WalkPlayerState : BaseCharacterState3D
{
    private readonly FirstPersonController controller;
    private readonly PlayerSettings playerSettings;
    private readonly Transform transform;
    private readonly FirstPersonAnimationController animController;
    private readonly FirstPersonInputs inputs;
    private readonly Camera mainCamera;
    private readonly AudioSource walkStepAudio;
    private Timer walkStepTimer;

    public WalkPlayerState(
        FirstPersonController controller,
        PlayerSettings playerSettings,
        FirstPersonAnimationController animController,
        FirstPersonInputs inputs,
        Camera mainCamera,
        [Inject(Id = AudioSources.PlayerMovement)] AudioSource audioSource
    ){
        this.controller = controller;
        this.playerSettings = playerSettings;
        this.transform = controller.transform;

        this.animController = animController;
        this.inputs = inputs;
        this.mainCamera = mainCamera;
        this.walkStepAudio = audioSource;

        // TODO: Passar essa instanciação do timer para um factory do DI
        UpdateWalkingStepClip();
        walkStepTimer = new Timer(0.4f, UpdateWalkingStepClip).Loop();
    }

    private void UpdateWalkingStepClip()
    {
        var clipIdx = UnityEngine.Random.Range(0, playerSettings.WalkingStepsSFX.Count - 1);
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
        // TODO: esse código de rotate está repetido, 
        // pensar e uma forma de compartilhar implementações entre estados
        Rotate();

        if(inputs.Move == Vector2.zero){
            controller.TransitionToState(controller.IdlePlayerState);
            return;
        }

        Move();
    }

    private void Rotate()
    {
        transform.rotation = Quaternion.Euler(0f, mainCamera.transform.eulerAngles.y, 0f);
    }

    private void Move()
    {
        var targetDirection = new Vector3(inputs.Move.x, 0f, inputs.Move.y).normalized;
        var newPos = transform.forward * targetDirection.z + transform.right * targetDirection.x;
        transform.position += newPos * playerSettings.MoveSpeed * Time.deltaTime;
    }

    public override void ExitState()
    {
        walkStepTimer.Close();
        walkStepAudio.Stop();
    }
}