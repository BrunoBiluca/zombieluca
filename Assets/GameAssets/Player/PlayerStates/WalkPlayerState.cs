using Assets.UnityFoundation.Systems.Character3D.Scripts;
using UnityEngine;

public class WalkPlayerState : BaseCharacterState3D
{
    private readonly FirstPersonController controller;
    private readonly PlayerSettings playerSettings;
    private readonly Transform transform;
    private readonly FirstPersonAnimationController animController;
    private readonly FirstPersonInputs inputs;
    private readonly Camera mainCamera;

    public WalkPlayerState(
        FirstPersonController controller,
        PlayerSettings playerSettings,
        FirstPersonAnimationController animController,
        FirstPersonInputs inputs,
        Camera mainCamera
    ){
        this.controller = controller;
        this.playerSettings = playerSettings;
        this.transform = controller.transform;

        this.animController = animController;
        this.inputs = inputs;
        this.mainCamera = mainCamera;
    }

    public override void EnterState()
    {
        animController.Walking(true);
    }

    public override void Update()
    {
        // TODO: esse código de rotate está repetido, 
        // pensar e uma forma de compartilhar implementações entre estados
        Rotate();

        if(inputs.Move == Vector2.zero){
            controller.TransitionToState(controller.idlePlayerState);
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
}