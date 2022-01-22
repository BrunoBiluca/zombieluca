using Assets.UnityFoundation.Systems.Character3D.Scripts;
using UnityEngine;

public class IdlePlayerState : BaseCharacterState3D
{
    private readonly PlayerSettings playerSettings;
    private readonly FirstPersonController controller;
    private readonly FirstPersonInputs inputs;
    private readonly FirstPersonAnimationController animController;
    private readonly Camera mainCamera;
    private Transform transform;

    public IdlePlayerState(
        PlayerSettings playerSettings,
        FirstPersonController controller,
        FirstPersonInputs inputs,
        FirstPersonAnimationController animController,
        Camera mainCamera
    )
    {
        this.playerSettings = playerSettings;

        this.controller = controller;
        this.transform = controller.transform;

        this.inputs = inputs;
        this.animController = animController;
        this.mainCamera = mainCamera;
    }

    public override void EnterState()
    {
        animController.Walking(false);
    }

    public override void Update()
    {
        Rotate();
        Move();
    }

    private void Rotate()
    {
        transform.rotation = Quaternion.Euler(0f, mainCamera.transform.eulerAngles.y, 0f);
    }

    private void Move()
    {
        if(!controller.IsGrounded) return;

        if(inputs.Move == Vector2.zero) return;

        // TODO: utilizar factores para criar esses estados
        // criar uma factory com todos os estados do controller, 
        // dessa forma não precisamos de usar referências públicas
        controller.TransitionToState(controller.WalkPlayerState);
    }
}
