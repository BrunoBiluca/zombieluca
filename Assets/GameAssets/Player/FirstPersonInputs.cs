using Assets.UnityFoundation.EditorInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class FirstPersonInputs : MonoBehaviour
{
    [SerializeField] [ShowOnly] private Vector2 move;
    public Vector2 Move => move;

    private FirstPersonInputActions inputActions;

    private void Awake() {
        inputActions = new FirstPersonInputActions();

        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.started += OnMove;
        inputActions.Player.Move.canceled += OnMove;

        inputActions.Enable();
    }

    private void OnMove(CallbackContext ctx) => move = ctx.ReadValue<Vector2>();
}
