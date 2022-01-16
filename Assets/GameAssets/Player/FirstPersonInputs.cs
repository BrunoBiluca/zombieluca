using System;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using Zenject;
using static UnityEngine.InputSystem.InputAction;

public class FirstPersonInputs
{
    public Vector2 Move { get; private set; }
    public bool Jump { get; private set; }
    public Vector2 Look => inputActions.Player.Look.ReadValue<Vector2>();

    private FirstPersonInputActions inputActions;

    public FirstPersonInputs(FirstPersonInputActions inputActions)
    {
        this.inputActions = inputActions;
    }

    public void Enable()
    {
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.started += OnMove;
        inputActions.Player.Move.canceled += OnMove;

        inputActions.Player.Jump.performed += OnJump;
        inputActions.Player.Jump.started += OnJump;
        inputActions.Player.Jump.canceled += OnJump;

        inputActions.Enable();
    }
    private void OnMove(CallbackContext ctx) => Move = ctx.ReadValue<Vector2>();

    private void OnJump(CallbackContext ctx) => Jump = ctx.ReadValueAsButton();
}
