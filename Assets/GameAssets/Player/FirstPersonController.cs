using Assets.UnityFoundation.Code;
using Cinemachine;
using System;
using UnityEngine;
using Zenject;

public class FirstPersonController : MonoBehaviour
{
    // TODO: transformar essas configurações em settings
    [Header("Player")]

    [Tooltip("Move speed of the character in m/s")]
    public float MoveSpeed = 2.0f;

    [Space(10)]
    [Tooltip("The height the player can jump")]
    public float JumpHeight = 1.2f;

    [Space(10)]
    [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
    public float JumpTimeout = 0.50f;

    private FirstPersonInputs inputs;
    private FirstPersonAnimationController animController;
    private Camera mainCamera;
    private Rigidbody rigBody;

    private CapsuleCollider capsuleCollider;

    private bool isGrounded;

    [Inject]
    public void Init(
        FirstPersonInputs inputs,
        FirstPersonAnimationController animController,
        Camera mainCamera
    )
    {
        this.inputs = inputs;
        this.animController = animController;
        this.mainCamera = mainCamera;
    }

    private void Start()
    {
        inputs.Enable();
        rigBody = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    private void Update()
    {
        CheckGround();
        Rotate();
        Move();
        TryJump();
        TryAim();
        TryFire();
        TryReload();
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

    private bool CheckGround()
    {
        isGrounded = Physics.Raycast(
            capsuleCollider.bounds.center,
            transform.Down(),
            capsuleCollider.height + 0.05f
        );

        Debug.DrawRay(
            capsuleCollider.bounds.center,
            transform.Down() * (capsuleCollider.height + 0.05f),
            isGrounded ? Color.green : Color.red
        );

        return isGrounded;
    }

    private void Rotate()
    {
        transform.rotation = Quaternion.Euler(0f, mainCamera.transform.eulerAngles.y, 0f);
    }

    private void Move()
    {
        if(!isGrounded) return;

        if(inputs.Move == Vector2.zero)
            return;

        var targetDirection = new Vector3(inputs.Move.x, 0f, inputs.Move.y).normalized;
        var newPos = transform.forward * targetDirection.z + transform.right * targetDirection.x;
        transform.position += newPos * MoveSpeed * Time.deltaTime;
    }

    private void TryJump()
    {
        if(!isGrounded) return;

        if(!inputs.Jump) return;

        rigBody.AddForce(Vector3.up * 2f, ForceMode.Impulse);
    }
}
